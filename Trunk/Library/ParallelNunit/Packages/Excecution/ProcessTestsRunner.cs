using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ProcessTestsRunner : TestsRunner, IProcessTestsRunner
    {
        private readonly IProcessCreator processCreator;
        public ProcessTestsRunner(IProcessCreator processCreator, IDirectoriesManipulator directoriesManipulator, ITestsConfig config, ITestsUpdater updater, ISynchronizer synchronizer, ITestsMetricsCalculator calculator)
            : base(directoriesManipulator, config, updater, synchronizer, calculator)
        {
            this.processCreator = processCreator;
        }

        protected override IRunnerContext DoRun(string handle)
        {
            return processCreator.Create(handle, Config.NeedSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest);
        }

    }
}
