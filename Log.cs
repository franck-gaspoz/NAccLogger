using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using NAccLogger.Itf;
using NAccLogger.Loggers;
using NAccLogger.Loggers.Pipe;
using NAccLogger.Impl;

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
        /// log components factory
        /// </summary>
        public static ILogFactory LogFactory { get; set; }
            = new LogFactory();
        
        /// <summary>
        /// common loggers parameters
        /// </summary>
        public static LogParameters LogParameters
            = new LogParameters();

        /// <summary>
        /// log dispatcher
        /// </summary>
        public static ILogDispatcher LogDispatcher { get; set; }
            = LogParameters
                .LogFactory
                .CreateLogDispatcher();

        /// <summary>
        /// log implementation abstraction
        /// </summary>
        static ILog LogImpl { get; set; }
            = new Dispatcher(
                new List<ILog>{
                    new Loggers.Console.SystemConsole(),
                    new SystemDiagnostics()
                });
     
        #endregion

        #region facade specific operations
       
        /// <summary>
        /// set the root logger pipeline implementation
        /// </summary>
        /// <param name="log"></param>
        public static void SetLogger(ILog log)
        {
            LogImpl = log;
        }

        /// <summary>
        /// get the root logger pipeline implementation
        /// </summary>
        /// <returns>log interface of the current logger implementation</returns>
        public static ILog GetLogger()
        {
            return LogImpl;
        }

        #endregion

        /// <summary>
        /// return appropriate logs impl.
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>        
        /// <param name="callerLineNumber">line number where log has been called</param>
        /// <param name="callerFilePath">file path where source code called the log</param>
        /// <returns>log impl. is get either from dispatcher or pipeline depending on dispatching rules and pipeline configuration</returns>
        static 
            (Dispatcher dispatcher,
            ILog pipelineLog)
            GetImpl(
                object caller,
                string callerTypeName,
                string callerMemberName,
                LogType logType,
                LogCategory logCategory,
                int callerLineNumber,
                string callerFilePath
            )
        {
            if (LogDispatcher == null)
                return (null, LogImpl);

            return (LogDispatcher
                .GetDispatcher(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    logType,
                    logCategory
                ), LogImpl);
        }

        static void CallImpl(
            string text,
            object caller,
            string callerTypeName,
            LogType logType,
            LogCategory logCategory,
            string callerMemberName,
            int callerLineNumber,
            string callerFilePath
            )
        {
            var (dispatcher, pipelineLog) = GetImpl(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    LogType.Info,
                    logCategory,
                    callerLineNumber,
                    callerFilePath);

            bool pipelineEnabled = true;
            
            dispatcher?.Add(
                text,
                caller,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );

            if (pipelineEnabled)
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
        
        /// <summary>
        /// non filtered log action call to impl.
        /// </summary>
        static ILogInvoker GetCallableImpl(
            object caller,
            string callerTypeName,
            LogType logType,
            LogCategory logCategory,
            string callerMemberName,
            int callerLineNumber,
            string callerFilePath
            )
        {
            var (dispatcher, pipelineLog) = GetImpl(
                    caller,
                    callerTypeName,
                    callerMemberName,
                    logType,
                    logCategory,
                    callerLineNumber,
                    callerFilePath);

            if (dispatcher != null)
            { 
                if (!dispatcher.Loggers.Contains(LogImpl))
                    dispatcher.Loggers.AddLast(LogImpl);

                return 
                    dispatcher?
                    .Add(
                        caller,
                        logType,
                        logCategory,
                        callerMemberName,
                        callerLineNumber,
                        callerFilePath
                    );
            }
            else
                return LogImpl
                    .Add(
                        caller,
                        logType,
                        logCategory,
                        callerMemberName,
                        callerLineNumber,
                        callerFilePath
                    );
        }
       
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
            return
                GetCallableImpl(
                    caller,
                    caller?.GetType().FullName,                    
                    logType,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
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
            return
                GetCallableImpl(
                    null,
                    null,
                    LogType.Info,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
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
            return
                GetCallableImpl(
                    null,
                    null,
                    LogType.Debug,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
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
            return
                GetCallableImpl(
                    null,
                    null,
                    LogType.Warning,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
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
            return
                GetCallableImpl(
                    null,
                    null,
                    LogType.Error,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
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
            return
                GetCallableImpl(
                    null,
                    null,
                    LogType.Fatal,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath);
        }

        #endregion

        #region log add entry operations without filtering

        /// <summary>
        /// add header entry to the log
        /// <para>TODO: fix poor desing: is not dispatched ?</para>
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
            CallImpl(
                null,
                null,
                text,
                LogType.Info,
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
            CallImpl(
                null,
                null,
                text,
                LogType.Error,
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
            CallImpl(
                null,
                null,
                text,
                LogType.Warning,
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
            CallImpl(
                null,
                null,
                text,
                LogType.Debug,
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
            CallImpl(                
                text,
                caller,
                caller?.GetType().FullName,
                logType,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// add a log item to the log
        /// <para>usage: transfert a log item from a log to a log, not dispatchable</para>
        /// </summary>
        /// <param name="logItem">log item</param>
        public static void Add(ILogItem logItem)
        {
            LogImpl.Add(logItem);
        }

        #endregion

        #endregion
    }
}
