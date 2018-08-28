using NAccLogger.Ext;
using NAccLogger.Itf;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace NAccLogger.Impl
{
    /// <summary>
    /// format a log item into a string
    /// </summary>
    public class LogItemTextFormatter : ILogItemTextFormatter
    {
        /// <summary>
        /// columns seperator
        /// </summary>
        public string LogItemToStringColumnsSeparator
        { get; set; } = " | ";

        /// <summary>
        /// log item properties that can be be added to a log entry
        /// </summary>
        protected Dictionary<string, PropertyInfo> AvailableColumns
            = new Dictionary<string, PropertyInfo>();

        /// <summary>
        /// store the minimal column size for each column
        /// </summary>
        protected Dictionary<string, int> MinColumnSize =
            new Dictionary<string, int>();

        /// <summary>
        /// formatted log items columns
        /// </summary>
        public LinkedList<string> Columns
            = new LinkedList<string>();

        /// <summary>
        /// build a new formatter
        /// <para>check LogItem type to retreive available columns</para>
        /// </summary>
        public LogItemTextFormatter()
        {
            ResetColumns();
        }

        /// <summary>
        /// restore default columns
        /// </summary>
        public void ResetColumns()
        {
            AvailableColumns.Clear();
            Columns.Clear();
            MinColumnSize.Clear();

            foreach (var p in typeof(LogItem).GetProperties())
            {
                var at = p.GetCustomAttribute<IgnoreColumnAttribute>(true);
                if (at == null)
                {
                    AvailableColumns.Add(p.Name, p);
                    Columns.AddLast(p.Name);
                    MinColumnSize.Add(p.Name, p.Name.Length);
                }
            }
        }

        /// <summary>
        /// set columns taken into account when formatting log item to text
        /// </summary>
        /// <param name="columns"></param>
        public void SetColumns(IEnumerable<string> columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));
            Columns.Clear();
            MinColumnSize.Clear();
            foreach ( var c in columns )
            {
                if (c == null)
                    throw new ArgumentNullException("column name can't be null");
                if (!AvailableColumns.ContainsKey(c))
                    UnknownLogItemColumnException(c);
                Columns.AddLast(c);
                MinColumnSize.Add(c, c.Length);
            }
        }

#if deprecated
        // alternative fast way ...
        public Func<LogItem, string> LogItemToStringFunc
            = (x) => {
                var s = " | ";
                return $"{x.Index}{s}{x.DateTime}{s}{x.DateTime.Ticks}{s}{x.LogType}{s}{x.LogCategory}{s}{x.ProcessId}{s}{x.ProcessName}{s}{x.ThreadId}{s}{x.Text}{s}{x.CallerTypeName}{s}{x.CallerMemberName}{s}{x.CallerFilePath}{s}{x.CallerLineNumber}";
            };
#endif

        /// <summary>
        /// return a log entry header
        /// </summary>
        /// <returns></returns>
        public string GetHeader()
        {
            var sb = new StringBuilder();
            int n = 0;
            foreach (var s in Columns)
            {
                if (n > 0)
                    sb.Append(LogItemToStringColumnsSeparator);
                n++;
                var mincsize = MinColumnSize[s];
                if (s.Length > mincsize)
                {
                    mincsize = s.Length;
                    MinColumnSize[s] = mincsize;
                }
                if (s.Length < mincsize)
                    sb.Append(s.PadRight(mincsize));
                else
                    sb.Append(s);
            }
            return sb.ToString();
        }

        /// <summary>
        /// format log item to a string
        /// </summary>
        /// <param name="it">log item</param>
        /// <returns>log entry as a string</returns>
        public string LogItemToString(ILogItem it)
        {
            if (it.IsTextOnly)
                return it.Text;
            var sb = new StringBuilder();
            int n = 0;
            foreach ( var cn in Columns )
            {
                if (n > 0)
                    sb.Append(LogItemToStringColumnsSeparator);
                n++;
                if (AvailableColumns.TryGetValue(cn, out var cp))
                {
                    var v = cp.GetValue(it);
                    var s = (v == null) ? string.Empty : v.ToString();
                    var mincsize = MinColumnSize[cn];
                    if (s.Length > mincsize)
                    {
                        mincsize = s.Length;
                        MinColumnSize[cn] = mincsize;
                    }
                    if (s.Length < mincsize)
                        sb.Append(s.PadRight(mincsize));
                    else
                        sb.Append(s);
                }
                else
                    UnknownLogItemColumnException(cn);
            }
            return sb.ToString();
            //return LogItemToStringFunc(it);
        }

        void UnknownLogItemColumnException(string column)
        {
            throw new Exception($"Unknown log item column: '{column}'");
        }
    }
}
