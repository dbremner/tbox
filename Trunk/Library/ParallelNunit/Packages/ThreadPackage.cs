using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages
{
    sealed class ThreadPackage : BasePackage<IThreadTestConfig>
    {
        public ThreadPackage(IOrderOptimizationManager orderOptimizationManager, ITestsRunner<IThreadTestConfig> testsRunner)
            : base(orderOptimizationManager, testsRunner)
        {
        }
    }
}
