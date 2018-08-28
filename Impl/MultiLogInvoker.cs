using NAccLogger.Itf;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NAccLogger.Impl
{
    public class MultiLogInvoker
        : LogInvoker
    {
        /// <summary>
        /// ordered list of loggers
        /// </summary>
        public LinkedList<ILog> Loggers =
            new LinkedList<ILog>();

        public MultiLogInvoker() { }

        public override void T(string text)
        {
            Add(text, Caller, LogType, LogCategory, CallerMemberName, CallerLineNumber, CallerFilePath);
        }

        void Add(string text, object caller, LogType logType = LogType.NotDefined, LogCategory logCategory = LogCategory.NotDefined, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "")
        {
            foreach ( var o in Loggers )
                o.Add(text, caller, logType, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }
    }
}
