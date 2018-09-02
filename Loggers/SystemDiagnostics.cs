using NAccLogger.Impl;
using NAccLogger.Itf;

namespace NAccLogger.Loggers
{
    /// <summary>
    /// system diagnostics log
    /// </summary>
    public class SystemDiagnostics
        : LogBase
    {
        /// <summary>
        /// build a new system diagnostics logger
        /// </summary>
        /// <param name="logParameters"></param>
        public SystemDiagnostics(
            LogParameters logParameters = null
        ) : base(logParameters) { }

        /// <summary>
        /// add a new entty to the system diagnostics
        /// </summary>
        /// <param name="logItem">log item to be added</param>
        public override void Log(ILogItem logItem)
        {
            System.Diagnostics.Debug.WriteLine(logItem.LogEntryText);
        }
    }
}
