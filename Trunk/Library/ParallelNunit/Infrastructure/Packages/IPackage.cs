using System;
using System.Collections.Generic;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Packages
{
    public interface IPackage: IDisposable 
    {
        string FilePath { get; }
        int Count { get; }
        int FailedCount { get; }

        bool EnsurePathIsValid();
        IList<IList<Result>> PrepareToRun(int processCount, string[] categories, bool? include, bool usePrefetch, IList<Result> checkedTests = null);
        void DoRefresh(Action<IPackage> onReceive, Action<IPackage> onError);
        void DoRun(Action<IPackage> onReceive, IList<Result> allTests, IList<IList<Result>> packages, bool copyToSeparateFolders,
                          string[] copyMasks, bool needSynchronizationForTests, int startDelay,
                          Synchronizer synchronizer, IProgressStatus u, bool needOutput);
        void ApplyResults(bool usePrefetch);
    }
}
