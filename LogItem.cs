using System;
using System.Diagnostics;
using System.Threading;

namespace NAccLogger
{
    public class LogItem
    {
        static long _Index = 0;

        public long Index { get; protected set; }

        public DateTime DateTime { get; protected set; }

        public long ThreadId { get; protected set; }

        public long ProcessId { get; protected set; }

        public string ProcessName { get; protected set; }

        public string Text { get; protected set; }

        public LogType LogType { get; protected set; }

        public LogCategory LogCategory { get; protected set; }

        public string CallerMemberName { get; protected set; }

        public int CallerLineNumber { get; protected set; }

        public string CallerFilePath { get; protected set; }

        public LogItem(
            string text, 
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined, 
            string callerMemberName = "", 
            int callerLineNumber = -1, 
            string callerFilePath = ""
            )
        {
            Index = _Index;
            _Index++;
            DateTime = DateTime.Now;
            var p = Process.GetCurrentProcess();
            ProcessId = p.Id;
            ProcessName = p.ProcessName;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            Text = text;
            LogType = logType;
            LogCategory = logCategory;
            CallerMemberName = callerMemberName;
            CallerLineNumber = callerLineNumber;
            CallerFilePath = callerFilePath;
        }

        public override string ToString()
        {
            return $"[{LogType}] {Text}";
        }
    }
}
