using NAccLogger.Loggers.Pipe;

namespace NAccLogger.Itf
{
    /// <summary>
    /// dispatch a log call to a specific impl. according to dispatching rules
    /// </summary>
    public interface ILogDispatcher
    {
        /// <summary>
        /// set a new or change an existing dispatching rule
        /// </summary>
        /// <param name="logger">target logger</param>
        /// <param name="forwardEnabled">if true, the log action handled by the dispatcher is also forwarded to other dispatchers and to the logs pipeline. If not the dispatcher is the one handling the log action</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param> 
        /// <returns>the log dispatcher</returns>
        ILogDispatcher SetDispatchingRule(
            ILog logger,
            bool forwardEnabled = false,
            object caller = null,
            string callerTypeName = null,
            string callerMemberName = null,
            LogType? logType = null,
            LogCategory? logCategory = null
            );

        /// <summary>
        /// get a dispatcher according to dispatching properties and rules. return a dispatcher pipe if founded, null else
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <returns>an invoker object to the log, else null</returns>
        Dispatcher GetDispatcher(
            object caller, 
            string callerTypeName, 
            string callerMemberName, 
            LogType logType, 
            LogCategory logCategory
            );

        /// <summary>
        /// clear dispatching rules
        /// </summary>
        /// <returns>the log dispatcher</returns>
        ILogDispatcher Clear();
    }
}
