using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NAccLogger.Loggers
{
    /// <summary>
    /// system diagnostics
    /// </summary>
    public class SystemDiagnostics
        : LogBase
    {
        public override void Add(LogItem logItem)
        {
            System.Diagnostics.Debug.WriteLine(
                LogItemToString(logItem)
                );
        }
    }
}
