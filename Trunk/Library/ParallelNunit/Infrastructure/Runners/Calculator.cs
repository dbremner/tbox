namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    class Calculator
    {
        private readonly AgentProcessCreator processCreator;
        public Calculator(AgentProcessCreator processCreator)
        {
            this.processCreator = processCreator;
        }

        public void CollectTests(string path, string handle)
        {
            IContext p = null;
            try
            {
                p = processCreator.Create(path, handle, TestsCommands.Collect);
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
