namespace NAccLogger.Loggers.File
{
    /// <summary>
    /// indicates when a file log rotation is fired
    /// </summary>
    public enum FileLogArchiveEvent
    {
        /// <summary>
        /// max file log size has been reached
        /// </summary>
        MaxSizeReached,

        /// <summary>
        /// day has changed since last log item added to the file
        /// </summary>
        DayChanged,
    }
}
