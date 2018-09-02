using NAccLogger.Itf;
using System.Runtime.CompilerServices;

namespace NAccLogger.Impl
{
    public abstract class LogBase 
        : ILog
    {
        #region attributes

        /// <summary>
        /// log parameters
        /// </summary>
        public LogParameters LogParameters { get; protected set; }

        /// <summary>
        /// if true, any log action is forwarded to nexts loggers in a dispatcher or a pipeline where this log is contained, otherwize this logger is the last to handle the log action (default is true)
        /// </summary>
        public bool IsForwardEnabled { get; set; } = true;

        #endregion

        /// <summary>
        /// build a new logger base
        /// </summary>
        /// <param name="logParameters">logger parameters. use NAccLogger.Log log parameters if null</param>
        public LogBase(
            LogParameters logParameters
            )
        {
            LogParameters = logParameters ?? NAccLogger.Log.LogParameters;
        }

        #region log add entry operations with filtering

        public virtual ILogInvoker Info(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Info, 
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        public virtual ILogInvoker Warning(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Warning,
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        public virtual ILogInvoker Error(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Error,
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        public virtual ILogInvoker Fatal(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Fatal,
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="logType">type of the log entry from LogType</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public virtual ILogInvoker Add(
            object caller,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = "")
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                caller,
                caller?.GetType().FullName,
                callerMemberName,
                logType,
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        public virtual ILogInvoker Debug(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogParameters.LogFilter.CheckFilter(
                this,
                null,
                null,
                callerMemberName,
                LogType.Debug,
                logCategory,
                callerLineNumber,
                callerFilePath
                );
        }

        #endregion

        #region log add entry operations without filtering

        /// <summary>
        /// add a header entry to the log
        /// </summary>
        public virtual void Header()
        {
            var it = LogParameters.LogFactory.CreateLogItem(
                LogParameters.LogItemTextFormatter.GetHeader()
                );
            it.IsTextOnly = true;
            it.LogEntryText = LogParameters.LogItemTextFormatter.LogItemToString(it);

            // TODO: add this in logitemtextformatter (?)
            var ithsep = LogParameters.LogFactory.CreateLogItem("".PadLeft(it.LogEntryText.Length, '-'));
            ithsep.IsTextOnly = true;

            AddInternal(ithsep);
            AddInternal(it);
            AddInternal(ithsep);
        }

        public virtual void Info(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, null, LogType.Info, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public virtual void Error(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, null, LogType.Error, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public virtual void Fatal(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, null, LogType.Fatal, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public virtual void Warning(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, null, LogType.Warning, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public virtual void Debug(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, null, LogType.Debug, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }
        
        /// <summary>
        /// build a new log item and add it to this logger
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caller"></param>
        /// <param name="logType"></param>
        /// <param name="logCategory"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        /// <returns>the log item</returns>
        public virtual ILogItem Add(
            string text,
            object caller = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            var callerTypeName = caller?.GetType().FullName;
            var it =
                    LogParameters
                        .LogFactory
                            .CreateLogItem(
                                text,
                                callerTypeName,
                                logType,
                                logCategory,
                                callerMemberName,
                                callerLineNumber,
                                callerFilePath
                        );

            AddInternal(it);

            return it;
        }
        
        /// <summary>
        /// log an item (impl.)
        /// </summary>
        /// <param name="logItem">log item</param>
        public abstract void Log(ILogItem logItem);

        #endregion

        /// <summary>
        /// add a log item to the log
        /// </summary>
        /// <param name="logItem">log item</param>
        public void Add(ILogItem logItem)
        {
            AddInternal(logItem);
        }

        /// <summary>
        /// internal add a log item to log
        /// </summary>
        /// <param name="it"></param>
        protected virtual void AddInternal(ILogItem it)
        {
            // build log item text
            it.LogEntryText = LogParameters.LogItemTextFormatter.LogItemToString(it);

            if (LogParameters.IsRecordingEnabled)
                // record log item
                LogParameters
                    .LogItemBuffer
                    .Add(it);

            // add to log impl.
            Log(it);
        }
    }
}
