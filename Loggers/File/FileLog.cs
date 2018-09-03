using NAccLogger.Impl;
using NAccLogger.Itf;
using System;
using System.Collections.Generic;
using System.IO;

namespace NAccLogger.Loggers.File
{
    /// <summary>
    /// file log
    /// </summary>
    public class FileLog
        : LogBase
    {
        /// <summary>
        /// file log parameters
        /// </summary>
        public FileLogParameters FileLogParameters { get; protected set; }

        /// <summary>
        /// synchro lock
        /// </summary>
        protected readonly object Lock = new object();

        /// <summary>
        /// build a new file logger
        /// </summary>
        /// <param name="fileLogParameters">file log parameters</param>
        /// <param name="logParameters">log parameters</param>
        public FileLog(
            FileLogParameters fileLogParameters,
            LogParameters logParameters = null
            ) : base(logParameters)
        {
            FileLogParameters = fileLogParameters;
            if (FileLogParameters.IsDefered)
                LogParameters
                    .LogItemBuffer
                    .ItemRangeAdded
                    += LogItemBuffer_ItemRangeAdded;
        }

        /// <summary>
        /// a new range of log items must been added to the log
        /// </summary>
        /// <param name="sender">sender (log item buffer)</param>
        /// <param name="lst">event args containing new log items</param>
        private void LogItemBuffer_ItemRangeAdded(
            object sender, 
            IEnumerable<ILogItem> lst)
        {
            lock (Lock)
            {
                var fi =
                    GetCurentFileLogInfo();

                CheckArchiveFileLog(fi);

                foreach (var o in lst)
                    AppendLogItemToFile(o,fi);
            }
        }

        /// <summary>
        /// add a log entry to the system console
        /// </summary>
        /// <param name="logItem"></param>
        public override void Log(ILogItem logItem) {
            if (!FileLogParameters.IsDefered)
            {
                lock (Lock)
                {
                    var fi = GetCurentFileLogInfo();

                    CheckArchiveFileLog(fi);

                    AppendLogItemToFile(
                        logItem,
                        fi);
                }
            }
        }

        protected FileInfo GetCurentFileLogInfo()
        {
            var filePath =
                Path.
                Combine(
                    FileLogParameters.Path,
                    FileLogParameters.FileName);
            return new FileInfo(filePath);
        }

        /// <summary>
        /// append a log item to file log
        /// <para>Beware: No interprocess lock</para>
        /// </summary>
        /// <param name="logItem">log item</param>
        /// <param name="fileInfo">current log file info</param>
        protected void AppendLogItemToFile(
            ILogItem logItem,
            FileInfo fileInfo)
        {                                    
            using (StreamWriter sw = 
                global::System.IO.File.AppendText(
                    fileInfo.FullName
                    ))
            {
                sw.WriteLine(logItem.LogEntryText);
                sw.Flush();
            }
        }

        protected void CheckArchiveFileLog(
            FileInfo beforeWriteItemFileInfo
            )
        {
            switch (FileLogParameters.FileLogArchiveEvent)
            {
                case FileLogArchiveEvent.DayChanged:
                    if (beforeWriteItemFileInfo.Exists)
                    {
                        var dec = DateTime.Now
                            - beforeWriteItemFileInfo.LastWriteTime;
                        if (dec.Days >= 1)
                            ArchiveFileLog(beforeWriteItemFileInfo);
                    }                       
                    break;

                case FileLogArchiveEvent.MaxSizeReached:
                    if (beforeWriteItemFileInfo.Exists
                        && beforeWriteItemFileInfo.Length >= FileLogParameters.FileMaxSize)
                        ArchiveFileLog(beforeWriteItemFileInfo);
                    break;
            }
        }

        protected string GetArchiveFileNameCopy(string fileName)
        {
            var t = fileName.Split('(');
            if (t.Length == 1)
            {
                // ( missing
                fileName =
                    Path.GetFileNameWithoutExtension(fileName)
                    + " (2)"
                    + Path.GetExtension(fileName);
            }
            else
            {
                var snum = t[1].Substring(0, t[1].IndexOf(')'));
                var n = Convert.ToInt32(snum) + 1;
                fileName =
                    t[0]
                    + $"({n})"
                    + Path.GetExtension(fileName);
            }
            return fileName;
        }

        protected void ArchiveFileLog(FileInfo fileInfo)
        {
            var archiveFileName = GetArchiveFileName(fileInfo);
            while (System.IO.File.Exists(archiveFileName))
                archiveFileName = GetArchiveFileNameCopy(archiveFileName);

            System.IO.File.Copy(
                fileInfo.FullName,
                archiveFileName
                );
            System.IO.File.Delete(fileInfo.FullName);
        }

        protected string GetArchiveFileName(FileInfo fileInfo)
        {
            var fn = FileLogParameters
                .ArchiveFileName;
            var pt = FileLogParameters
                .ArchivePath;
            fn = AssignVariables(fileInfo,fn);
            return fn;
        }

        protected string AssignVariables(
            FileInfo fileInfo,
            string templatedString)
        {
            templatedString = templatedString.Replace(
                "{mm}", fileInfo.LastWriteTime.Month.ToString().PadLeft(2, '0'));
            templatedString = templatedString.Replace(
                "{dd}", fileInfo.LastWriteTime.Day.ToString().PadLeft(2, '0'));
            templatedString = templatedString.Replace(
                "{yyyy}", fileInfo.LastWriteTime.Year.ToString().PadLeft(4, '0'));

            return templatedString;
        }
    }
}
