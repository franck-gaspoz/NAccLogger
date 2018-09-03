using NAccLogger.Itf;

namespace NAccLogger
{
    /// <summary>
    /// common loggers parameters
    /// </summary>
    public class LogParameters
    {
        /// <summary>
        /// log components factory
        /// </summary>
        public ILogFactory LogFactory;

        /// <summary>
        /// log filter. ignored if null
        /// </summary>
        public ILogFilter LogFilter;

        /// <summary>
        /// log item text formatter. ignored if null
        /// </summary>
        public ILogItemTextFormatter LogItemTextFormatter;

        /// <summary>
        /// log item buffer
        /// </summary>
        public ILogItemBuffer LogItemBuffer;

        /// <summary>
        /// enable/disable log items recording in log history (LogItems)
        /// </summary>
        public bool IsRecordingEnabled { get; set; } = false;

        /// <summary>
        /// build new parameters with default values
        /// </summary>
        public LogParameters()
        {
            LogFactory = Log.LogFactory;
            LogFilter = LogFactory.CreateLogFilter();
            LogItemTextFormatter = LogFactory.CreateLogItemTextFormatter();
            LogItemBuffer = LogFactory.CreateLogItemBuffer();
        }

        /// <summary>
        /// get new log parameters by cloning this
        /// </summary>
        /// <returns>log parameters</returns>
        public LogParameters Clone()
        {
            var r = new LogParameters()
            {
                LogFactory = LogFactory,
                LogFilter = LogFilter.Clone(),
                LogItemTextFormatter = LogItemTextFormatter.Clone(),
                LogItemBuffer = LogItemBuffer.Clone(),
                IsRecordingEnabled = IsRecordingEnabled
            };
            return r;
        }
    }
}
