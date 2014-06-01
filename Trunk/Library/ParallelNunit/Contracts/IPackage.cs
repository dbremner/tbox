using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IPackage<out TConfig>: IDisposable 
        where TConfig: ITestsConfig
    {
        TConfig Config { get; }
        ITestsMetricsCalculator Metrics { get; }
        IList<Result> Items { get; set; }
        event Action<IPackage<TConfig>> RefreshSuccessEventHandler;
        event Action<IPackage<TConfig>> RefreshErrorEventHandler;
        event Action<IPackage<TConfig>> TestsFinishedEventHandler;

        bool EnsurePathIsValid();
        void Refresh();
        void Run(IList<Result> checkedTests = null);
        IList<IList<Result>> DivideTests(IList<Result> checkedTests = null);
    }
}
