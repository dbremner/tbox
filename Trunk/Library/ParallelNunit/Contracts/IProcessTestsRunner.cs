using System.Collections.Generic;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IProcessTestsRunner
    {
        void Run(IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server);
    }
}