using NAccLogger.Itf;
using System.Collections.Generic;
using System.Threading;

namespace NAccLogger.Impl
{
    /// <summary>
    /// common log filter propertiess
    /// </summary>
    public abstract class LogFilterBase
    {
        /// <summary>
        /// per thread enabled log invokers
        /// </summary>
        protected Dictionary<int, ILogInvoker>
            LogInvokers =
            new Dictionary<int, ILogInvoker>();

        /// <summary>
        /// return a log invoker if any depending on log filters, null otherwise
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <param name="callerMemberName"></param>
        /// <param name="logType"></param>
        /// <param name="logCategory"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerFilePath"></param>
        /// <returns>log invoker to logger, else null</returns>
        protected ILogInvoker GetLogInvoker(
            ILog logger,
            object caller,
            string callerMemberName,
            LogType logType,
            LogCategory logCategory,
            int callerLineNumber,
            string callerFilePath
            )
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            if (!LogInvokers.TryGetValue(id,
                out ILogInvoker o))
                LogInvokers
                    .Add(
                        id,
                        o = new LogInvoker());

            // setup up the invoker for the current action
            o.Log = logger;
            o.Caller = caller;
            o.LogType = logType;
            o.LogCategory = logCategory;
            o.CallerFilePath = callerFilePath;
            o.CallerLineNumber = callerLineNumber;
            o.CallerMemberName = callerMemberName;
            return o;
        }
    }
}
