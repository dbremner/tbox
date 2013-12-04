using System;
using System.Collections.Generic;
using Common.UI.ModelsContainers;
using ParallelNUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Infrastructure.Interfaces;
using ParallelNUnit.Infrastructure.Runners;

namespace ParallelNUnit.Infrastructure.Packages
{
    public sealed class ThreadPackage : BasePackage
    {
        private readonly ResolveEventHandler loadFromSameFolder;

        public ThreadPackage(string path, string dirToCloneTests, string commandToExecuteBeforeTests, IUnitTestsView view, ResolveEventHandler loadFromSameFolder)
            :base(path, dirToCloneTests, commandToExecuteBeforeTests, view)
        {
            this.loadFromSameFolder = loadFromSameFolder;
        }

        public override void DoRefresh(Action<IPackage> onReceive, Action<IPackage> onError)
        {
            try
            {
                var cl = new NUnitExecutor(loadFromSameFolder);
                var results = cl.CollectTests(FilePath);
                if(results == null)
                    throw new ArgumentException("Can't collect tests in: " + FilePath);
                Items = new[]{results};
                onReceive(this);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't refresh tests from dll: " + FilePath);
                onError(this);
            }
        }

        public override void DoRun(Action<IPackage> onReceive, IList<Result> allTests, IList<IList<Result>> packages, bool copyToSeparateFolders, string[] copyMasks, bool needSynchronizationForTests, int startDelay, Synchronizer synchronizer, IProgressStatus u, bool needOutput)
        {
            try
            {
                var runner = new ThreadTestsRunner(loadFromSameFolder);
                runner.Run(FilePath, allTests, packages, Server, copyToSeparateFolders, copyMasks, needSynchronizationForTests, DirToCloneTests, CommandToExecuteBeforeTests, startDelay, synchronizer, u, needOutput);
                onReceive(this);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't run test, from dll: " + FilePath);
            }
        }
    }
}
