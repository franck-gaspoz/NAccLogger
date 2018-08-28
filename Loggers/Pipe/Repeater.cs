using NAccLogger.Impl;
using NAccLogger.Itf;
using System;
using System.Collections.Generic;

namespace NAccLogger.Loggers.Pipe
{
    /// <summary>
    /// pipe repeater : allow to substitute a single logger implementation by a list of logger, which are called one after the other
    /// <para>the substitution is performed after filters are applied, so the loggers in the list received only filtered items</para>
    /// <para>loggers are called in the specific order of the loggers linked list</para>
    /// </summary>
    public class Repeater
        : LogBase
    {
        /// <summary>
        /// ordered list of loggers
        /// </summary>
        public LinkedList<ILog> Loggers =
            new LinkedList<ILog>();

        /// <summary>
        /// build a new repeater logger
        /// </summary>
        /// <param name="logParameters"></param>
        public Repeater(LogParameters logParameters = null)
            : base(logParameters) { }

        /// <summary>
        /// build a new repeater logger initialized with logger list
        /// </summary>
        /// <param name="logParameters"></param>
        public Repeater(
            List<ILog> loggers,
            LogParameters logParameters = null)
            : base(logParameters) {
            if (loggers == null)
                throw new ArgumentNullException(nameof(loggers));
            foreach (var o in loggers)
                Loggers.AddLast(o);
        }

        /// <summary>
        /// add a new log entry to the repeater logger
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem)
        {
            foreach (var o in Loggers)
                o.Log(logItem);
        }
    }
}
