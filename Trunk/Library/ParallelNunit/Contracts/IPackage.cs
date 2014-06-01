using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IPackage<out TConfig>: IDisposable 
        where TConfig: ITestsConfig
    {
        TConfig Config { get; }
        ITestsMetricsCalculator Tmc { get; }
        IList<Result> Items { get; set; }
        event Action<IPackage<TConfig>> RefreshSuccessEvent;
        event Action<IPackage<TConfig>> RefreshErrorEvent;
        event Action<IPackage<TConfig>> TestsFinishedEvent;

        bool EnsurePathIsValid();
        void Refresh();
        void Run(IList<Result> checkedTests = null);
        IList<IList<Result>> DivideTests(IList<Result> checkedTests = null);
    }
}
