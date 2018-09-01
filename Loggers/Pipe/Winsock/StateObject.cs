using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    /// <summary>
    /// state object for reading data asynchronously
    /// </summary>
    public class StateObject
    {
        /// <summary>
        /// Client socket
        /// </summary>
        public Socket WorkSocket = null;

        /// <summary>
        /// Size of receive buffer
        /// </summary>
        public const int BufferSize = 1024 * 10;

        /// <summary>
        /// Receive buffer
        /// </summary>
        public byte[] Buffer = new byte[BufferSize];

        /// <summary>
        /// message bytes
        /// </summary>
        protected List<byte> Message = new List<byte>();

        /// <summary>
        /// append incoming bytes from bufferer to the current message
        /// </summary>
        /// <param name="bytesCount">number of bytes to ber transfered to message</param>
        public void AppendBytes(int bytesCount)
        {
            Message
                .AddRange(
                    Buffer.Take(bytesCount)
                );
        }

        /// <summary>
        /// clear message. Get ready to receive a new one
        /// </summary>
        public void ClearMessage()
        {
            Message.Clear();
        }

        /// <summary>
        /// returns the current message and clear it
        /// </summary>
        /// <returns>bytes of the message</returns>
        public byte[] GetMessage()
        {
            var r = Message.ToArray();
            ClearMessage();
            return r;
        }
    }
}

