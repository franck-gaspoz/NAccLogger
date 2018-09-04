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
        /// build a new parameters with default values and a specific factory
        /// </summary>
        /// <param name="logFactory">log components factory</param>
        public LogParameters(
            ILogFactory logFactory
            )
        {
            LogFactory = logFactory;
            LogFilter = LogFactory.CreateLogFilter();
            LogItemTextFormatter = LogFactory.CreateLogItemTextFormatter();
            LogItemBuffer = LogFactory.CreateLogItemBuffer();
        }

        /// <summary>
        /// build new parameters with specified filters and default values
        /// </summary>
        /// <param name="logFilter">log filter</param>
        public LogParameters(ILogFilter logFilter)
        {
            LogFilter = logFilter;
            LogFactory = Log.LogFactory;
            LogItemTextFormatter = LogFactory.CreateLogItemTextFormatter();
            LogItemBuffer = LogFactory.CreateLogItemBuffer();
        }

        /// <summary>
        /// build new parameters with default values and specified log components factory and filters
        /// </summary>
        /// <param name="logFactory">log component factory</param>
        /// <param name="logFilter">log filters</param>
        public LogParameters(
            ILogFactory logFactory, 
            ILogFilter logFilter) : this(logFactory)
        {
            LogFilter = logFilter;
        }
        
        /// <summary>
        /// build new parameters
        /// </summary>
        /// <param name="logFactory">log components factory</param>
        /// <param name="logFilter">log filters</param>
        /// <param name="logItemTextFormatter">log item text formatter</param>
        /// <param name="logItemBuffer">log item buffer</param>
        /// <param name="isRecordingEnabled">recording enabled or not (default false)</param>
        public LogParameters(
            ILogFactory logFactory, 
            ILogFilter logFilter, 
            ILogItemTextFormatter logItemTextFormatter, 
            ILogItemBuffer logItemBuffer, 
            bool isRecordingEnabled = false) : this(logFactory)
        {
            LogFilter = logFilter;
            LogItemTextFormatter = logItemTextFormatter;
            LogItemBuffer = logItemBuffer;
            IsRecordingEnabled = isRecordingEnabled;
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
