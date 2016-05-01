using System.Collections.Generic;

namespace Mnk.Rat
{
    public class SearchResult
    {
        public SearchState State { get; set; }
        public ICollection<int> Files { get; set; }

        public SearchResult()
        {
            Files = new HashSet<int>();
            State = SearchState.NothingToSearch;
        }
    }
}