using System.IO;
using System.Reflection;

namespace NAccLogger
{
    public static class Files
    {
        /// <summary>
        /// current path from the executing assembly
        /// </summary>
        public static string CurrentPath
        {
            get
            {
                return Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}
