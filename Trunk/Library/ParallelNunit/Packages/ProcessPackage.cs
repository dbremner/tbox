using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages
{
    sealed class ProcessPackage : BasePackage<IProcessTestConfig>
    {
        public ProcessPackage(IOrderOptimizationManager orderOptimizationManager, ITestsRunner<IProcessTestConfig> testsRunner)
            : base(orderOptimizationManager, testsRunner)
        {
        }
    }
}
