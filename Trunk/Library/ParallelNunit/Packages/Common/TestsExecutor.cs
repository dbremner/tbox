using System;
using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    class TestsExecutor : ITestsExecutor
    {
        private readonly IDirectoriesManipulator directoriesManipulator;
        private readonly Func<string, ITestsExecutionFacade> factory;
        protected readonly ILog Log = LogManager.GetLogger<TestsExecutor>();

        public TestsExecutor(IDirectoriesManipulator directoriesManipulator, Func<string, ITestsExecutionFacade> factory)
        {
            this.directoriesManipulator = directoriesManipulator;
            this.factory = factory;
        }

        public TestsResults CollectTests(ITestsConfig config, InterprocessServer<INunitRunnerClient> server)
        {
            return GetFacade(config).CollectTests(config, server);
        }

        public TestsResults Run(ITestsConfig config, ITestsMetricsCalculator metrics, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, ITestsUpdater updater)
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
                        Cmd.Start(config.CommandBeforeTestsRun, Log,
                            directory: Path.GetDirectoryName(folder),
                            waitEnd: true,
                            noWindow: true);
                        if (updater.UserPressClose) return new TestsResults();
                    }
                }
                if (updater.UserPressClose) return new TestsResults();
                s.PrepareToRun(new Synchronizer(), updater,
                    new TestRunConfig(dllPaths)
                        {
                            StartDelay = config.StartDelay * 1000,
                        },
                    packages, allTests,
                    metrics, config
                    );
                GetFacade(config).Run(config, handle);
            }
            finally
            {
                directoriesManipulator.ClearFolders(config, dllPaths);
            }
            return new TestsResults(allTests);
        }

        private ITestsExecutionFacade GetFacade(ITestsConfig config)
        {
            return factory(config.Mode);
        }

    }
}
