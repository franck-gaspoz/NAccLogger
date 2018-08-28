using NAccLogger.Impl;
using NAccLogger.Itf;

namespace NAccLogger
{
    /// <summary>
    /// common loggers parameters
    /// </summary>
    public class LogParameters
    {
        /// <summary>
        /// log filter. ignored if null
        /// </summary>
        public ILogFilter LogFilter;

        /// <summary>
        /// log item text formatter. ignored if null
        /// </summary>
        public ILogItemTextFormatter LogItemTextFormatter;

        /// <summary>
        /// build new parameters with default values
        /// </summary>
        public LogParameters()
        {
            LogFilter = new LogFilter();
            LogItemTextFormatter = new LogItemTextFormatter();
        }
    }
}
