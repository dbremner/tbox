using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsFixture: IDisposable 
    {
        bool EnsurePathIsValid(ITestsConfig config);
        TestsResults Refresh(ITestsConfig config);
        TestsResults Run(ITestsConfig config, TestsResults tests, ITestsUpdater updater, IList<Result> checkedTests = null);
    }
}
