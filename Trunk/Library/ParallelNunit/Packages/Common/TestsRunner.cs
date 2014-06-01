using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    abstract class TestsRunner
    {
        private readonly IDirectoriesManipulator directoriesManipulator;
        protected readonly ITestsConfig Config;
        private readonly ITestsUpdater updater;
        private readonly ISynchronizer synchronizer;
        private readonly ITestsMetricsCalculator metricsCalculator;
        private readonly ILog log = LogManager.GetLogger<TestsRunner>();

        protected TestsRunner(IDirectoriesManipulator directoriesManipulator, ITestsConfig config, ITestsUpdater updater, ISynchronizer synchronizer, ITestsMetricsCalculator metricsCalculator)
        {
            this.directoriesManipulator = directoriesManipulator;
            this.Config = config;
            this.updater = updater;
            this.synchronizer = synchronizer;
            this.metricsCalculator = metricsCalculator;
        }

        public void Run(IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server)
        {
            var dllPaths = directoriesManipulator.GenerateFolders(packages.Count);
            var s = (NunitRunnerClient)server.Owner;
            try
            {
                if (updater.UserPressClose) return;
                var handle = server.Handle;
                if (!string.IsNullOrEmpty(Config.CommandBeforeTestsRun))
                {
                    foreach (var folder in dllPaths)
                    {
                        Cmd.Start(Config.CommandBeforeTestsRun, log,
                            directory: Path.GetDirectoryName(folder),
                            waitEnd: true,
                            noWindow: true);
                    }
                }
                if (updater.UserPressClose) return;
                s.PrepareToRun(synchronizer, updater,
                    new TestRunConfig(dllPaths)
                        {
                            StartDelay = Config.StartDelay * 1000,
                        },
                    packages, allTests,
                    Run(handle),
                    metricsCalculator
                    );
            }
            finally
            {
                s.TestsRunnerContext.WaitForExit();
                s.TestsRunnerContext.Dispose();
                directoriesManipulator.ClearFolders(dllPaths);
            }
        }

        protected abstract IRunnerContext Run(string handle);
    }
}
