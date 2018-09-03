using System;
using System.Collections.Generic;

namespace NAccLogger.Itf
{
    /// <summary>
    /// buffer of log items
    /// </summary>
    public interface ILogItemBuffer
    {
        /// <summary>
        /// indicates if deferred log adds is enabled or not
        /// </summary>
        bool IsDeferedAddEnabled { get; set; }

        /// <summary>
        /// size of range of defered adds
        /// </summary>
        int DeferedAddRangeSize { get; set; }

        /// <summary>
        /// flush defered buffer
        /// </summary>
        void Flush();

        /// <summary>
        /// add a log item to the buffer
        /// </summary>
        /// <param name="logItem">log item</param>
        void Add(ILogItem logItem);

        /// <summary>
        /// event item added
        /// </summary>
        event EventHandler<ILogItem> ItemAdded;

        /// <summary>
        /// event item range added
        /// </summary>
        event EventHandler<IEnumerable<ILogItem>> ItemRangeAdded;

        /// <summary>
        /// get a new log item buffer by cloning this
        /// <para>event handlers and buffer state is not cloned</para>
        /// </summary>
        /// <returns>cloned log item buffer</returns>
        ILogItemBuffer Clone();
    }
}
