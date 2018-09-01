using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NAccLogger.Ext;
using NAccLogger.Itf;
using NAccLogger.Loggers.Pipe.Winsock.Com;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// base client for winsock pipe logger server
    /// </summary>
    public class WinsockLogClient
    {
        #region communication attributes

        /// <summary>
        /// listening task
        /// </summary>
        public Task ListeningTask = null;

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

        #region ManualResetEvent instances signal completion

        protected ManualResetEvent ConnectDone =
            new ManualResetEvent(false);

        private ManualResetEvent ReceiveDone =
            new ManualResetEvent(false);

        #endregion

        protected WinsockClientContext SocketClientContext;
        protected IPEndPoint IPEndPoint;
        protected Socket ClientSocket;
        protected readonly MessageSerializer MessageSerializer
            = new MessageSerializer();

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

        ClientHandler ClientHandler = null;

        #endregion

        #region events

        public event EventHandler<ILogItem> LogItemReceived;

        public event EventHandler<GetHeader> GetHeaderReceived;

        public event EventHandler<GetHeaderText> GetHeaderTextReceived;

        #endregion

        #region build

        /// <summary>
        /// build a new client of a pipe winsock logger server
        /// </summary>
        /// <param name="socketClientContext">socket client context</param>
        /// <param name="debugLog">private debug logger for debug purposes</param>
        /// <param name="isDebugLogEnabled">enabled/disable server private logger</param>
        public WinsockLogClient(
            WinsockClientContext socketClientContext = null,
            bool autoStart = false,
            ILog debugLog = null,
            bool isDebugLogEnabled = true            
            )
        {
            this.DebugLog = debugLog;
            this.IsDebugLogEnabled = isDebugLogEnabled;
            this.SocketClientContext = socketClientContext ?? new WinsockClientContext();
            if (autoStart)
                Start();
        }

        #endregion

        #region connection

        /// <summary>
        /// start the client for connecting 
        /// </summary>
        public void Start()
        {
            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                IPEndPoint = new IPEndPoint(
                    SocketClientContext.IPAddress
                    ,SocketClientContext.PortNumber);

                // Create a TCP/IP socket.  
                Socket client = new Socket(
                    SocketClientContext.IPAddress.AddressFamily,
                    SocketType.Stream,
                    SocketClientContext.ProtocolType)
                {
                    SendTimeout = SocketClientContext.SendTimeout,
                    ReceiveTimeout = SocketClientContext.ReceiveTimeout
                };

                if (IsDebugLogEnabled)
                    DebugLog.Debug(LogCategory.Network)?.
                        T($"connecting pipe winsock logger client to addr={SocketClientContext.IPAddress} port={SocketClientContext.PortNumber} protocol type={SocketClientContext.ProtocolType} ...");

                // Connect to the remote endpoint.  
                var cnxAsync = client.BeginConnect(
                    IPEndPoint,
                    new AsyncCallback(ConnectCallback), 
                    client);

                cnxAsync.AsyncWaitHandle.WaitOne(SocketClientContext.ConnectTimeout, false);

                ConnectDone.WaitOne();

                // get the current log header structure
                Send(new GetHeader());

                // get the current log header text
                Send(new GetHeaderText());

                ListeningTask = Task.Run(() => { Listening(); });
                
                // Release the socket.  
                //client.Shutdown(SocketShutdown.Both);
                //client.Close();

            }
            catch (Exception e)
            {
                this.Error(LogCategory.Network)?.
                    T($"pipe winsock logger client crashed due to error: {e.Message}");
                throw e;
            }
        }

        protected void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                ClientSocket = (Socket)ar.AsyncState;

                // Complete the connection.  
                ClientSocket.EndConnect(ar);

                if (IsDebugLogEnabled)
                    DebugLog.Debug(LogCategory.Network)?.
                        T($"pipe winsock logger client connected to {ClientSocket.RemoteEndPoint}");

                ClientHandler = new ClientHandler(
                    new StateObject()
                    {
                        WorkSocket = ClientSocket
                    }
                    );

                // Signal that the connection has been made.  
                ConnectDone.Set();                
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"pipe winsock logger client crashed due to error: {e.Message}");
                throw e;
            }
        }

        #endregion

        #region receive message

        /// <summary>
        /// listening to server
        /// </summary>
        protected void Listening()
        {
            if (IsDebugLogEnabled)
                DebugLog.Debug(LogCategory.Network)?.
                    T($"pipe winsock logger client is listening...");

            var state = new StateObject
            {
                WorkSocket = ClientSocket
            };

            try
            {
                while (IsRunning)
                {
                    ReceiveDone.Reset();

                    ClientSocket.BeginReceive(
                        state.Buffer, 
                        0, 
                        StateObject.BufferSize, 
                        0,
                        new AsyncCallback(ReceiveCallback), 
                        state);

                    ReceiveDone.WaitOne();
                }
            } catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"pipe winsock logger client crashed due to error: '{e.Message}'");
                throw;
            }
        }

        protected void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                var client = state.WorkSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)                
                    state.AppendBytes(
                        bytesRead
                        );                        

                // All the data has arrived; put it in response.  
                var r = state.GetMessage();

                // handle the message
                HandleMessage(r);

                // Signal that all bytes have been received.  
                ReceiveDone.Set();
            }
            catch (Exception e)
            {
                if (IsDebugLogEnabled)
                    DebugLog.Error(LogCategory.Network)?.
                        T($"receive message failed due to error: {e.Message}");
            }
        }

        /// <summary>
        /// handle a new incoming message
        /// </summary>
        /// <param name="message">bytes of the message</param>
        protected void HandleMessage(byte[] message)
        {
            try
            {
                var logItems = MessageSerializer.Unserialize<ILogItem>(message);

                foreach (var logItem in logItems)
                {
                    if (IsDebugLogEnabled)
                        DebugLog.Debug(LogCategory.Network)?.
                            T($"log item received: {logItem.Text}");

                    LogItemReceived?.Invoke(this, logItem);
                }

            } catch (InvalidCastException)
            {
                var coms = MessageSerializer.Unserialize<ICommandMessage>(message);

                foreach (var com in coms)
                {
                    if (IsDebugLogEnabled)
                        DebugLog.Debug(LogCategory.Network)?.
                            T($"command reply received: {com}");

                    HandleCommand(com);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region command

        protected void HandleCommand(ICommandMessage com)
        {
            switch (com.Command)
            {
                case Command.GetHeader:
                    GetHeaderReceived?.Invoke(this, (GetHeader)com);
                    break;
                case Command.GetHeaderText:
                    GetHeaderTextReceived?.Invoke(this, (GetHeaderText)com);
                    break;
                default:
                    if (IsDebugLogEnabled)
                        DebugLog.Error(LogCategory.Network)?.
                            T($"unknown command: {com}");
                    break;
            }
        }

        #endregion

        #region send message

        /// <summary>
        /// send a command to the server
        /// </summary>
        /// <param name="command">command message</param>
        protected void Send(ICommandMessage command)
        {
            ClientHandler.SendDone.Reset();

            // get item as bytes
            var bytes = ClientHandler.MessageSerializer.Serialize<ICommandMessage>(command);
            // transmit to server socket
            ClientHandler
                .StateObject
                .WorkSocket
                .BeginSend(bytes, 0, bytes.Length, 0,
                new AsyncCallback(
                    SendCallback),
                    ClientHandler
                        /*.StateObject
                        .WorkSocket*/);

            ClientHandler.SendDone.WaitOne();
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
                        T($"Sent {bytesSent} bytes to server.");
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

        #endregion
    }
}
