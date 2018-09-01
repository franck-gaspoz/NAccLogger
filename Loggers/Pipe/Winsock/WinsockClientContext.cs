using System.Net;
using System.Net.Sockets;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// winsock client context
    /// </summary>
    public class WinsockClientContext
    {
        /// <summary>
        /// server address (default 127.0.0.1)
        /// </summary>
        public IPAddress IPAddress
            = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// port the server is listening to (default 11000)
        /// </summary>
        public int PortNumber = 11000;

        /// <summary>
        /// protocole type (default Tcp)
        /// </summary>
        public ProtocolType ProtocolType = ProtocolType.Tcp;

        /// <summary>
        /// send timeout
        /// </summary>
        public int ReceiveTimeout = 3000;

        /// <summary>
        /// receive timeout
        /// </summary>
        public int SendTimeout = 3000;

        /// <summary>
        /// connect timeout
        /// </summary>
        public int ConnectTimeout = 10000;

        /// <summary>
        /// build a new socket server using default settings
        /// </summary>
        public WinsockClientContext() { }

        /// <summary>
        /// build a new socket server
        /// </summary>
        /// <param name="ipAddress">ip address</param>
        /// <param name="portNumber">port number the server is listening to</param>
        /// <param name="protocolType">protocole type</param>
        /// <param name="receiveTimeout">receive timeout</param>
        /// <param name="sendTimeout">send timeout</param>
        /// <param name="connectTimeout">connect timeout</param>
        public WinsockClientContext(
            IPAddress ipAddress,
            int portNumber,
            ProtocolType protocolType,
            int receiveTimeout,
            int sendTimeout,
            int connectTimeout
            )
        {
            this.IPAddress = ipAddress;
            this.PortNumber = portNumber;
            this.ProtocolType = protocolType;
            this.ReceiveTimeout = receiveTimeout;
            this.SendTimeout = sendTimeout;
            this.ConnectTimeout = connectTimeout;
        }
    }
}
