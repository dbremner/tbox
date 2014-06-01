using System.Collections.Generic;
using Mnk.Library.Common.Communications;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IThreadTestsRunner
    {
        void Run(IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server);
    }
}