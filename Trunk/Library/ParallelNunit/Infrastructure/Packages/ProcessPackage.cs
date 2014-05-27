using System;
using System.Collections.Generic;
using System.Linq;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Communication;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Infrastructure.Runners;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Packages
{
    public sealed class ProcessPackage : BasePackage
    {
        private readonly TestsRunner testsRunner;
        private readonly Calculator calculator;

        public CheckableDataCollection<CheckableData> Categories
        {
            get
            {
                return new CheckableDataCollection<CheckableData>(
                    Metrics.Tests
                    .SelectMany(x => x.Categories)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(x => new CheckableData { Key = x })
                    );
            }
        }

        public ProcessPackage(string path, string nunitAgentPath, bool runAsx86, bool runAsAdmin, string dirToCloneTests, string commandToExecuteBeforeTests, IUnitTestsView view, string runAsx86Path, string runtimeFramework)
            :base(path, dirToCloneTests, commandToExecuteBeforeTests, view, runtimeFramework)
        {
            var processCreator = new AgentProcessCreator(nunitAgentPath, runAsx86Path, runAsx86, runAsAdmin, runtimeFramework);
            calculator = new Calculator(processCreator);
            testsRunner = new ProcessTestsRunner(processCreator);
        }

        public override void DoRefresh(Action<IPackage> onReceive, Action<IPackage> onError)
        {
            try
            {
                var client = ((NunitRunnerClient) Server.Owner);
                client.PrepareToCalc();
                calculator.CollectTests(FilePath, Server.Handle);
                Items = client.Collection;
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
                testsRunner.Run(FilePath, allTests, packages, Server, copyToSeparateFolders, copyMasks, needSynchronizationForTests, DirToCloneTests, CommandToExecuteBeforeTests, startDelay, synchronizer, u, needOutput, RuntimeFramework);
                onReceive(this);
            }
            catch (Exception ex)
            {
                Log.Write(ex, "Can't run test, from dll: " + FilePath);
            }
        }
    }
}
