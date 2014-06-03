using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ProcessTestsRunner : TestsRunner<IProcessTestConfig>, IProcessTestsRunner
    {
        private readonly IProcessCreator processCreator;
        public ProcessTestsRunner(IProcessCreator processCreator, IDirectoriesManipulator directoriesManipulator)
            : base(directoriesManipulator)
        {
            this.processCreator = processCreator;
        }

        protected override IRunnerContext DoRun(IProcessTestConfig config, string handle)
        {
            return processCreator.Create(config, handle, config.NeedSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest);
        }
    }
}
