using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NAccLogger
{
    public abstract class LogBase 
        : ILog
    {
        /// <summary>
        /// replace the default add to LogItems action if defined
        /// </summary>
        public Action<BindingList<LogItem>, LogItem> AddAction { get; set; } = null;

        /// <summary>
        /// enable/disable log items recording in log history (LogItems)
        /// </summary>
        public bool IsRecordingEnabled { get; set; } = true;

        /// <summary>
        /// log items history
        /// </summary>
        public BindingList<LogItem> LogItems { get; } =
            new BindingList<LogItem>();

        #region log filters

        public Dictionary<LogType, LogType> EnabledLogTypes
            = new Dictionary<LogType, LogType>();

        public Dictionary<LogCategory, LogCategory> EnabledLogCategories =
            new Dictionary<LogCategory, LogCategory>();

        #endregion

        #region log formating

        public string LogItemToStringColumnsSeparator
        {
            get
            {
                return " | ";
            }
        }

        public Func<LogItem,string> LogItemToString
            = (x) => {
                var s = " | ";
                return $"{x.Index}{s}{x.DateTime}{s}{x.DateTime.Ticks}{s}{x.LogType}{s}{x.LogCategory}{s}{x.ProcessId}{s}{x.ProcessName}{s}{x.ThreadId}{s}{x.CallerMemberName}{s}{x.CallerFilePath}{s}{x.CallerLineNumber}{s}{x.Text}";
            };

        #endregion

        /// <summary>
        /// enabled log invokers
        /// </summary>
        protected Dictionary<int, ILogInvoker>
            LogInvokers =
            new Dictionary<int, ILogInvoker>();

        /// <summary>
        /// new instance
        /// </summary>
        public LogBase()
        {
            foreach (var v in Enum.GetValues(typeof(LogType)))
                EnabledLogTypes.Add((LogType)v, (LogType)v);
            foreach (var v in Enum.GetValues(typeof(LogCategory)))
                EnabledLogCategories.Add((LogCategory)v, (LogCategory)v);
        }

        #region filters queries

        public bool IsLogTypeEnabled(LogType logType)
        {
            return EnabledLogTypes.ContainsKey(logType);
        }

        public bool IsLogCategoryEnabled(LogCategory logCategory)
        {
            return EnabledLogCategories.ContainsKey(logCategory);
        }

        #endregion

        /// <summary>
        /// return a log invoker if any depending on log filters, null otherwise
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="logCategory"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        /// <returns></returns>
        protected ILogInvoker GetLogInvoker(
            LogType logType,
            LogCategory logCategory,
            string callerMemberName,
            int callerLineNumber,
            string callerFilePath
            )
        {
            if (IsLogTypeEnabled(logType)
                && IsLogCategoryEnabled(logCategory))
            {
                var id = Thread.CurrentThread.ManagedThreadId;
                if (!LogInvokers.TryGetValue(id, out ILogInvoker o))
                    LogInvokers
                        .Add(
                            id,
                            o = new LogInvoker());
                o.Log = this;
                o.LogType = logType;
                o.LogCategory = logCategory;
                o.CallerFilePath = callerFilePath;
                o.CallerLineNumber = callerLineNumber;
                o.CallerMemberName = callerMemberName;
                return o;
            }
            return null;
        }

        public ILogInvoker Info(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return GetLogInvoker(
                LogType.Info, 
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public ILogInvoker Warning(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return GetLogInvoker(
                LogType.Warning,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public ILogInvoker Error(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return GetLogInvoker(
                LogType.Error,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public ILogInvoker Debug(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return GetLogInvoker(
                LogType.Debug,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public void Info(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, LogType.Info, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public void Error(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, LogType.Error, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public void Warning(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, LogType.Warning, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public void Debug(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            Add(text, LogType.Debug, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

        public LogItem Add(
            string text,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            // TODO: 
            // 1. build log item text depending on schema
            // 2. do it in a dervied class
            //System.Diagnostics.Debug.WriteLine(text);

            var it =
                    new LogItem(
                        text,
                        logType,
                        logCategory,
                        callerMemberName,
                        callerLineNumber,
                        callerFilePath
                        );

            if (IsRecordingEnabled)
            {
                if (AddAction != null)
                    AddAction.Invoke(LogItems, it);
                else
                    LogItems.Add(it);
            }

            Add(it);

            return it;
        }

        public abstract void Add(LogItem logItem);
    }
}
