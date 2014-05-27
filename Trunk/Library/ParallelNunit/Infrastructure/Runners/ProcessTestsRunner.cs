using System;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    class ProcessTestsRunner : TestsRunner
    {
        private readonly AgentProcessCreator processCreator;
        public ProcessTestsRunner(AgentProcessCreator processCreator)
        {
            this.processCreator = processCreator;
        }

        protected override IContext Run(string path, bool needSynchronizationForTests, string handle, bool needOutput, string runtimeFramework)
        {
            return processCreator.Create(path, handle, (needSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest));
        }

    }
}
