using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NAccLogger
{
    /// <summary>
    /// interface to any log
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// enable/disable log items recording in log history (LogItems)
        /// </summary>
        bool IsRecordingEnabled { get; set; }

        /// <summary>
        /// log items history
        /// </summary>
        BindingList<LogItem> LogItems { get; }

        /// <summary>
        /// replace the default add to LogItems action if defined
        /// </summary>
        Action<BindingList<LogItem>, LogItem> AddAction { get; set; }

        /// <summary>
        /// default log item add action for recording log items into LogItems
        /// </summary>
        /// <param name="text"></param>
        /// <param name="logType"></param>
        /// <param name="logCategory"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        LogItem Add(string text, LogType logType = LogType.NotDefined, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");

        void Debug(string text, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        void Error(string text, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        void Info(string text, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        void Warning(string text, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");

        ILogInvoker Debug(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        ILogInvoker Error(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        ILogInvoker Info(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
        ILogInvoker Warning(LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "");
    }
}