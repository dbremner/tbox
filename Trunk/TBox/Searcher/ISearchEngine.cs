using Mnk.Library.Common.MT;

namespace Mnk.Rat
{
    public interface ISearchEngine
    {
        ISearcher Searcher { get; }
        bool LoadSearchInfo(string folderPath, IUpdater updater);
        bool MakeIndex(string folderPath, IUpdater updater);
        void Unload();
    }
}