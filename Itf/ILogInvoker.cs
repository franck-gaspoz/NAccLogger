namespace NAccLogger.Itf
{
    public interface ILogInvoker
    {
        LogType LogType { get; set; }
        LogCategory LogCategory { get; set; }
        ILog Log { get; set; }
        string CallerMemberName { get; set; }
        int CallerLineNumber { get; set; }
        string CallerFilePath { get; set; }
        object Caller { get; set; }

        void T(string text);
    }
}