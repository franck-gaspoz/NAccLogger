using System.Collections.Generic;

namespace NAccLogger.Itf
{
    /// <summary>
    /// format a log item into a string
    /// </summary>
    public interface ILogItemTextFormatter
    {
        /// <summary>
        /// columns seperator
        /// </summary>
        string LogItemToStringColumnsSeparator { get; set; }

        /// <summary>
        /// return a log entry header
        /// </summary>
        /// <returns></returns>
        string GetHeader();

        /// <summary>
        /// format log item to a string
        /// </summary>
        /// <param name="it">log item</param>
        /// <returns>log entry as a string</returns>
        string LogItemToString(ILogItem it);

        /// <summary>
        /// set columns taken into account when formatting log item to text
        /// </summary>
        /// <param name="columns"></param>
        void SetColumns(IEnumerable<string> columns);

        /// <summary>
        /// restore default columns
        /// </summary>
        void ResetColumns();
    }
}