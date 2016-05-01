using System;
using System.Collections.Generic;

namespace Mnk.Rat.Finders.Search
{
    sealed class SearchAdder : IWordsGenerator
    {
        public ISet<string> Words { get; private set; }

        public IDictionary<string, HashSet<int>> FileWords
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public SearchAdder()
        {
            Words = new SortedSet<string>();
        }
        public void AddWord(string word, int fileId)
        {
            Words.Add(word.Trim());
        }

        public void Save(string fileDir)
        {
            throw new NotImplementedException();
        }
    }
}
