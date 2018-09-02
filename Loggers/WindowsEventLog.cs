using NAccLogger.Impl;
using NAccLogger.Itf;
using System;
using System.Diagnostics;
using System.Linq;

namespace NAccLogger.Loggers
{
    /// <summary>
    /// logger to windows event log
    /// </summary>
    public class WindowsEventLog
        : LogBase
    {
        /// <summary>
        /// name of the windows event log
        /// </summary>
        public string EventLogName { get; protected set; }

        public bool ResetEventLog { get; protected set; }

        bool SourceChecked = false;

        /// <summary>
        /// build a new windows event log logger logging to 'Application'
        /// </summary>
        /// <param name="logParameters">log parameters</param>
        public WindowsEventLog(
            LogParameters logParameters = null
            ) : base(logParameters)
        {
            EventLogName = "Application";
        }

        /// <summary>
        /// build a new windows event log logger
        /// </summary>
        /// <param name="eventLogName">name of the windows event log</param>
        /// <param name="logParameters">log parameters</param>
        public WindowsEventLog(
            string eventLogName,
            LogParameters logParameters = null            
            ) : base(logParameters)
        {
            EventLogName = eventLogName ?? throw new ArgumentNullException(nameof(eventLogName), "can't be null");
        }

        /// <summary>
        /// create a windows event log if not already exists
        /// </summary>
        /// <param name="name">event log name</param>
        /// <param name="appName">source name for the event log</param>
        void CreateEventLogIfNotExisting(
            string logName,
            string appName
            )
        {
            if (!SourceChecked)
            {
                if (EventLog.SourceExists(appName))
                    EventLog.DeleteEventSource(appName);
                SourceChecked = true;

                if (!EventLog.SourceExists(appName))
                {
                    var x = new EventSourceCreationData(
                            appName, logName
                        );

                    EventLog.CreateEventSource(
                        x
                        );
                }
            }
        }

        /// <summary>
        /// add a log entry to the system console
        /// </summary>
        /// <param name="logItem">log item to be added</param>
        public override void Log(ILogItem logItem)
        {
            CreateEventLogIfNotExisting(
                EventLogName,
                logItem.ProcessName
                );

            EventLogEntryType logEntryType
                = EventLogEntryType.Information;

            switch ( logItem.LogType )
            {
                case LogType.Error:
                case LogType.Fatal:
                    logEntryType = EventLogEntryType.Error;
                    break;
                case LogType.Warning:
                    logEntryType = EventLogEntryType.Warning;
                    break;
                case LogType.Info:
                case LogType.Debug:
                case LogType.NotDefined:
                    logEntryType = EventLogEntryType.Information;
                    break;
            }

            var msg = logItem.Text + Environment.NewLine;
            var cols = LogParameters.LogItemTextFormatter.GetColumns();
            var p = logItem.GetType().GetProperties();
            foreach ( var c in cols )
            {
                if (c!=nameof(ILogItem.Text))
                {
                    msg += Environment.NewLine;
                    msg += c + " = " +
                        p.Where(x => x.Name == c)
                        .FirstOrDefault()
                        ?.GetValue(logItem)
                        ?.ToString();                    
                }
            }

            var evlog = new EventLog
            {
                Log = EventLogName,
                Source = logItem.ProcessName
            };

            /*using (var evlog = 
                new EventLog(
                    EventLogName,
                    Environment.MachineName, 
                    logItem.ProcessName))
            {*/
            evlog.WriteEntry(
                    msg,
                    logEntryType,
                    (int)logItem.Index
                    );
            //}
        }
    }
}
