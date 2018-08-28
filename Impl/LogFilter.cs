using NAccLogger.Itf;
using System.Collections.Generic;
using System.Threading;

namespace NAccLogger.Impl
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
    public class LogFilter : ILogFilter
    {
        #region attributes

        /// <summary>
        /// per thread enabled log invokers
        /// </summary>
        protected Dictionary<int, ILogInvoker>
            LogInvokers =
            new Dictionary<int, ILogInvoker>();

        /// <summary>
        /// filters values
        /// </summary>
        protected FilterValues FilterValues = new FilterValues();

        #endregion

        /// <summary>
        /// construct a filter accepting anything
        /// </summary>
        public LogFilter()
        {
            SetIsEnabledFilter(true);
        }

        /// <summary>
        /// clear filter values
        /// <para>all further log items will not be selected (no filters) until new filters are added</para>
        /// </summary>
        public void Clear()
        {
            FilterValues.Clear();
            LogInvokers.Clear();
        }

        /// <summary>
        /// check if a filter is enabled
        /// </summary>
        /// <param name="logger">target logger</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        /// <returns>an invoker object to the log, else null</returns>
        public ILogInvoker CheckFilter(
            ILog logger,
            object caller,
            string callerTypeName,
            string callerMemberName,
            LogType logType,
            LogCategory logCategory,
            int callerLineNumber,
            string callerFilePath
            )
        {
            return
                (FilterValues.GetValue(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    logType,
                    logCategory
                    )) ?
                     GetLogInvoker(
                        logger,
                        caller,
                        callerMemberName,
                        logType,
                        logCategory,
                        callerLineNumber,
                        callerFilePath
                        )
                     :
                     null;
        }

        /// <summary>
        /// enable or disable a log item filer for the specified properties. null values act as wildcards making accepting any value
        /// </summary>
        /// <param name="isEnabled">enabled (true), disabled (false)</param>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        public void SetIsEnabledFilter(
            bool isEnabled,
            object caller = null,
            string callerTypeName = null,
            string callerMemberName = null,
            LogType? logType = null,
            LogCategory? logCategory = null
            )
        {
            FilterValues.AddOrSetValue(
                isEnabled,
                caller,
                callerTypeName,
                callerMemberName,
                logType,
                logCategory);
        }

        /// <summary>
        /// return a log invoker if any depending on log filters, null otherwise
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerMemberName"></param>
        /// <param name="logType"></param>
        /// <param name="logCategory"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        /// <returns>log invoker to logger, else null</returns>
        protected ILogInvoker GetLogInvoker(
            ILog logger,
            object caller,
            string callerMemberName,
            LogType logType,
            LogCategory logCategory,
            int callerLineNumber,
            string callerFilePath
            )
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            if (!LogInvokers.TryGetValue(id, 
                out ILogInvoker o))
                LogInvokers
                    .Add(
                        id,
                        o = new LogInvoker());
            o.Log = logger;
            o.Caller = caller;
            o.LogType = logType;
            o.LogCategory = logCategory;
            o.CallerFilePath = callerFilePath;
            o.CallerLineNumber = callerLineNumber;
            o.CallerMemberName = callerMemberName;
            return o;
        }
    }
}
