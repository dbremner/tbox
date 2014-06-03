using System.Collections.Generic;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    internal interface IDirectoriesManipulator
    {
        IList<string> GenerateFolders(ITestsConfig config, ITestsUpdater updater, int count);
        void ClearFolders(ITestsConfig config, IList<string> dllPaths);
    }
}
