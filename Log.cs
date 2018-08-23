using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAccLogger
{
    /// <summary>
    /// NAccLogger static facade
    /// wraps calls through static methods to logger(s) implementation
    /// </summary>
    public static class Log
    {
        static ILog LogImpl { get; set; }
            = new Loggers.SystemConsole();

        /// <summary>
        /// enable/disable log items records in LogItems
        /// </summary>
        public static bool IsRecordingEnabled
        {
            get
            {
                return LogImpl.IsRecordingEnabled;
            }
            set
            {
                LogImpl.IsRecordingEnabled = true;
            }
        }

        /// <summary>
        /// log items list
        /// </summary>
        public static BindingList<LogItem> LogItems
        {
            get
            {
                return LogImpl.LogItems;
            }
        }

        /// <summary>
        /// get or replace the logger add item action
        /// </summary>
        public static Action<BindingList<LogItem>, LogItem> AddAction
        {
            get
            {
                return LogImpl.AddAction;
            }
            set
            {
                LogImpl.AddAction = value;
            }
        }

        /// <summary>
        /// set the logger implementation
        /// </summary>
        /// <param name="log"></param>
        public static void SetLogger(ILog log)
        {
            LogImpl = log;
        }

        /// <summary>
        /// get the logger implementation
        /// </summary>
        /// <returns>log interface of the current logger implementation</returns>
        public static ILog GetLogger()
        {
            return LogImpl;
        }

        public static ILogInvoker Info(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Info(
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static ILogInvoker Debug(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Debug(
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static ILogInvoker Warning(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Warning(
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static ILogInvoker Error(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Error(
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static void Info(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Info(
                text,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static void Error(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Error(
                text,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static void Warning(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Warning(
                text,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static void Debug(
            string text,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Debug(
                text,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        public static void Add(
            string text,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Add(
                text,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }
    }
}
