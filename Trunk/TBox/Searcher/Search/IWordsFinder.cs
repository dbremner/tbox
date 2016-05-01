using System.Collections.Generic;
using Mnk.Rat.Checkers;

namespace Mnk.Rat.Search
{
    interface IWordsFinder
    {
        void Clear();
        bool Find(ISet<string> types, IFileChecker checker, int maxFiles, ICollection<int> list);
        void Load();
        void Load(string fileDir);
    }
}