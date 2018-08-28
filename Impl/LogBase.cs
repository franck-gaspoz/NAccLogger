using NAccLogger.Itf;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAccLogger.Impl
{
    public abstract class LogBase 
        : ILog
    {
        #region attributes

        /// <summary>
        /// replace the default add to LogItems action if defined
        /// </summary>
        public Action<BindingList<ILogItem>, ILogItem> AddAction { get; set; } = null;

        /// <summary>
        /// enable/disable log items recording in log history (LogItems)
        /// </summary>
        public bool IsRecordingEnabled { get; set; } = true;

        /// <summary>
        /// log items history
        /// </summary>
        public BindingList<ILogItem> LogItems { get; } =
            new BindingList<ILogItem>();

        /// <summary>
        /// log filters
        /// </summary>
        public ILogFilter LogFilter { get; protected set; }

        /// <summary>
        /// log item text formatter
        /// </summary>
        public ILogItemTextFormatter LogItemTextFormatter { get; protected set; }

        #endregion

        /// <summary>
        /// build a new logger base
        /// </summary>
        /// <param name="loggerParameters">common logger parameters. ignored if null</param>
        public LogBase(
            LogParameters loggerParameters
            )
        {
            LogFilter = (loggerParameters != null && loggerParameters.LogFilter != null) ?
                loggerParameters.LogFilter : NAccLogger.Log.LoggerParameters.LogFilter;
            LogItemTextFormatter = (loggerParameters != null && loggerParameters.LogItemTextFormatter!=null) ?
                loggerParameters.LogItemTextFormatter : NAccLogger.Log.LoggerParameters.LogItemTextFormatter;
        }

        #region log add entry operations with filtering

        public virtual ILogInvoker Info(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogFilter.CheckFilter(
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
            return LogFilter.CheckFilter(
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
            return LogFilter.CheckFilter(
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
            return LogFilter.CheckFilter(
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
            return LogFilter.CheckFilter(
                this,
                caller,
                (caller == null) ? null : caller.GetType().FullName,
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
            return LogFilter.CheckFilter(
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
            var it = new LogItem(
                    LogItemTextFormatter.GetHeader()
                )
            {
                IsTextOnly = true
            };
            it.LogEntryText = LogItemTextFormatter.LogItemToString(it);

            // TODO: add this in logitemtextformatter (?)
            var ithsep = new LogItem("".PadLeft(it.LogEntryText.Length, '-'))
            {
                IsTextOnly = true
            };

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

        public virtual ILogItem Add(
            string text,
            object caller,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            // TODO: 
            // 1. build log item text depending on schema
            // 2. do it in a derived class

            var callerTypeName = (caller == null) ? null : caller.GetType().FullName;

            var it =
                    new LogItem(
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
        /// add to log implementation
        /// </summary>
        /// <param name="logItem"></param>
        public abstract void Log(ILogItem logItem);

        #endregion        

        /// <summary>
        /// internal add a log item to log
        /// </summary>
        /// <param name="it"></param>
        protected virtual void AddInternal(ILogItem it)
        {
            // build log item text
            it.LogEntryText = LogItemTextFormatter.LogItemToString(it);

            if (IsRecordingEnabled)
            {
                if (AddAction != null)
                    // custom record log item
                    AddAction.Invoke(LogItems, it);
                else
                    // record log item
                    LogItems.Add(it);
            }

            // add to log impl
            Log(it);
        }
    }
}
