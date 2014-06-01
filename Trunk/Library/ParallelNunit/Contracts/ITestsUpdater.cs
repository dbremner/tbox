using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsUpdater
    {
        void Update(int allCount, Result[] items, int failed, ISynchronizer synchronizer);
        void Update(string text);
        bool UserPressClose { get; }
    }
}
