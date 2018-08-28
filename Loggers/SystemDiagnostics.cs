using NAccLogger.Impl;
using NAccLogger.Itf;

namespace NAccLogger.Loggers
{
    /// <summary>
    /// system diagnostics
    /// </summary>
    public class SystemDiagnostics
        : LogBase
    {
        /// <summary>
        /// build a new system diagnostics logger
        /// </summary>
        /// <param name="loggerParameters"></param>
        public SystemDiagnostics(
            LogParameters loggerParameters = null
        ) : base(loggerParameters) { }

        /// <summary>
        /// add a new entty to the system diagnostics
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            System.Diagnostics.Debug.WriteLine(logItem.LogEntryText);
        }
    }
}
