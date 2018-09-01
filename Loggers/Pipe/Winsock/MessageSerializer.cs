using NAccLogger.Itf;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NAccLogger.Loggers.Pipe.Winsock
{
    public class MessageSerializer
    {
        public int InternalBufferCapacity = 1024 * 10;

        protected BinaryFormatter BinaryFormatter;

        public MessageSerializer()
        {
            BinaryFormatter = new BinaryFormatter();
        }

        /// <summary>
        /// serialize an object to a byte array
        /// </summary>
        /// <param name="obj">log item to be serialized</param>
        /// <returnsserilized log item></returns>
        public byte[] Serialize<T>(T obj)
        {
            byte[] r = null;
            using (MemoryStream stream
                = new MemoryStream(InternalBufferCapacity))
            {
                BinaryFormatter.Serialize(stream, obj);
                stream.Position = 0;
                r = stream.ToArray();
            }
            return r;
        }

        /// <summary>
        /// get one or several objects from bytes array of a serialization stream
        /// </summary>
        /// <param name="objBytes">serilized log item</param>
        /// <returns>log item object</returns>
        public IList<T> Unserialize<T>(byte[] objBytes)
        {            
            using (MemoryStream stream = new MemoryStream(objBytes))
            {
                var lst = new List<T>();
                while (stream.Position != stream.Length) { 
                    T r = (T)BinaryFormatter.Deserialize(stream);
                    lst.Add(r);
                }
                return lst;
            }
        }
    }
}
