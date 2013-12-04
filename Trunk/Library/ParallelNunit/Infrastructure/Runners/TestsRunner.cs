using System.Collections.Generic;
using System.IO;
using Common.Base.Log;
using Common.Communications.Interprocess;
using Common.Console;
using ParallelNUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Infrastructure.Communication;
using ParallelNUnit.Infrastructure.Interfaces;
using ParallelNUnit.Interfaces;

namespace ParallelNUnit.Infrastructure.Runners
{
    abstract class TestsRunner
    {
        private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();
        private readonly DirectoriesManipulator dirMan = new DirectoriesManipulator();

        public void Run(string path, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, bool copyToLocalFolders, string[] copyMasks, bool needSynchronizationForTests, string dirToCloneTests, string commandToExecuteBeforeTests, int startDelay, Synchronizer synchronizer, IProgressStatus u, bool needOutput)
        {
            var dllPaths = dirMan.GenerateFolders(path, packages, copyToLocalFolders, copyMasks, dirToCloneTests, u);
            var s = (NunitRunnerClient)server.Owner;
            try
            {
                if (u.UserPressClose) return;
                var handle = server.Handle;
                if (!string.IsNullOrEmpty(commandToExecuteBeforeTests))
                {
                    foreach (var folder in dllPaths)
                    {
                        Cmd.Start(commandToExecuteBeforeTests, Log,
                            directory: Path.GetDirectoryName(folder),
                            waitEnd: true,
                            nowindow: true);
                    }
                }
                if (u.UserPressClose) return;
                s.PrepareToRun(synchronizer, u,
                    new TestRunConfig
                        {
                            StartDelay = startDelay * 1000,
                            DllPathes = dllPaths
                        },
                    packages, allTests,
                    Run(needSynchronizationForTests, handle, needOutput)
                    );
            }
            finally
            {
                s.TestsContext.WaitForExit();
                s.TestsContext.Dispose();
                dirMan.ClearFolders(dllPaths, copyToLocalFolders, copyMasks);
            }
        }

        protected abstract IContext Run(bool needSynchronizationForTests, string handle, bool needOutput);
    }
}
