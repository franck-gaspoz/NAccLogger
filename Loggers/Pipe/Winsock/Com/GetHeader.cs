using System;
using System.Collections.Generic;

namespace NAccLogger.Loggers.Pipe.Winsock.Com
{
    [Serializable]
    public class GetHeader
        : CommandMessage<object,IEnumerable<string>>
    {
        public GetHeader()
            : base(Command.GetHeader) { }
    }
}
