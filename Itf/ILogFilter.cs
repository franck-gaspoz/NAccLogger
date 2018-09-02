namespace NAccLogger.Itf
{
    /// <summary>
    /// filter of log items, depending on:
    /// <para>- caller object</para>
    /// <para>- caller type name</para>
    /// <para>- caller member name</para>
    /// <para>- log type</para>
    /// <para>- log category</para>
    /// shared throught loggers and can be specialized by loggers
    /// test result is cached by thread throught a specialized ILogInvoker
    /// </summary>
    public interface ILogFilter
    {
        /// <summary>
        /// check if a filter is enabled
        /// </summary>
        /// <param name="logger">target logger</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <param name="callerLineNumber">line number where log has been called</param>
        /// <param name="callerFilePath">file path where source code called the log</param>
        /// <returns>an invoker object to the log, else null</returns>
        ILogInvoker CheckFilter(
            ILog logger, 
            object caller, 
            string callerTypeName, 
            string callerMemberName, 
            LogType logType, 
            LogCategory logCategory, 
            int callerLineNumber, 
            string callerFilePath);

        /// <summary>
        /// clear filter values
        /// <para>all further log items will not be selected (no filters) until new filters are added</para>
        /// </summary>
        /// <returns>the log filter</returns>
        ILogFilter Clear();

        /// <summary>
        /// enable or disable a log item filer for the specified properties. null values act as wildcards making accepting any value
        /// </summary>
        /// <param name="isEnabled">enabled (true), disabled (false)</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param> 
        /// <returns>the log filter</returns>
        ILogFilter SetIsEnabledFilter(
            bool isEnabled, 
            object caller = null, 
            string callerTypeName = null, 
            string callerMemberName = null, 
            LogType? logType = null, 
            LogCategory? logCategory = null);
    }
}