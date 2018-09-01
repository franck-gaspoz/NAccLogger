namespace NAccLogger.Loggers.Pipe.Winsock.Com
{
    public interface ICommandMessage/*<TP, TR>*/
    {
        Command Command { get; }
        /*TP Parameters { get; set; }
        TR Reply { get; set; }*/

        void SetReply(object reply);

        string ToString();
    }
}