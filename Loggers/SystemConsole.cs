using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAccLogger.Loggers
{
    public class SystemConsole
        : LogBase
    {
        public override void Add(LogItem logItem)
        {
            System.Console.WriteLine(
                LogItemToString(logItem)
                );
        }
    }
}
