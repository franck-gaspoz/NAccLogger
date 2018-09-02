using NAccLogger.Impl;
using NAccLogger.Itf;

namespace NAccLogger.Loggers.Console
{
    /// <summary>
    /// system ocnsole log
    /// </summary>
    public class SystemConsole
        : LogBase
    {
        protected bool IsColorizationEnabled = false;

        protected ColorSettings ColorSettings = null;

        /// <summary>
        /// build a new system console logger
        /// </summary>
        /// <param name="logParameters">log parameters</param>
        /// <param name="isColorizationEnabled">true if colorization enabled, false otherwize</param>
        /// <param name="colorSettings">if provided allows to specifiy colors for log types</param>
        public SystemConsole(
            LogParameters logParameters = null,
            bool isColorizationEnabled = true,
            ColorSettings colorSettings = null
        ) : base(logParameters) {
            IsColorizationEnabled = isColorizationEnabled;
            ColorSettings = colorSettings ?? new ColorSettings();
        }

        static readonly object _Lock = new object();

        /// <summary>
        /// add a log entry to the system console
        /// </summary>
        /// <param name="logItem">log item to be added</param>
        public override void Log(ILogItem logItem)
        {
            lock (_Lock)
            {
                var oldCol = System.Console.ForegroundColor;
                if (IsColorizationEnabled)
                {
                    switch (logItem.LogType)
                    {
                        case LogType.Error:
                            System.Console.ForegroundColor = ColorSettings.TypeErrorColor;
                            break;
                        case LogType.Debug:
                            System.Console.ForegroundColor = ColorSettings.TypeDebugColor;
                            break;
                        case LogType.Warning:
                            System.Console.ForegroundColor = ColorSettings.TypeWarningColor;
                            break;
                        case LogType.Fatal:
                            System.Console.ForegroundColor = ColorSettings.TypeFatalColor;
                            break;
                        case LogType.Info:
                            System.Console.ForegroundColor = ColorSettings.TypeInfoColor;
                            break;
                    }
                }
                System.Console.WriteLine(logItem.LogEntryText);
                if (IsColorizationEnabled)
                {
                    System.Console.ForegroundColor = oldCol;
                }
            }
        }
    }
}
