using NAccLogger.Itf;
using System;

namespace NAccLogger.Impl
{
    /// <summary>
    /// 'anonymous' fast log implementer from lambda expressions
    /// </summary>
    public class LogImpl
        : LogBase
    {
        /// <summary>
        /// log action
        /// </summary>
        public Action<ILogItem> LogAction { get; set; }

        /// <summary>
        /// bulid a new instance
        /// </summary>
        /// <param name="logParameters"></param>
        public LogImpl(
            LogParameters logParameters = null)
            : base(logParameters) { }

        /// <summary>
        /// bulid a new instance
        /// </summary>
        /// <param name="logAction">action for logging log items</param>
        /// <param name="logParameters"></param>
        public LogImpl(
            Action<ILogItem> logAction,
            LogParameters logParameters = null)
            : base(logParameters) {
            LogAction = logAction;
        }

        /// <summary>
        /// log an item (impl.)
        /// </summary>
        /// <param name="logItem">log item</param>
        public override void Log(ILogItem logItem)
        {
            if (LogAction==null)
                throw new NotImplementedException();
            LogAction.Invoke(logItem);
        }
    }
}
