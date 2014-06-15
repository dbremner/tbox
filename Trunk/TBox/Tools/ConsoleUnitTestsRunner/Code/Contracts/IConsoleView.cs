using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    internal interface IConsoleView
    {
        TestsResults TotalResult { get; }
        void SetItems(TestsResults result);
        void PrintTotalResults();
        void GenerateReport(string path, string xmlReport);
    }
}