using NAccLogger.Impl;
using NAccLogger.Itf;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

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
        protected Dictionary<int, MultiLogInvoker>
            MultiLogInvokers =
                new Dictionary<int, MultiLogInvoker>();

        /// <summary>
        /// indicates if the dispatcher filter is enabled or not (thus only logs filters are enabled)
        /// </summary>
        public bool IsDispatcherFilterEnabled { get; protected set; }

        /// <summary>
        /// build a new dispatcher logger
        /// </summary>
        /// <param name="logParameters"></param>
        public Dispatcher(LogParameters logParameters = null)
            : base(logParameters) { }

        /// <summary>
        /// build a new repeater logger initialized with logger list
        /// </summary>
        /// <param name="loggers">loggers to dispatch to</param>
        /// <param name="isDispatcherFilterEnabled">if true, the dispatcher check its own filtering rules, else only loggers filters are checked for filtered log actions</param>
        /// <param name="logParameters"></param>
        public Dispatcher(
            List<ILog> loggers,
            bool isDispatcherFilterEnabled = true,
            LogParameters logParameters = null)
            : base(logParameters)
        {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggers));
            this.IsDispatcherFilterEnabled = isDispatcherFilterEnabled;
            foreach (var o in loggers)
                Loggers.AddLast(o);
        }

        /// <summary>
        /// check if a filter is enabled
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <param name="callerLineNumber">line number where log has been called</param>
        /// <param name="callerFilePath">file path where source code called the log</param>
        /// <returns>an invoker object to the log, else null</returns>
        protected ILogInvoker CheckFilter(
            object caller, 
            string callerTypeName, 
            string callerMemberName, 
            LogType logType, 
            LogCategory logCategory, 
            int callerLineNumber, 
            string callerFilePath
            )
        {
            if (IsDispatcherFilterEnabled)
            {
                // check the Dispatcher logger filter
                var invoker = LogParameters.LogFilter.CheckFilter(
                    this,
                    caller,
                    callerTypeName,
                    callerMemberName,
                    LogType.Error,
                    logCategory,
                    callerLineNumber,
                    callerFilePath
                    );

                if (invoker == null)
                    return null;
            }

            // invoker !=null => multi invoker enabled

            var id = Thread.CurrentThread.ManagedThreadId;
            if (!MultiLogInvokers
                .TryGetValue(id, out var minvoker))           
                MultiLogInvokers
                    .Add(
                        id,
                        minvoker =
                            new MultiLogInvoker());         
    
            // check individual loggers filters
            minvoker.Loggers.Clear();
            foreach (var o in Loggers)
            {
                var linvoker =
                    o
                        .LogParameters
                        .LogFilter
                        .CheckFilter(
                            o,
                            caller,
                            callerTypeName,
                            callerMemberName,
                            LogType.Error,
                            logCategory,
                            callerLineNumber,
                            callerFilePath
                        );
                if (linvoker != null)
                {
                    minvoker
                        .Loggers
                        .AddLast(o);
                    if (!o.IsForwardEnabled)
                        break;
                }
            }

            if (minvoker.Loggers.Count == 0)
                // no active logger
                return null;

            // setup up the invoker for the current log action
            minvoker.Log = this;
            minvoker.Caller = caller;
            minvoker.LogType = logType;
            minvoker.LogCategory = logCategory;
            minvoker.CallerFilePath = callerFilePath;
            minvoker.CallerLineNumber = callerLineNumber;
            minvoker.CallerMemberName = callerMemberName;

            return minvoker;
        }

        #region filtered log actions

        public override ILogInvoker Add(object caller, LogType logType = LogType.NotDefined, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return CheckFilter(caller, caller?.GetType().FullName, callerMemberName, logType, logCategory, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Debug(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return CheckFilter(null, null, callerMemberName, LogType.Debug, logCategory, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Error(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return CheckFilter(null, null, callerMemberName, LogType.Error, logCategory, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Fatal(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return CheckFilter(null, null, callerMemberName, LogType.Fatal, logCategory, callerLineNumber, callerFilePath);
        }

        public override ILogInvoker Info(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            return CheckFilter(null, null, callerMemberName, LogType.Info, logCategory, callerLineNumber, callerFilePath);
        }

        #endregion

        /// <summary>
        /// add a new log entry to the dispatcher logger
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            foreach (var o in Loggers)
            {
                o.Log(logItem);
                if (!o.IsForwardEnabled)
                    break;
            }
        }
    }
}
