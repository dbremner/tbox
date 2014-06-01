using System.Collections.Generic;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    internal interface IDirectoriesManipulator
    {
        IList<string> GenerateFolders(int count);
        void ClearFolders(IList<string> dllPaths);
    }
}
