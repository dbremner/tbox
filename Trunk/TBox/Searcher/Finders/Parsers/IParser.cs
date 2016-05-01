using Mnk.Rat.Finders.Scanner;

namespace Mnk.Rat.Finders.Parsers
{
    interface IParser
    {
        bool Parse(IWordsGenerator adder, AddInfo info);
        void ParseFileData(IWordsGenerator adder, string data, int fileId);
    }
}
