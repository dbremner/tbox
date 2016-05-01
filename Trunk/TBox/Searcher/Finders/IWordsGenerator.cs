using System.Collections.Generic;

namespace Mnk.Rat.Finders
{
    interface IWordsGenerator 
    {
        IDictionary<string, HashSet<int>> FileWords { get; }
        void Save(string fileDir);
        void AddWord(string word, int fileId);
    }
}