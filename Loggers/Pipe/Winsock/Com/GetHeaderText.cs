using System;

namespace NAccLogger.Loggers.Pipe.Winsock.Com
{
    [Serializable]
    public class GetHeaderText 
        : CommandMessage<object,string>
    {
        public GetHeaderText()
            : base(Command.GetHeaderText) { }
    }
}
