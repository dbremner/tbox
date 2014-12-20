using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.PluginsShared.LongPaths;
using Mnk.TBox.Locales.Localization.Plugins.Searcher;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Parsers;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Search;
using Mnk.TBox.Plugins.Searcher.Code.Settings;
using ZetaLongPaths;

namespace Mnk.TBox.Plugins.Searcher.Code.Finders.Scanner
{
    sealed class Scanner
    {
        private static readonly ILog Log = LogManager.GetLogger<Scanner>();
        internal readonly UnicList Dirs = new UnicList();
        private readonly IParser parser;
        private IUpdater updater;
        internal readonly Dictionary<string, FilesList> Files = new Dictionary<string, FilesList>();
        public const int MaxFilesPerType = 10000000;
        private int dirsCount;
        private int currDirNo;
        private long readedSize;
        private Filter filter;
        private int calcDirsTime;

        internal Scanner(IParser parser)
        {
            this.parser = parser;
        }

        public void ScanDirectory(IList<string> targetDirectories, IndexSettings index, IUpdater upd)
        {
            calcDirsTime = Environment.TickCount;
            updater = upd;
            updater.Update(SearcherLang.CalcDirsCount, -1);
            filter = new Filter(index.FileMasksToExclude.Select(x => x.Key));
            dirsCount = targetDirectories
                .Sum(x => GetSubdirsCount(new ZlpDirectoryInfo(x)));
            currDirNo = 0;
            readedSize = 0;
            foreach (var s in index.FileTypes)
            {
                Files.Add(s.Key, new FilesList());
            }
            calcDirsTime = (Environment.TickCount - calcDirsTime) / 1000;
            Parallel.ForEach(Scan(targetDirectories), Parse);
            updater.Update(dirsCount);
        }

        private int GetSubdirsCount(ZlpDirectoryInfo info)
        {
            return 1 + info.SafeEnumerateDirectories(Log)
                .Where(x => filter.CheckAttribute(x.Attributes))
                .Sum(dir => GetSubdirsCount(dir));
        }

        private void Parse(AddInfo info)
        {
            try
            {
                parser.Parse(info);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't parse files");
            }
        }

        private IEnumerable<AddInfo> Scan(IEnumerable<string> targetDirectories)
        {
            return targetDirectories
                .SelectMany(s => ScanDirectory(s, new List<int>(), s));
        }

        private IEnumerable<AddInfo> ScanDirectory(string dirName, List<int> path, string fullDirName)
        {
            if (fullDirName.Length > 248)
            {
                Log.Write("Dir path too long: '" + fullDirName + "'");
                yield break;
            }
            if (updater.UserPressClose)
                yield break;
            ++currDirNo;
            updater.Update(
                    time => string.Format(SearcherLang.ProgressString,
                        currDirNo, time.FormatTimeInSec(), (time - calcDirsTime <= 0) ? 0 : (readedSize / (time - calcDirsTime) / 1024)),
                    currDirNo, dirsCount);

            path.Add(Dirs.Add(dirName));
            var dirInfo = new ZlpDirectoryInfo(fullDirName);
            var count = 0;
            foreach (var data in Files)
            {
                foreach (var file in dirInfo.SafeEnumerateFiles(Log, "*." + data.Key)
                    .Where(x => filter.CanInclude(x)))
                {
                    var info = new AddInfo
                    {
                        Path = file.FullName,
                        Id = count + data.Value.Count
                    };
                    data.Value.Add(ZlpPathHelper.GetFileNameWithoutExtension(file.Name), path);
                    readedSize += file.Length;
                    yield return info;
                }
                count += MaxFilesPerType;
            }
            foreach (var dir in dirInfo.SafeEnumerateDirectories(Log)
                .Where(x => filter.CheckAttribute(x.Attributes)))
            {
                var tmp = new List<int>();
                tmp.AddRange(path);
                foreach (var info in ScanDirectory(dir.Name, tmp, ZlpPathHelper.Combine(fullDirName, dir.Name)))
                {
                    yield return info;
                }
            }
        }

        public void Save(string path)
        {
            var dirName = path;
            if (!Dirs.Save(ZlpPathHelper.Combine(dirName, Folders.DirPath))) return;
            dirName = ZlpPathHelper.Combine(dirName, Folders.FilesPath);
            if (!ZlpIOHelper.DirectoryExists(dirName)) ZlpIOHelper.CreateDirectory(dirName);
            Files.ForEach(data => data.Value.Save(ZlpPathHelper.Combine(dirName, data.Key)));
        }
    }
}
