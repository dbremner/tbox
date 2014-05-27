using System.Collections.Generic;
using System.IO;
using Mnk.Library.Common;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Execution;
using Mnk.Library.ParallelNUnit.Infrastructure.Communication;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Interfaces;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    abstract class TestsRunner
    {
        private static readonly ILog Log = LogManager.GetLogger<TestsRunner>();
        private readonly DirectoriesManipulator dirMan = new DirectoriesManipulator();

        public void Run(string path, IList<Result> allTests, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, bool copyToLocalFolders, string[] copyMasks, bool needSynchronizationForTests, string dirToCloneTests, string commandToExecuteBeforeTests, int startDelay, Synchronizer synchronizer, IProgressStatus u, bool needOutput, string runtimeFramework)
        {
            var dllPaths = dirMan.GenerateFolders(path, packages.Count, copyToLocalFolders, copyMasks, dirToCloneTests, u);
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
                            noWindow: true);
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
                    Run(path, needSynchronizationForTests, handle, needOutput, runtimeFramework)
                    );
            }
            finally
            {
                s.TestsContext.WaitForExit();
                s.TestsContext.Dispose();
                dirMan.ClearFolders(dllPaths, copyToLocalFolders, copyMasks);
            }
        }

        protected abstract IContext Run(string path, bool needSynchronizationForTests, string handle, bool needOutput, string runtimeFramework);
    }
}
