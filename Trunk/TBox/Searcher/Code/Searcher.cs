using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.Rat.Checkers;
using Mnk.Rat.Finders.Parsers;
using Mnk.Rat.Finders.Search;
using Mnk.Rat.Search;
using Mnk.Library.Common.MT;

namespace Mnk.Rat.Code
{
    class Searcher : ISearcher
    {
        private readonly IIndexContextBuilder indexContextBuilder;
        private readonly IFileInformer fileInformer;
        private readonly IWordsFinder wordsFinder;
        private readonly IDataProvider dataProvider;
        private readonly IParser parser;

        public Searcher(IIndexContextBuilder indexContextBuilder, IFileInformer fileInformer, IWordsFinder wordsFinder, IDataProvider dataProvider, IParser parser)
        {
            this.indexContextBuilder = indexContextBuilder;
            this.fileInformer = fileInformer;
            this.wordsFinder = wordsFinder;
            this.dataProvider = dataProvider;
            this.parser = parser;
        }

        public SearchResult Search(string searchText, SearchConfig config, IUpdater u)
        {
            searchText = searchText.Trim();
            var context = new SearchContext { SearchConfig = config, Updater = u, DataProvider = dataProvider };
            var result = new SearchResult();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchAdder = new SearchAdder();
                if (context.SearchConfig.SearchMode == SearchMode.FileNames)
                {
                    searchAdder.Words.Add(searchText);
                }
                else
                {
                    parser.ParseFileData(searchAdder, searchText, 0);
                }
                if (!searchAdder.Words.Any())
                {
                    result.State = SearchState.TextDontContainSearchableSymbols;
                    return result;
                }
                if (searchAdder.Words.Count < 2 && (!context.SearchConfig.FullTextSearch || context.SearchConfig.SearchMode == SearchMode.FileNames))
                {
                    Search(searchAdder.Words.FirstOrDefault(), context, result.Files, context.SearchConfig.FileCount);
                }
                else
                {
                    var firstWord = true;
                    foreach (var word in searchAdder.Words)
                    {
                        var tmp = new HashSet<int>();
                        Search(word, context, tmp, int.MaxValue);
                        if (firstWord)
                        {
                            result.Files = tmp;
                            firstWord = false;
                        }
                        else
                        {
                            ((HashSet<int>)result.Files).IntersectWith(tmp);
                        }
                    }
                    var items = (IEnumerable<int>)result.Files;
                    if (context.SearchConfig.FullTextSearch && context.SearchConfig.SearchMode == SearchMode.FileData)
                    {
                        DoFullTextSearch(searchText, result, context);
                    }
                    else
                    {
                        result.State = SearchState.Done;
                        result.Files = items.Take(context.SearchConfig.FileCount).ToArray();
                    }
                }
            }
            return result;
        }

        public string GetFilePath(int id)
        {
            return fileInformer.GetFilePath(id);
        }

        private void DoFullTextSearch(string searchText, SearchResult result, SearchContext context)
        {
            var count = (float)Math.Min(result.Files.Count, context.SearchConfig.FileCount);
            var i = 0;
            result.Files = result.Files.AsParallel()
                .Where(x => CheckFile(searchText, context, x, count, ref i))
                .Take(context.SearchConfig.FileCount)
                .ToArray();
        }

        private bool CheckFile(string searchText, SearchContext context, int x, float count, ref int i)
        {
            var comparationType = context.SearchConfig.MatchCase ?
                StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            var path = fileInformer.GetFilePath(x);
            if (context.Updater.UserPressClose || !context.DataProvider.Contains(path, searchText, comparationType)) return false;
            context.Updater.Update(++i / count);
            return true;
        }

        private void Search(string searchText, SearchContext context, ICollection<int> files, int fileCount)
        {
            var checker = CreateChecker(searchText, context);
            switch (context.SearchConfig.SearchMode)
            {
                case SearchMode.FileNames:
                    fileInformer.Find(GetTypes(), checker, fileCount, files);
                    break;
                case SearchMode.FileData:
                    wordsFinder.Find(GetTypes(), checker, fileCount, files);
                    break;
                default:
                    throw new ArgumentException("Unknown search mode: " + context.SearchConfig.SearchMode);
            }
        }

        private static IFileChecker CreateChecker(string searchText, SearchContext context)
        {
            return FileCheckerFactory.Create(
                context.SearchConfig.CompareType, context.SearchConfig.MatchCase, searchText);
        }

        private ISet<string> GetTypes()
        {
            return new HashSet<string>(indexContextBuilder.Context.TargetFileTypes);
        }
    }
}