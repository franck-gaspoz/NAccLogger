using NAccLogger.Itf;
using System;
using System.Collections.Generic;

namespace NAccLogger.Impl
{
    /// <summary>
    /// buffer of log items
    /// </summary>
    public class LogItemBuffer
        : ILogItemBuffer
    {
        /// <summary>
        /// log items buffer
        /// </summary>
        public Dictionary<int, ILogItem> LogItems { get; } =
            new Dictionary<int, ILogItem>();

        /// <summary>
        /// buffer of defered items
        /// </summary>
        protected List<ILogItem> DeferedItems
            = new List<ILogItem>();

        /// <summary>
        /// indicates if deferred log adds is enabled or not
        /// </summary>
        public bool IsDeferedAddEnabled { get; set; } = false;

        /// <summary>
        /// size of range of defered adds (default 100)
        /// </summary>
        public int DeferedAddRangeSize { get; set; } = 100;

        /// <summary>
        /// event item added
        /// </summary>
        public event EventHandler<ILogItem> ItemAdded;

        /// <summary>
        /// event item range added
        /// </summary>
        public event EventHandler<IEnumerable<ILogItem>> ItemRangeAdded;

        /// <summary>
        /// add a log item to the buffer
        /// </summary>
        /// <param name="logItem">log item</param>
        public void Add(ILogItem logItem)
        {
            if (!IsDeferedAddEnabled)
            {
                if (!LogItems.ContainsKey(logItem.Index))
                {
                    LogItems.Add(
                        logItem.Index,
                        logItem);
                    ItemAdded?.Invoke(this, logItem);
                }
            } else
            {
                DeferedItems.Add(logItem);
                if (DeferedItems.Count==DeferedAddRangeSize)
                    Flush();
            }
        }

        /// <summary>
        /// flush defered buffer
        /// </summary>
        public void Flush()
        {
            var lst = new List<ILogItem>();
            lst.AddRange(DeferedItems);
            DeferedItems.Clear();
            foreach (var o in lst)
                if (!LogItems.ContainsKey(o.Index))
                    LogItems.Add(
                        o.Index,
                        o);
            ItemRangeAdded?.Invoke(this, lst);
        }

    }
}
