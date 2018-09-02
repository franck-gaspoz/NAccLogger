using System.Runtime.CompilerServices;

namespace NAccLogger.Itf
{
    /// <summary>
    /// interface to any log
    /// </summary>
    public interface ILog
    {
        #region attributes

        /// <summary>
        /// log parameters
        /// </summary>
        LogParameters LogParameters { get; }

        /// <summary>
        /// if true, any log action is forwarded to nexts loggers in a dispatcher or a pipeline where this log is contained, otherwize this logger is the last to handle the log action
        /// </summary>
        bool IsForwardEnabled { get; set; }

        #endregion
        
        /// <summary>
        /// add a log item to the log
        /// </summary>
        /// <param name="logItem">log item</param>
        void Log(ILogItem logItem);

        #region log add entry operations without filtering

        /// <summary>
        /// add header entry to the log
        /// </summary>
        void Header();

        /// <summary>
        /// add a log item to the log
        /// </summary>
        /// <param name="logItem">log item</param>
        void Add(ILogItem logItem);

        /// <summary>
        /// build a new log item and add it to this logger
        /// </summary>
        /// <param name="text"></param>
        /// <param name="caller">caller object</param>
        /// <param name="logType">log entry type</param>
        /// <param name="logCategory">log entry category</param>
        /// <param name="callerMemberName">caller member name</param>
        /// <param name="callerLineNumber">line number where log has been called</param>
        /// <param name="callerFilePath">file path where source code called the log</param>
        ILogItem Add(
            string text,
            object caller = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            );

        /// <summary>
        /// add an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        void Debug(
            string text, 
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// add an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        void Error(
            string text, 
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// add an entry to the log having log level 'Fatal' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        void Fatal(
            string text, 
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// add an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        void Info(
            string text, 
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// add an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="text">message of the log entry</param>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        void Warning(
            string text, 
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        #endregion

        #region log add entry operations with filtering

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
        ILogInvoker Add(
            object caller,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// check for adding an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        ILogInvoker Debug(
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// check for adding an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        ILogInvoker Error(
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// check for adding an entry to the log having log level 'Fatal' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        ILogInvoker Fatal(
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// check for adding an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        ILogInvoker Info(
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        /// <summary>
        /// check for adding an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        ILogInvoker Warning(
            LogCategory logCategory = LogCategory.NotDefined, 
            [CallerMemberName] string callerMemberName = "", 
            [CallerLineNumber] int callerLineNumber = 0, 
            [CallerFilePath] string callerFilePath = "");

        #endregion
    }
}