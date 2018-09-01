using System;

namespace NAccLogger.Loggers.Pipe.Winsock.Com
{
    /// <summary>
    /// command item sendable to a WinsockServer
    /// </summary>
    [Serializable]
    public abstract class CommandMessage<TP,TR> 
        : ICommandMessage
    {
        /// <summary>
        /// command
        /// </summary>
        public Command Command { get; set; }

        /// <summary>
        /// command parameters
        /// </summary>
        public TP Parameters { get; set; }

        /// <summary>
        /// reply to the command
        /// </summary>
        public TR Reply { get; set; }
        
        /// <summary>
        /// build a command message for the named Command
        /// </summary>
        /// <param name="command"></param>
        public CommandMessage(Command command)
        {
            Command = command;
        }

        public void SetReply(object reply)
        {
            Reply = (TR)reply;
        }

        public override string ToString()
        {
            return $"com={Command} Reply={Reply}";
        }
    }
}
