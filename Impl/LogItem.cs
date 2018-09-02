using NAccLogger.Ext;
using NAccLogger.Itf;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Linq;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace NAccLogger.Impl
{
    /// <summary>
    /// item of log
    /// </summary>
    [Serializable]
    public class LogItem : ILogItem
    {
        /// <summary>
        /// log entry counter
        /// </summary>
        static int _Index = 0;

        /// <summary>
        /// log entry index
        /// </summary>
        public int Index { get; protected set; }

        /// <summary>
        /// date and time when the call was done
        /// </summary>
        public DateTime DateTime { get; protected set; }

        /// <summary>
        /// Host name
        /// </summary>
        public string HostName { get; protected set; }

        /// <summary>
        /// IPAddress
        /// </summary>
        public IPAddress IPAddress { get; protected set; }

        /// <summary>
        /// process id of the caller
        /// </summary>
        public long ProcessId { get; protected set; }

        /// <summary>
        /// process name of the caller
        /// </summary>
        public string ProcessName { get; protected set; }

        /// <summary>
        /// thread id of the caller
        /// </summary>
        public long ThreadId { get; protected set; }

        /// <summary>
        /// type of the log entry
        /// </summary>
        public LogType LogType { get; protected set; }

        /// <summary>
        /// category of the log entry
        /// </summary>
        public LogCategory LogCategory { get; protected set; }

        /// <summary>
        /// message of the log entry
        /// </summary>
        public string Text { get; protected set; }

        /// <summary>
        /// caller type name
        /// </summary>        
        public object CallerTypeName { get; set; }

        /// <summary>
        /// method or property name of the caller
        /// </summary>
        public string CallerMemberName { get; protected set; }
        
        /// <summary>
        /// filename of the caller
        /// </summary>
        public string CallerFilePath { get; protected set; }

        /// <summary>
        /// line number of the caller
        /// </summary>
        public int CallerLineNumber { get; protected set; }
        
        /// <summary>
        /// text of the log entry after formatted by any logger
        /// </summary>
        [IgnoreColumn]
        public string LogEntryText { get; set; }
        
        /// <summary>
        /// indicates if a log entry has no properties except Text (case of header log entry)
        /// </summary>
        [IgnoreColumn]
        public bool IsTextOnly { get; set; } = false;

        /// <summary>
        /// new log item having mandatory,not automatic properties
        /// </summary>
        /// <param name="text">text of the log item</param>
        /// <param name="callerTypeName">caller type name</param>
        /// <param name="logType">type of the log entry</param>
        /// <param name="logCategory">category of the log entry</param>
        /// <param name="callerMemberName">name of the property or method wich made the call</param>
        /// <param name="callerLineNumber">line number in the source file where the call was done</param>
        /// <param name="callerFilePath">file name where the call was done</param>
        public LogItem(
            string text,
            string callerTypeName = null,
            LogType logType = LogType.NotDefined,
            LogCategory logCategory = LogCategory.NotDefined, 
            string callerMemberName = "", 
            int callerLineNumber = -1, 
            string callerFilePath = ""
            )
        {
            Init();
            
            CallerTypeName = callerTypeName;
            Text = text;
            LogType = logType;
            LogCategory = logCategory;
            CallerMemberName = callerMemberName;
            CallerLineNumber = callerLineNumber;
            CallerFilePath = callerFilePath;            
        }

        /// <summary>
        /// init automatic properties
        /// </summary>
        public virtual void Init()
        {
            Index = _Index;
            _Index++;
            DateTime = DateTime.Now;
            var p = Process.GetCurrentProcess();
            ProcessId = p.Id;
            ProcessName = p.ProcessName;
            ThreadId = Thread.CurrentThread.ManagedThreadId;

            HostName = Network.HostName;
            IPAddress = Network.IP_V4 ?? Network.IP_V6;
        }

        /// <summary>
        /// returns a simple string view of the log item for debug purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[{LogType}] {Text}";
        }
    }
}
