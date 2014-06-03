using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IPackage<in TConfig>: IDisposable 
        where TConfig: ITestsConfig
    {
        bool EnsurePathIsValid(TConfig config);
        TestsResults Refresh(TConfig config);
        TestsResults Run(TConfig config, TestsResults tests, ITestsUpdater updater, IList<Result> checkedTests = null);
        IList<IList<Result>> DivideTests(TConfig config, ITestsMetricsCalculator metrics, IList<Result> checkedTests = null);
    }
}
