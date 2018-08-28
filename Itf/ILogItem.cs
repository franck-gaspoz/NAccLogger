using System;

namespace NAccLogger.Itf
{
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
    }
}