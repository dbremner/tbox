using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.Searcher;
using Mnk.TBox.Plugins.Searcher.Code.Finders;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Parsers;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Scanner;
using Mnk.TBox.Plugins.Searcher.Code.Search;
using Mnk.TBox.Plugins.Searcher.Code.Settings;

namespace Mnk.TBox.Plugins.Searcher.Code
{
    sealed class SearchEngine
    {
        private static readonly ILog Log = LogManager.GetLogger<SearchEngine>();
        private static readonly ILog InfoLog = LogManager.GetInfoLogger<SearchEngine>();
        private readonly IndexSettings indexSettings;
        private readonly IPathResolver pathResolver;
        public FileInformer FileInformer { get; private set; }
        public WordsFinder WordsFinder { get; private set; }

        public SearchEngine(IndexSettings indexSettings, IPathResolver pathResolver)
        {
            this.indexSettings = indexSettings;
            this.pathResolver = pathResolver;
            FileInformer = new FileInformer();
            WordsFinder = new WordsFinder(FileInformer);
        }

        public bool MakeIndex(string folderPath, IUpdater updater)
        {
            var notExists = indexSettings.FileNames
                .Where(x => x.IsChecked)
                .Select(x => pathResolver.Resolve(x.Key))
                .Where(x => !Directory.Exists(x))
                .ToArray();
            if (notExists.Any())
            {
                Log.Write("Can't find folders: " + Environment.NewLine + string.Join(Environment.NewLine, notExists));
                return false;
            }

            var time = Environment.TickCount;
            var targetDirectories =
                indexSettings.FileNames.CheckedItems.Select(x => pathResolver.Resolve(x.Key)).ToArray();
            Unload();
            var wordsGenerator = new WordsGenerator();
            var parser = new Parser(wordsGenerator, indexSettings);
            var scanner = new Scanner(parser);

            scanner.ScanDirectory(targetDirectories, indexSettings, updater);
            Exception ex = null;
            ClearFolder(folderPath);
            Parallel.Invoke(
                () => SafeRun(() => scanner.Save(folderPath), ref ex),
                () => SafeRun(() => wordsGenerator.Save(folderPath), ref ex),
                () => SafeRun(() => FileInformer.Load(scanner), ref ex),
                () => SafeRun(() => WordsFinder.Load(wordsGenerator), ref ex)
                );
            InfoLog.Write("Rebuild indexes time: {0}", Environment.TickCount - time);
            if (ex != null)
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
            updater.Update(SearcherLang.Starting_CreatingEngine, ++curr / count);
            updater.Update(SearcherLang.Starting_LoadingFileNames, ++curr / count);
            Exception ex = null;
            Parallel.Invoke(
                () => SafeRun(() => FileInformer.Load(folderPath, indexSettings.FileTypes), ref ex),
                () => SafeRun(() => WordsFinder.Load(folderPath), ref ex)
            );
            InfoLog.Write("Load indexes time: {0}", Environment.TickCount - time);
            if (ex != null)
            {
                Unload();
                Log.Write(ex, "Can't load indexes. Maybe you need to rebuild it.");
                return false;
            }
            updater.Update(SearcherLang.Starting_LoadingFileData, ++curr / count);
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
