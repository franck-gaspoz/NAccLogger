using System.Runtime.CompilerServices;

namespace NAccLogger
{
    public class LogInvoker
        : ILogInvoker
    {
        public LogType LogType { get; set; }
        public LogCategory LogCategory { get; set; }
        public ILog Log { get; set; }
        public string CallerMemberName { get; set; }
        public int CallerLineNumber { get; set; }
        public string CallerFilePath { get; set; }

        public LogInvoker() { }

        public void T(string text)
        {
            Add(text, LogType, LogCategory, CallerMemberName, CallerLineNumber, CallerFilePath);
        }

        void Add(string text, LogType logType = LogType.NotDefined, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            Log.Add(text, logType, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }

    }
}
