using NAccLogger.Itf;
using NAccLogger.Loggers.Pipe.Winsock.Com;

namespace NAccLogger.Loggers.Pipe.Winsock.Com
{
    public class CommandHandler
    {
        public void HandleCommand(
            ILog log,
            ICommandMessage com
            )
        {
            switch (com.Command)
            {
                case Command.GetHeader:
                    com.SetReply(
                        log
                            .LogParameters
                            .LogItemTextFormatter
                            .GetColumns());
                    break;
                case Command.GetHeaderText:
                    com.SetReply(
                        log
                            .LogParameters
                            .LogItemTextFormatter
                            .GetHeader());
                    break;
                default:
                    com.SetReply( $"unknown command '{com.Command}'" );
                    break;
            }
        }
    }
}
