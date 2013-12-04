using System;

namespace ParallelNUnit.Infrastructure.Runners
{
    class ProcessTestsRunner : TestsRunner
    {
        private readonly AgentProcessCreator processCreator;
        public ProcessTestsRunner(AgentProcessCreator processCreator)
        {
            this.processCreator = processCreator;
        }

        protected override IContext Run(bool needSynchronizationForTests, string handle, bool needOutput)
        {
            return processCreator.Create(string.Empty, handle, (needSynchronizationForTests ? TestsCommands.Test : TestsCommands.FastTest));
        }

    }
}
