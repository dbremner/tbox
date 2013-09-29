using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Common.Base;
using Common.Base.Log;
using Common.MT;
using Searcher.Code.Finders;
using Searcher.Code.Finders.Parsers;
using Searcher.Code.Finders.Scanner;
using Searcher.Code.Search;
using Searcher.Code.Settings;

namespace Searcher.Code
{
	sealed class SearchEngine
	{
		private static readonly ILog Log = LogManager.GetLogger<SearchEngine>();
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<SearchEngine>();
		private readonly IndexSettings indexSettings;
		public FileInformer FileInformer { get; private set; }
		public WordsFinder WordsFinder { get; private set; }

		public SearchEngine(IndexSettings indexSettings)
		{
			this.indexSettings = indexSettings;
			FileInformer = new FileInformer();
			WordsFinder = new WordsFinder(FileInformer);
		}

		public bool MakeIndex(string folderPath, IUpdater updater)
		{
			var notExists = indexSettings.FileNames.Where(x => x.IsChecked && !Directory.Exists(x.Key)).Select(x=>x.Key).ToArray();
			if (notExists.Any())
			{
				Log.Write("Can't find folders: " + Environment.NewLine + string.Join(Environment.NewLine, notExists));
				return false;
			}

			var time = Environment.TickCount;
			Unload();
			var wordsGenerator = new WordsGenerator();
			var parser = new Parser(wordsGenerator, indexSettings);
			var scanner = new Scanner(parser);

			scanner.ScanDirectory(indexSettings, updater);
			Exception ex = null;
			ClearFolder(folderPath);
			Parallel.Invoke(
				() => SafeRun(() => scanner.Save(folderPath), ref ex),
				() => SafeRun(() => wordsGenerator.Save(folderPath), ref ex),
				() => SafeRun(() => FileInformer.Load(scanner), ref ex),
				() => SafeRun(() => WordsFinder.Load(wordsGenerator), ref ex)
				);
			InfoLog.Write("Rebuild indexes time: {0}", Environment.TickCount - time);
			if (ex!=null)
			{
				Unload();
				ClearFolder(folderPath);
				Log.Write(ex, "Can't save indexes");
				return false;
			}
			return true;
		}

		private static void ClearFolder(string path)
		{
			if (Directory.Exists(path))
			{
				var info = new DirectoryInfo(path);
				info.Delete(true);
			}
			Directory.CreateDirectory(path);
		}

		public void Unload()
		{
			FileInformer.Clear();
			WordsFinder.Clear();
		}

		public bool LoadSearchInfo(string folderPath, IUpdater updater)
		{
			var time = Environment.TickCount;
			float curr = 0;
			const float count = 4;
			if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
			updater.Update(Properties.Resources.Starting_CreatingEngine, ++curr / count);
			updater.Update(Properties.Resources.Starting_LoadingFileNames, ++curr / count);
			Exception ex = null;
			Parallel.Invoke(
				() => SafeRun(() => FileInformer.Load(folderPath, indexSettings.FileTypes), ref ex),
				() => SafeRun(() => WordsFinder.Load(folderPath), ref ex)
			);
			InfoLog.Write("Load indexes time: {0}", Environment.TickCount - time);
			if(ex!=null)
			{
				Unload();
				Log.Write(ex, "Can't load indexes. Maybe you need to rebuild it.");
				return false;
			}
			updater.Update(Properties.Resources.Starting_LoadingFileData, ++curr / count);
			return true;
		}

		private static void SafeRun(Action action, ref Exception ex)
		{
			Exception tmp = null;
			if (!ExceptionsHelper.HandleException(action, e => { tmp = e; })) return;
			if (tmp != null) ex = tmp;
		}
	}
}
