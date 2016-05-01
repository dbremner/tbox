using Mnk.Library.Common.MT;

namespace Mnk.Rat.Code
{
    class SearchContext
    {
        public IUpdater Updater { get; set; }
        public SearchConfig SearchConfig { get; set; }
        public IDataProvider DataProvider { get; set; }
    }
}
