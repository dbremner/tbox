using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    abstract class TestsRunner<TConfig>
        where TConfig : ITestsConfig
    {
        private readonly IDirectoriesManipulator directoriesManipulator;
        private readonly ILog log = LogManager.GetLogger<TestsRunner<TConfig>>();

        protected TestsRunner(IDirectoriesManipulator directoriesManipulator)
        {
            this.directoriesManipulator = directoriesManipulator;
        }

        public TestsResults Run(TConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, ITestsUpdater updater)
        {
            var dllPaths = directoriesManipulator.GenerateFolders(config, updater, packages.Count);
            var s = (NunitRunnerClient)server.Owner;
            try
            {
                if (updater.UserPressClose) return new TestsResults();
                var handle = server.Handle;
                if (!string.IsNullOrEmpty(config.CommandBeforeTestsRun))
                {
                    foreach (var folder in dllPaths)
                    {
                        Cmd.Start(config.CommandBeforeTestsRun, log,
                            directory: Path.GetDirectoryName(folder),
                            waitEnd: true,
                            noWindow: true);
                        if (updater.UserPressClose) return new TestsResults();
                    }
                }
                if (updater.UserPressClose) return new TestsResults();
                s.PrepareToRun(new Synchronizer(), updater,
                    new TestRunConfig(dllPaths, config)
                        {
                            StartDelay = config.StartDelay * 1000,
                        },
                    packages, allTests,
                    DoRun(config, handle),
                    metrics
                    );
            }
            finally
            {
                s.TestsRunnerContext.WaitForExit();
                s.TestsRunnerContext.Dispose();
                directoriesManipulator.ClearFolders(config, dllPaths);
            }
            return new TestsResults(allTests);
        }

        protected abstract IRunnerContext DoRun(TConfig config, string handle);
    }
}
