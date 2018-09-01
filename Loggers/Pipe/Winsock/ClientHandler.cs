using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// pipe winsock logger client handler
    /// </summary>
    public class ClientHandler
    {
        public readonly StateObject StateObject;
        public readonly MessageSerializer MessageSerializer 
            = new MessageSerializer();

        public Task ClientListeningTask;
        public ManualResetEvent ReceiveDone = new ManualResetEvent(false);
        public ManualResetEvent SendDone = new ManualResetEvent(false);

        public ClientHandler(StateObject stateObject)
        {
            StateObject = stateObject;
        }

        public override string ToString()
        {
            var o = StateObject.WorkSocket;
            return $"address family={o.AddressFamily} handle={o.Handle} local end point={o.LocalEndPoint} remote end point={o.RemoteEndPoint}";
        }

        public void Dispose()
        {
            StateObject?.WorkSocket?.Shutdown(
                SocketShutdown.Both
                );
            StateObject?.WorkSocket?.Close();
        }
    }
}
