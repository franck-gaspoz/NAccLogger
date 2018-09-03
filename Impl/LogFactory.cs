using NAccLogger.Itf;

namespace NAccLogger.Impl
{
    /// <summary>
    /// log factory which contains log components factory methods
    /// </summary>
    public class LogFactory
        : ILogFactory
    {
        /// <summary>
        /// creates a new log factory
        /// </summary>
        public LogFactory() { }

        /// <summary>
        /// get a new log factory by cloning this
        /// </summary>
        /// <returns>cloned log factory</returns>
        public virtual ILogFactory Clone()
        {
            return new LogFactory();
        }

        /// <summary>
        /// create a log filter
        /// </summary>
        /// <returns>log filter impl.</returns>
        public virtual ILogFilter CreateLogFilter()
        {
            return new LogFilter();
        }

        /// <summary>
        /// create a new log item
        /// </summary>
        /// <param name="text">text of the log item</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="logType">type of the log entry</param>
        /// <param name="logCategory">category of the log entry</param>
        /// <param name="callerMemberName">name of the property or method wich made the call</param>
        /// <param name="callerLineNumber">line number in the source file where the call was done</param>
        /// <returns>log item impl.</returns>
        public virtual ILogItem CreateLogItem(
            string text,
            string callerTypeName = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            string callerMemberName = "",
            int callerLineNumber = -1,
            string callerFilePath = ""
            )
        {
            return new LogItem(
                text,
                callerTypeName,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// create a log item text formatter
        /// </summary>
        /// <returns>log item text formatter impl.</returns>
        public virtual ILogItemTextFormatter CreateLogItemTextFormatter()
        {
            return new LogItemTextFormatter();
        }

        /// <summary>
        /// create a log dispatcher
        /// </summary>
        /// <returns>a log dispatcher</returns>
        public virtual ILogDispatcher CreateLogDispatcher()
        {
            return new LogDispatcher();
        }

        /// <summary>
        /// create a log item buffer
        /// </summary>
        /// <returns>log item buffer</returns>
        public ILogItemBuffer CreateLogItemBuffer()
        {
            return new LogItemBuffer();
        }
    }
}
