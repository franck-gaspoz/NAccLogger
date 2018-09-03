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
    public class LogFilter 
        : LogFilterBase, 
        ILogFilter
    {
        #region attributes

        /// <summary>
        /// filters values
        /// </summary>
        protected FilterValues<bool> FilterValues 
            = new FilterValues<bool>();

        #endregion

        /// <summary>
        /// construct a filter accepting anything
        /// </summary>
        public LogFilter()
        {
            SetIsEnabledFilter(true);
        }

        /// <summary>
        /// get a new log filter by cloning (deep)
        /// </summary>
        /// <returns>log filter</returns>
        public ILogFilter Clone()
        {
            var r = new LogFilter()
                { FilterValues = FilterValues.Clone() };
            return r;
        }

        /// <summary>
        /// clear filter values
        /// <para>all further log items will not be selected (no filters) until new filters are added</para>
        /// </summary>
        /// <returns>the log filter</returns>
        public ILogFilter Clear()
        {
            FilterValues.Clear();
            LogInvokers.Clear();
            return this;
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
        /// <returns>the log filter</returns>
        public ILogFilter SetIsEnabledFilter(
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
            return this;
        }
        
    }
}
