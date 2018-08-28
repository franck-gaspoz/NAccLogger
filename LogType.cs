namespace NAccLogger
{
    /// <summary>
    /// log levels
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// was not defined
        /// </summary>
        NotDefined,

        /// <summary>
        /// info level : relevant user message
        /// </summary>
        Info,

        /// <summary>
        /// debug level : developing and test purposes
        /// </summary>
        Debug,

        /// <summary>
        /// warning level : relevant to warm user or developer
        /// </summary>
        Warning,

        /// <summary>
        /// error level : application error
        /// </summary>
        Error,

        /// <summary>
        /// fatal level : inform of program failure and probably application end
        /// </summary>
        Fatal,

        /// <summary>
        /// to define filters only
        /// </summary>
        All
    }
}