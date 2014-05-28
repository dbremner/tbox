using System.Diagnostics;

namespace Mnk.Library.ParallelNUnit.Infrastructure.Runners
{
    public sealed class ProcessContext : IContext
    {
        private readonly Process process;
        public ProcessContext(Process process)
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
