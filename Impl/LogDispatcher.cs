﻿using NAccLogger.Itf;
using NAccLogger.Loggers.Pipe;
using System.Collections.Generic;

namespace NAccLogger.Impl
{
    /// <summary>
    /// dispatch a log call to a specific impl. according to dispatching rules
    /// </summary>
    public class LogDispatcher
        : ILogDispatcher
    {
        #region attributes

        /// <summary>
        /// filters values
        /// </summary>
        protected FilterValues
            <Dispatcher> FilterValues 
            = new FilterValues
            <Dispatcher>();

        #endregion

        /// <summary>
        /// construct a filter accepting anything
        /// </summary>
        public LogDispatcher()
        {
            
        }

        /// <summary>
        /// clear dispatching rules
        /// </summary>
        /// <returns>the log dispatcher</returns>
        public ILogDispatcher Clear()
        {
            FilterValues.Clear();
            return this;
        }

        /// <summary>
        /// get a dispatcher according to dispatching properties and rules. return a dispatcher pipe if founded, null else
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <returns>a logger or null</returns>
        public Dispatcher GetDispatcher(
            object caller, 
            string callerTypeName, 
            string callerMemberName, 
            LogType logType, 
            LogCategory logCategory
            )
        {            
            return
                FilterValues.GetValue(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    logType,
                    logCategory
                    );            
        }

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
        public ILogDispatcher SetDispatchingRule(
            ILog logger,
            bool forwardEnabled = false,
            object caller = null,
            string callerTypeName = null,
            string callerMemberName = null,
            LogType? logType = null,
            LogCategory? logCategory = null
            )
        {
            var dispatcher = FilterValues
                .GetValue(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    logType ?? LogType.NotDefined,
                    logCategory ?? LogCategory.NotDefined
                );
            if (dispatcher == null)
                dispatcher = new Dispatcher();
            dispatcher
                .Loggers
                .AddLast(logger);

            FilterValues.AddOrSetValue(
                dispatcher,
                caller,
                callerTypeName,
                callerMemberName,
                logType,
                logCategory);
            return this;
        }
    }
}
