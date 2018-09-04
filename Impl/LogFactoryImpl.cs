using NAccLogger.Itf;
using System;

namespace NAccLogger.Impl
{
    /// <summary>
    /// build a log components factory 'anonymous' implementation from lambda expressions
    /// <para>use provided create functions, if not defined use default implementation from LogFactory</para>
    /// </summary>
    public class LogFactoryImpl
        : LogFactory
    {
        /// <summary>
        /// create log filter function
        /// </summary>
        public Func<ILogFilter> CreateLogFilterFunction { get; set; }

        /// <summary>
        /// create log item function
        /// </summary>
        public Func<
                string,
                string,
                LogType,
                LogCategory,
                string,
                int,
                string,
                ILogItem> 
            CreateLogItemFunction { get; set; }

        /// <summary>
        /// create log item text formatter function
        /// </summary>
        public Func<ILogItemTextFormatter> CreateLogItemTextFormatterFunction { get; set; }

        /// <summary>
        /// create log dispatcher function
        /// </summary>
        public Func<ILogDispatcher> CreateLogDispatcherFunction { get; set; }

        /// <summary>
        /// create log item buffer function
        /// </summary>
        public Func<ILogItemBuffer> CreateLogItemBufferFunction { get; set; }

        /// <summary>
        /// create a log dispatcher
        /// </summary>
        /// <returns>a log dispatcher</returns>
        public override ILogDispatcher CreateLogDispatcher()
        {
            if (CreateLogDispatcherFunction == null)
                return base.CreateLogDispatcher();
            return CreateLogDispatcherFunction();
        }

        /// <summary>
        /// create a log filter
        /// </summary>
        /// <returns>log filter impl.</returns>
        public override ILogFilter CreateLogFilter()
        {
            if (CreateLogFilterFunction == null)
                return base.CreateLogFilter();
            return CreateLogFilterFunction();
        }

        /// <summary>
        /// create a log item text formatter
        /// </summary>
        /// <returns>log item text formatter impl.</returns>
        public override ILogItemTextFormatter CreateLogItemTextFormatter()
        {
            if (CreateLogItemTextFormatterFunction == null)
                return base.CreateLogItemTextFormatter();
            return CreateLogItemTextFormatterFunction();
        }

        /// <summary>
        /// create a log item buffer
        /// </summary>
        /// <returns>log item buffer</returns>
        public override ILogItemBuffer CreateLogItemBuffer()
        {
            if (CreateLogItemBufferFunction == null)
                return base.CreateLogItemBuffer();
            return CreateLogItemBufferFunction();
        }

        /// <summary>
        /// create a new log item
        /// </summary>
        /// <param name="text">text of the log item</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="logType">type of the log entry</param>
        /// <param name="logCategory">category of the log entry</param>
        /// <param name="callerMemberName">name of the property or method wich made the call</param>
        /// <param name="callerLineNumber">line number in the source file where the call was done</param>
        /// <returns>log item impl.</returns>
        public override ILogItem CreateLogItem(
            string text, 
            string callerTypeName = null, 
            LogType logType = LogType.NotDefined, 
            LogCategory logCategory = LogCategory.NotDefined, 
            string callerMemberName = "", 
            int callerLineNumber = -1, 
            string callerFilePath = "")
        {
            if (CreateLogItemFunction == null)
                return base.CreateLogItem(
                    text,
                    callerTypeName,
                    logType,
                    logCategory,
                    callerMemberName,
                    callerLineNumber,
                    callerFilePath
                    );
            return CreateLogItemFunction(text, callerTypeName, logType, logCategory, callerMemberName, callerLineNumber, callerFilePath);
        }
    }
}
