using Mnk.Library.Common.MT;

namespace Mnk.Rat.Finders.Scanner
{
    interface IScanner
    {
        void Save(string path);
        void ScanDirectory(IWordsGenerator adder, IUpdater upd);
    }
}