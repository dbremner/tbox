using System.Threading.Tasks;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class MultiProcessTestsExecutionFacade : ProcessTestsExecutionFacade
    {
        public override void Run(ITestsConfig config, string handle)
        {
            Parallel.For(0, config.ProcessCount, i => 
                Execute(() =>
                    Create(config, handle,
                        config.NeedSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest)));
        }
    }
}
