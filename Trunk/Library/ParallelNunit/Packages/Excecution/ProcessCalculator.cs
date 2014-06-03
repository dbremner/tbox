using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    class ProcessCalculator : IProcessCalculator
    {
        private readonly IProcessCreator processCreator;
        public ProcessCalculator(IProcessCreator processCreator)
        {
            this.processCreator = processCreator;
        }

        public void CollectTests(IProcessTestConfig config, string path, string handle)
        {
            IRunnerContext p = null;
            try
            {
                p = processCreator.Create(config, handle, TestsCommands.Collect);
            }
            finally
            {
                if (p != null)
                {
                    p.WaitForExit();
                    p.Dispose();
                }
            }
        }
    }
}
