using Mnk.Library.Common.Communications;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsExecutionFacade
    {
        TestsResults CollectTests(ITestsConfig config, InterprocessServer<INunitRunnerClient> server);
        IRunnerContext Run(ITestsConfig config, string handle);
    }
}