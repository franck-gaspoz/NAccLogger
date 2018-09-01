using System.Net;
using System.Net.Sockets;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// winsock server context
    /// </summary>
    public class WinsockServerContext
        : WinsockClientContext
    {
        /// <summary>
        /// max. number of accepted in queue connections
        /// </summary>
        public int BackLog = 100;

        /// <summary>
        /// build a new socket server using default settings
        /// </summary>
        public WinsockServerContext() { }

        /// <summary>
        /// build a new socket server
        /// </summary>
        /// <param name="ipAddress">ip address</param>
        /// <param name="portNumber">port number the server is listening to</param>
        /// <param name="protocolType">protocole type</param>
        /// <param name="receiveTimeout">receive timeout</param>
        /// <param name="sendTimeout">send timeout</param>
        /// <param name="connectTimeout">connect timeout</param>
        /// <param name="backLog">connection queue size</param>
        public WinsockServerContext(
            IPAddress ipAddress,
            int portNumber, 
            ProtocolType protocolType, 
            int sendTimeout,
            int receiveTimeout,
            int connectTimeout,
            int backLog)
            : base(
                  ipAddress, 
                  portNumber,
                  protocolType,
                  receiveTimeout,
                  sendTimeout,
                  connectTimeout)
        {
            this.BackLog = backLog;
        }
    }
}
