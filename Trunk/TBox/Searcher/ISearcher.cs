using Mnk.Library.Common.MT;

namespace Mnk.Rat
{
    public interface ISearcher
    {
        SearchResult Search(string searchText, SearchConfig config, IUpdater u);
        string GetFilePath(int id);
    }
}
