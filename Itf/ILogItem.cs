using System;
using System.Net;

namespace NAccLogger.Itf
{
    /// <summary>
    /// log item interface containing a minimalistic set of properties
    /// </summary>
    public interface ILogItem
    {
        string CallerFilePath { get; }
        int CallerLineNumber { get; }
        string CallerMemberName { get; }
        object CallerTypeName { get; set; }
        DateTime DateTime { get; }
        long Index { get; }
        bool IsTextOnly { get; set; }
        LogCategory LogCategory { get; }
        string LogEntryText { get; set; }
        LogType LogType { get; }
        long ProcessId { get; }
        string ProcessName { get; }
        string Text { get; }
        long ThreadId { get; }
        string HostName { get; }
        IPAddress IPAddress { get; }

        /// <summary>
        /// init. automatic properties
        /// </summary>
        void Init();
    }
}