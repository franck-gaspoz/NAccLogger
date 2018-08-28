using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using NAccLogger;
using NAccLogger.Itf;

namespace NAccLogger.Ext
{
    public static class ObjectExt
    {
        /// <summary>
        /// check for adding a log entry using properties obtained from logger assigned to object
        /// </summary>
        /// <param name="caller">caller object</param>
        /// <returns></returns>
        public static ILogInvoker Log(
            this object caller
            )
        {
            throw new NotImplementedException();
            return null;
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Info' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Info(
            this object caller,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return NAccLogger.Log.Add(
                caller,
                LogType.Info,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Debug' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Debug(
            this object caller,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return NAccLogger.Log.Add(
                caller,
                LogType.Debug,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Warning' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Warning(
            this object caller,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return NAccLogger.Log.Add(
                caller,
                LogType.Warning,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Error' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Error(
            this object caller,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return NAccLogger.Log.Add(
                caller,
                LogType.Error,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }

        /// <summary>
        /// check for adding an entry to the log having log level 'Fatal' and the specified log category or LogCategory.NotDefined
        /// </summary>
        /// <param name="logCategory">category of the log entry from LogCategory</param>
        /// <param name="callerMemberName">retreive the name of the method or property wich called the log</param>
        /// <param name="callerLineNumber">retreive the name of the line number (if available) where the log call was done</param>
        /// <param name="callerFilePath">retreive the filename (if available) of the source code where the log call was done</param>
        /// <returns>null if log entry doesn't match log filters, else return an invoker to call the T method</returns>
        public static ILogInvoker Fatal(
            this object caller,
            LogCategory logCategory = LogCategory.NotDefined,
            [CallerMemberName] string callerMemberName = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerFilePath] string callerFilePath = ""
            )
        {
            return NAccLogger.Log.Add(
                caller,
                LogType.Fatal,
                logCategory,
                callerMemberName,
                callerLineNumber,
                callerFilePath
                );
        }
    }
}
