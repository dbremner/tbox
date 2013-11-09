using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Common.Base.Log;
using Common.MT;
using Common.Tools;
using Localization.Plugins.Searcher;
using Searcher.Code.Finders.Parsers;
using Searcher.Code.Finders.Search;
using Searcher.Code.Settings;

namespace Searcher.Code.Finders.Scanner
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
		private IndexSettings settings;
		private Filter filter;
		private int calcDirsTime;

		internal Scanner(IParser parser)
		{
			this.parser = parser;
		}

		public void ScanDirectory(IndexSettings index, IUpdater upd)
		{
			calcDirsTime = Environment.TickCount;
			updater = upd;
			updater.Update(SearcherLang.CalcDirsCount, -1);
			filter = new Filter(index.FileMasksToExclude.Select(x => x.Key));
			settings = index;
			dirsCount = settings.FileNames.CheckedItems
				.Sum(x => GetSubdirsCount(new DirectoryInfo(x.Key)));
			currDirNo = 0;
			readedSize = 0;
			foreach (var s in settings.FileTypes)
			{
				Files.Add(s.Key, new FilesList());
			}
			calcDirsTime = (Environment.TickCount - calcDirsTime)/1000;
			Parallel.ForEach(Scan(), Parse);
			updater.Update(dirsCount);
		}

		private int GetSubdirsCount(DirectoryInfo info)
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
			catch(Exception ex)
			{
				Log.Write(ex, "Can't parse files");
			}
		}

		private IEnumerable<AddInfo> Scan()
		{
			return settings.FileNames.CheckedItems
				.SelectMany(s => ScanDirectory(s.Key, new List<int>(), s.Key));
		}

		private IEnumerable<AddInfo> ScanDirectory(string dirName, List<int> path, string fullDirName)
		{
			if (fullDirName.Length > 248)
			{
				Log.Write("Dir path too long: '" + fullDirName + "'");
				yield break;
			}
			if (updater.UserPressClose )
				yield break;
			++currDirNo;
			updater.Update(
                    time => string.Format(SearcherLang.ProgressString,
						currDirNo, time, (time - calcDirsTime <= 0) ? 0 : (readedSize / (time - calcDirsTime) / 1024)),
					currDirNo, dirsCount);

			path.Add(Dirs.Add(dirName));
			var dirInfo = new DirectoryInfo(fullDirName);
			var count = 0;
			foreach (var data in Files)
			{
				foreach (var file in dirInfo.SafeEnumerateFiles(Log, "*." + data.Key)
					.Where(x => filter.CanInclude(x)))
				{
					AddInfo info = null;
					try
					{
						 info = new AddInfo
							{
								Path = file.FullName,
								Id = count + data.Value.Count
							};
						data.Value.Add(Path.GetFileNameWithoutExtension(file.Name), path);
						readedSize += file.Length;
					}
					catch (PathTooLongException ex)
					{
						Log.Write(ex, "Can't access file: " + file.Name);
					}
					if(info!=null)yield return info;
				}
				count += MaxFilesPerType;
			}
			foreach (var dir in dirInfo.SafeEnumerateDirectories(Log)
				.Where(x => filter.CheckAttribute(x.Attributes)))
			{
				var tmp = new List<int>();
				tmp.AddRange(path);
				foreach (var info in ScanDirectory(dir.Name, tmp, Path.Combine(fullDirName, dir.Name)))
				{
					yield return info;
				}
			}
		}

		public void Save(string path)
		{
			var dirName = path;
			if (!Dirs.Save(Path.Combine(dirName, Folders.DirPath))) return;
			dirName = Path.Combine(dirName, Folders.FilesPath);
			if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
			Files.ForEach( data => data.Value.Save(Path.Combine(dirName, data.Key)));
		}
	}
}
