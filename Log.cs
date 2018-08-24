using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAccLogger
{
    /// <summary>
    /// NAccLogger static facade
    /// wraps calls through static methods to logger(s) implementation abstractions
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// log implementation abstraction
        /// </summary>
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
        /// get or replace the default add to log action
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

        /// <summary>
        /// try to add an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
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

        /// <summary>
        /// try to add an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
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

        /// <summary>
        /// try to add an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
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

        /// <summary>
        /// try to add an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
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

        /// <summary>
        /// add an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
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

        /// <summary>
        /// add an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
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

        /// <summary>
        /// add an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
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

        /// <summary>
        /// add an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
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

        /// <summary>
        /// add a new log entry to the log having the specified properties
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logType">type of the log entry from LogType</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
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
