using System.Diagnostics;

namespace ParallelNUnit.Infrastructure.Runners
{
    public sealed class ProcessContext : IContext
    {
        private readonly Process p;
        public ProcessContext(Process p)
        {
            this.p = p;
        }

        public void Dispose()
        {
            p.Dispose();
        }

        public void WaitForExit()
        {
            p.WaitForExit();
        }

        public void Kill()
        {
            p.Kill();
        }
    }
}
