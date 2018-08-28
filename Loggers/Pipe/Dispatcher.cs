using NAccLogger.Impl;
using NAccLogger.Itf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NAccLogger.Loggers.Pipe
{
    /// <summary>
    /// pipe dispatcher : allow to subtitue the Log facade logger implementation by an ordered list of loggers
    /// <para>the dispatch is performed before log items are filtered, so items are filtered individually by each logger in the list</para>
    /// </summary>
    public class Dispatcher
        : LogBase
    {
        /// <summary>
        /// ordered list of loggers
        /// </summary>
        public LinkedList<ILog> Loggers =
            new LinkedList<ILog>();

        /// <summary>
        /// MultiLogInvoker cache
        /// </summary>
        protected Dictionary<ILogInvoker, MultiLogInvoker>
            MultiLogInvokers =
                new Dictionary<ILogInvoker, MultiLogInvoker>();

        /// <summary>
        /// build a new repeater logger
        /// </summary>
        /// <param name="logParameters"></param>
        public Dispatcher(LogParameters logParameters = null)
            : base(logParameters) { }

        /// <summary>
        /// build a new repeater logger initialized with logger list
        /// </summary>
        /// <param name="logParameters"></param>
        public Dispatcher(
            List<ILog> loggers,
            LogParameters logParameters = null)
            : base(logParameters)
        {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggers));
            foreach (var o in loggers)
                Loggers.AddLast(o);
        }

        #region filtered log methods

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
        protected ILogInvoker CheckFilter(
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
            // check the Dispatcher logger filter
            var invoker = LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Error,
                logCategory,
                callerLineNumber,
                callerFilePath
                );

            if (invoker == null)
                return null;

            if (!MultiLogInvokers
                .TryGetValue(invoker, out var minvoker)) {
                minvoker =
                    new MultiLogInvoker()
                    {
                        Log = this,
                        Caller = caller,
                        LogType = logType,
                        LogCategory = logCategory,
                        CallerFilePath = callerFilePath,
                        CallerLineNumber = callerLineNumber,
                        CallerMemberName = callerMemberName
                    };
                MultiLogInvokers.Add(invoker, minvoker);
            }

            // check individual loggers filters
            minvoker.Loggers.Clear();
            foreach (var o in Loggers)
            {
                var linvoker =
                    o
                        .LogFilter
                        .CheckFilter(
                            o,
                            null,
                            null,
                            callerMemberName,
                            LogType.Error,
                            logCategory,
                            callerLineNumber,
                            callerFilePath
                        );
                if (linvoker != null)
                    minvoker
                        .Loggers
                        .AddLast(o);
            }

            if (minvoker.Loggers.Count == 0)
                // no acitve logger
                return null;

            return minvoker;
        }

        public override ILogInvoker Add(object caller, LogType logType = LogType.NotDefined, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return base.Add(caller, logType, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Debug(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return base.Debug(logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Error(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return base.Error(logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Fatal(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return base.Fatal(logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Info(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return base.Info(logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        #endregion

        /// <summary>
        /// add a new log entry to the repeater logger
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            foreach (var o in Loggers)
                o.Log(logItem);
        }
    }
}
