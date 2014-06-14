using System.Diagnostics;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    public sealed class ProcessRunnerContext 
    {
        private readonly Process process;
        public ProcessRunnerContext(Process process)
        {
            this.process = process;
        }

        public void Dispose()
        {
            process.Dispose();
        }

        public void WaitForExit()
        {
            process.WaitForExit();
        }

        public void Kill()
        {
            process.Kill();
        }
    }
}
