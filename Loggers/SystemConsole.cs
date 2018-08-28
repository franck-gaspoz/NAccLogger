using NAccLogger.Impl;
using NAccLogger.Itf;

namespace NAccLogger.Loggers
{
    public class SystemConsole
        : LogBase
    {
        /// <summary>
        /// build a new system console logger
        /// </summary>
        /// <param name="loggerParameters"></param>
        public SystemConsole(
            LogParameters loggerParameters = null
        ) : base(loggerParameters) { }

        /// <summary>
        /// add a log entry to the system console
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            System.Console.WriteLine(logItem.LogEntryText);
        }
    }
}
