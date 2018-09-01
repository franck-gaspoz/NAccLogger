namespace NAccLogger.Itf
{
    /// <summary>
    /// interface of log factory wich contains log components factory methods
    /// </summary>
    public interface ILogFactory
    {
        /// <summary>
        /// create a log item
        /// </summary>
        /// <param name="text">text of the log item</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="logType">type of the log entry</param>
        /// <param name="logCategory">category of the log entry</param>
        /// <param name="callerMemberName">name of the property or method wich made the call</param>
        /// <param name="callerLineNumber">line number in the source file where the call was done</param>
        /// <returns>log item impl.</returns>
        ILogItem CreateLogItem(
            string text,
            string callerTypeName = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            string callerMemberName = "",
            int callerLineNumber = -1,
            string callerFilePath = ""
            );

        /// <summary>
        /// create a log item text formatter
        /// </summary>
        /// <returns></returns>
        ILogItemTextFormatter CreateLogItemTextFormatter();

        /// <summary>
        /// create a log filter
        /// </summary>
        /// <returns>log filter impl.</returns>
        ILogFilter CreateLogFilter();
    }
}
