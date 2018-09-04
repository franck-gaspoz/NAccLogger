namespace NAccLogger.Loggers.File
{
    /// <summary>
    /// file log parameters
    /// </summary>
    public class FileLogParameters
    {
        /// <summary>
        /// path to a folder containing the log files
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// path to a folder containing archived log files
        /// </summary>
        public string ArchivePath { get; set; }

        /// <summary>
        /// log archived filename pattern
        /// <para>charachters ( and ) are reserved to add a number for identifying archived logs having the same name</para>
        /// <para>use variables to identify files versions when log rotating is enabled</para>
        /// <para>available variables are:</para>
        /// <para>{yyyy} : 4 digits year. value is file last write year</para>
        /// <para>{mm} : 2 digits month. value is file last write month</para>
        /// <para>{dd} : 2 digits day. value is file last write day</para>
        /// </summary>
        public string ArchiveFileName { get; set; } = "log_{mm}-{dd}-{yyyy}.txt";

        /// <summary>
        /// current fie log pattern
        /// </summary>
        public string FileName { get; set; } = "log.txt";

        /// <summary>
        /// indicates when a file log archivage action is fired
        /// </summary>
        public FileLogArchiveEvent FileLogArchiveEvent { get; set; }
            = FileLogArchiveEvent.DayChanged;

        /// <summary>
        /// max log size before log file rotation or reset (default 1mo)
        /// </summary>
        public long FileMaxSize { get; set; } = 1024 * 1024 * 1024;

        /// <summary>
        /// if true, logs are added to file by ranges, else individually (default false)
        /// </summary>
        public bool IsDefered { get; set; } = false;

        /// <summary>
        /// size of defered range of log items
        /// </summary>
        public int DeferedRangeSize { get; set; } = 100;

    }
}
