using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.MT;
using Mnk.Rat.Finders;
using Mnk.Rat.Finders.Parsers;
using Mnk.Rat.Finders.Scanner;
using Mnk.Rat.Search;
using Mnk.TBox.Locales.Localization.Plugins.Searcher;

namespace Mnk.Rat.Code
{
    sealed class SearchEngine : ISearchEngine
    {
        private static readonly ILog Log = LogManager.GetLogger<SearchEngine>();
        private static readonly ILog InfoLog = LogManager.GetInfoLogger<SearchEngine>();
        private readonly IWordsGenerator wordsGenerator;
        private readonly IParser parser;
        private readonly IScanner scanner;
        private readonly IIndexContextBuilder contextBuilder;
        public IFileInformer FileInformer { get; private set; }
        public IWordsFinder WordsFinder { get; private set; }

        public ISearcher Searcher { get; private set; }
        
        public SearchEngine(IFileInformer fileInformer, IWordsFinder wordsFinder, IWordsGenerator wordsGenerator, IParser parser, IScanner scanner, IIndexContextBuilder contextBuilder, ISearcher searcher)
        {
            FileInformer = fileInformer;
            WordsFinder = wordsFinder;
            this.wordsGenerator = wordsGenerator;
            this.parser = parser;
            this.scanner = scanner;
            this.contextBuilder = contextBuilder;
            this.Searcher = searcher;
        }

        /*
        indexSettings.FileNames
                .Where(x => x.IsChecked)
                .Select(x => pathResolver.Resolve(x.Key))
        */
        public bool MakeIndex(string folderPath, IUpdater updater)
        {
            var notExists = contextBuilder.Context.TargetDirectories
                .Where(x => !Directory.Exists(x))
                .ToArray();
            if (notExists.Any())
            {
                Log.Write("Can't find folders: " + Environment.NewLine + string.Join(Environment.NewLine, notExists));
                return false;
            }

            var time = Environment.TickCount;
            Unload();
            scanner.ScanDirectory(wordsGenerator, updater);
            Exception ex = null;
            ClearFolder(folderPath);
            Parallel.Invoke(
                () => SafeRun(() => scanner.Save(folderPath), ref ex),
                () => SafeRun(() => wordsGenerator.Save(folderPath), ref ex),
                () => SafeRun(() => FileInformer.Load(), ref ex),
                () => SafeRun(() => WordsFinder.Load(), ref ex)
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

        /*
        indexSettings.TargetFileTypes
        */
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
                () => SafeRun(() => FileInformer.Load(folderPath), ref ex),
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
