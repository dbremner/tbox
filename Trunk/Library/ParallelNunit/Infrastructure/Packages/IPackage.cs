using System;
using System.Collections.Generic;
using ParallelNUnit.Core;
using ParallelNUnit.Infrastructure.Interfaces;

namespace ParallelNUnit.Infrastructure.Packages
{
    public interface IPackage 
    {
        string FilePath { get; }
        string[] Output { get; }
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
