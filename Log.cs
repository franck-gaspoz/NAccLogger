using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using NAccLogger.Itf;

namespace NAccLogger
{
    /// <summary>
    /// NAccLogger static facade
    /// wraps calls through static methods to logger(s) implementation abstractions
    /// </summary>
    public static class Log
    {
        #region attributes

        /// <summary>
        /// common loggers parameters
        /// </summary>
        public static LogParameters LoggerParameters
            = new LogParameters();

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
        /// get or replace the default add to log action
        /// </summary>
        public static Action<BindingList<ILogItem>, ILogItem> AddAction
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
        
        #endregion

        #region facade specific operations

        /// <summary>
        /// returns a wrapper to log items list
        /// </summary>
        public static List<ILogItem> LogItems
        {
            get
            {
                return LogImpl.LogItems.ToList();
            }
        }
        
        /// <summary>
        /// set the root logger implementation
        /// </summary>
        /// <param name="log"></param>
        public static void SetLogger(ILog log)
        {
            LogImpl = log;
        }

        /// <summary>
        /// get the root logger implementation
        /// </summary>
        /// <returns>log interface of the current logger implementation</returns>
        public static ILog GetLogger()
        {
            return LogImpl;
        }

        #endregion

        #region ILog interface wrappers

        #region log add entry operation with filtering

        /// <summary>
        /// check for adding an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="logType">type of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Add(
            object caller,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Add(
                caller,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
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
        /// check for adding an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
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
        /// check for adding an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
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
        /// check for adding an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
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
        /// check for adding an entry to the log having log level 'Fatal' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Fatal(
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return LogImpl.Fatal(
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        #endregion

        #region log add entry operations without filtering

        /// <summary>
        /// add header entry to the log
        /// </summary>
        public static void Header()
        {
            LogImpl.Header();
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
        /// <param name="caller">caller object</param>
        /// <param name="logType">type of the log entry from LogType</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        public static void Add(
            string text,
            object caller = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            LogImpl.Add(
                text,
                caller,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        #endregion

        #endregion
    }
}
