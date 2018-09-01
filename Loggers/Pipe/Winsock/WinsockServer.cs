using NAccLogger.Impl;
using NAccLogger.Itf;
using NAccLogger.Loggers.Pipe.Winsock.Com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// winsock pipe logger server
    /// </summary>
    public class WinsockServer
        : LogBase,
        IServer
    {
        #region attributes

        /// <summary>
        /// debug logger
        /// </summary>
        protected readonly ILog DebugLog = null;

        bool _IsDebugLogEnabled = true;

        /// <summary>
        /// disable or enable (if setted) the private debug logger
        /// </summary>
        protected bool IsDebugLogEnabled
        {
            get
            {
                return _IsDebugLogEnabled && DebugLog != null;
            }
            set
            {
                _IsDebugLogEnabled = value;
            }
        }

        /// <summary>
        /// server context settings
        /// </summary>
        protected WinsockServerContext SocketServerContext;

        public IPHostEntry IpHostInfo { get; protected set; }
        public IPEndPoint LocalEndPoint { get; protected set; }

        protected Socket Socket;
        protected Task ConnectionListenerTask = null;
        protected Socket Listener = null;

        protected bool _IsRunning = true;
        public bool IsRunning
        {
            get
            {
                return _IsRunning;
            }
            set
            {
                _IsRunning = value;
            }
        }

        /// <summary>
        /// thread signal
        /// </summary>
        public static ManualResetEvent AllDone = new ManualResetEvent(false);
        
        /// <summary>
        /// client handlers of connected clients
        /// </summary>
        protected BindingList<ClientHandler> Clients = new BindingList<ClientHandler>();

        /// <summary>
        /// command handler
        /// </summary>
        protected CommandHandler CommandHandler = new CommandHandler();

        #endregion

        /// <summary>
        /// build a new pipe socket server allowing to dispatch log items over the network
        /// <para>Exceptions:</para>
        /// <para>The combination of addressFamily, socketType, and protocolType results in an invalid socket</para>
        /// </summary>
        /// <param name="socketServerContext">server context settings</param>
        /// <param name="logParameters">log parameters</param>        
        /// <param name="debugLog">private debug logger for debug purposes</param>
        /// <param name="isDebugLogEnabled">enabled/disable server private logger</param>
        public WinsockServer(
            WinsockServerContext socketServerContext = null,
            LogParameters logParameters = null,
            ILog debugLog = null,
            bool isDebugLogEnabled = true
            ) : base(logParameters)
        {
            this.DebugLog = debugLog;
            this.IsDebugLogEnabled = isDebugLogEnabled;
            this.SocketServerContext = socketServerContext ?? new WinsockServerContext();
            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)
                    ?.T($"starting pipe winsock logger server on port {SocketServerContext.PortNumber}");
            ConnectionListenerTask = Task.Run(() => StartListening() );
        }

        protected void StartListening()
        {
            // Establish the local endpoint for the socket.  

            IpHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            /*;
            IpAddress = IpHostInfo.AddressList[0];*/
            LocalEndPoint = new IPEndPoint(
                SocketServerContext.IPAddress, 
                SocketServerContext.PortNumber);

            // Create the socket
            Listener = new Socket(
                SocketServerContext.IPAddress.AddressFamily,
                SocketType.Stream,
                SocketServerContext.ProtocolType)
            {                
                SendTimeout = SocketServerContext.SendTimeout,
                ReceiveTimeout = SocketServerContext.ReceiveTimeout
            };

            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)
                    ?.T($"pipe winsock logger server is listening on: host={IpHostInfo} address={SocketServerContext.IPAddress} port={SocketServerContext.PortNumber} protocole type={SocketServerContext.ProtocolType}");

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                Listener.Bind(LocalEndPoint);
                Listener.Listen(SocketServerContext.BackLog);

                while (IsRunning)
                {
                    // Set the event to nonsignaled state.  
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections
                    if (IsDebugLogEnabled)
                        DebugLog.Debug(LogCategory.Network)?.
                            T("waiting for a connection...");

                    Listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        Listener);

                    // wait until a connection is made before continuing
                    AllDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"pipe winsock logger server crashed due to error: '{e.Message}'");
                throw;
            }
        }

        /// <summary>
        /// accept a new client
        /// </summary>
        /// <param name="asyncResult">async result</param>
        public void AcceptCallback(IAsyncResult asyncResult)
        {            
            // Get the socket that handles the client request.  
            Socket listener = (Socket)asyncResult.AsyncState;
            Socket handler = listener.EndAccept(asyncResult);
            
            // Create the state object.  
            StateObject state = new StateObject
            {
                WorkSocket = handler
            };

            var clientHandler = new ClientHandler(
                state
                );

            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)?.
                    T($"new client accepted: {clientHandler}");

            // listen to the client ?
            clientHandler.ClientListeningTask = Task.Run(() =>
            {
                ClientListening(clientHandler);
            });

            Clients.Add(clientHandler);

            // Signal the main thread to continue.  
            AllDone.Set();
        }

        void ClientListening(ClientHandler clientHandler)
        {
            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)?.
                    T($"listening to client: {clientHandler}");

            var clientSocket = clientHandler.StateObject.WorkSocket;

            try
            {
                while (IsRunning)
                {
                    clientHandler.ReceiveDone.Reset();

                    clientSocket.BeginReceive(
                        clientHandler.StateObject.Buffer,
                        0,
                        StateObject.BufferSize,
                        0,
                        new AsyncCallback(ReceiveCallback),
                        clientHandler/*.StateObject*/);

                    clientHandler.ReceiveDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"pipe winsock logger failed to listen to client due to error: '{e.Message}'");
                throw;
            }
        }

        /// <summary>
        /// receive a message from a client
        /// </summary>
        /// <param name="ar">async result</param>
        protected void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  

                var clientHandler = (ClientHandler)ar.AsyncState;
                StateObject state = clientHandler.StateObject;
                var client = state.WorkSocket;

                // Read data from the remote device.  

                /*
                 * PROBLEME: ICI A RECU 2 MESSAGES EN 1
                 * cause: débit en entrée supérieur au rythme de consomation
                 * cas de test:
                 * message 1: 522 bytes
                 * message 2: 390 bytes
                 * reçu: 912 bytes (=522+390)
                 */

                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                    state.AppendBytes(
                        bytesRead
                        );

                // All the data has arrived; put it in response.  
                var r = state.GetMessage();

                // handle the message
                HandleMessage(clientHandler,r);

                // Signal that all bytes have been received.  
                clientHandler.ReceiveDone.Set();
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Warning(LogCategory.Network)?.
                        T($"receive message failed due to error: {e.Message}");
            }
        }

        protected void HandleMessage(
            ClientHandler clientHandler,
            byte[] message)
        {
            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)?.
                    T($"handle client message from: {clientHandler} nbytes={message.Length}");

            var coms = clientHandler
                .MessageSerializer
                .Unserialize<ICommandMessage>(message);

            foreach (var com in coms)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Debug(LogCategory.Network)?.
                        T($"handle command: {com}");

                CommandHandler
                    .HandleCommand(this, com);

                // send back the reply
                Send<ICommandMessage>(clientHandler, com);
            }
        }

#if obsolete
        /// <summary>
        /// send a string to client
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="data"></param>
        protected void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }
#endif

        /// <summary>
        /// send a log item to client
        /// </summary>
        /// <param name="cHandler">client handler</param>
        /// <param name="obj">log item</param>
        protected void Send<T>(
            ClientHandler cHandler, 
            T obj)
        {
            cHandler.SendDone.Reset();

            // get item as bytes
            var bytes = cHandler.MessageSerializer.Serialize<T>(obj);
            // transmit to client socket
            cHandler
                .StateObject
                .WorkSocket
                .BeginSend(bytes, 0, bytes.Length, 0,
                new AsyncCallback(
                    SendCallback),
                    cHandler
                        /*.StateObject
                        .WorkSocket*/);

            cHandler.SendDone.WaitOne();
        }

        /// <summary>
        /// send to client callback
        /// </summary>
        /// <param name="ar"></param>
        protected void SendCallback(IAsyncResult ar)
        {
            var clientHandler = (ClientHandler)ar.AsyncState;

            try
            {
                // Retrieve the socket from the state object.
                //Socket handler = (Socket)ar.AsyncState;
                var handler = clientHandler.StateObject.WorkSocket;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);

                if (IsDebugLogEnabled)
                    DebugLog.Debug(LogCategory.Network)?.
                        T($"Sent {bytesSent} bytes to client.");                
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"send message callback failed due to error: '{e.Message}'");
            }
            finally
            {
                clientHandler.SendDone.Set();
            }
        }

        /// <summary>
        /// add a new log entry to the pipe socket logger
        /// <para>dispatch throught network to connected clients</para>
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            var lst = new List<ClientHandler>();
            lst.AddRange(Clients);
            foreach (var c in lst)
            {
                try
                {
                    Send(c, logItem);
                }
                catch (SocketException se)
                {
                    if (IsDebugLogEnabled)
                        DebugLog.Warning(LogCategory.Network)?.
                            T($"send message failed due to error: '{se.Message} (error code={se.ErrorCode} socket native error code={se.NativeErrorCode} socket error code={se.SocketErrorCode}'");
                    if (se.ErrorCode == 10054)
                    {
                        if (IsDebugLogEnabled)
                            DebugLog.Debug(LogCategory.Network)?.
                                T($"disconnecting client: {c}");

                        // disconnect client
                        Clients.Remove(c);
                        c.Dispose();

                        if (IsDebugLogEnabled)
                            DebugLog.Debug(LogCategory.Network)?.
                                T($"client has been disconnected");
                    }
                }
                catch (Exception e)
                {
                    if (IsDebugLogEnabled)
                        DebugLog.Error(LogCategory.Network)?.
                            T($"send message failed due to error: '{e.Message} ({e.GetType().FullName})'");
                }
            }
        }
    }
}
