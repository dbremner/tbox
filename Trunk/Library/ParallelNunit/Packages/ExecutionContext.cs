using System;
using LightInject;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Packages
{
   public sealed class ExecutionContext : IDisposable
    {
        public string Path { get; set; }
        public int RetValue { get; set; }
        public ITestsConfig Config { get; set; }
        public TestsResults Results { get; set; }
        public ITestsFixture TestsFixture { get; set; }
        public IServiceContainer Container { get; set; }
        public int StartTime { get; set; }

        public ExecutionContext()
        {
            RetValue = 0;
        }

       public void Dispose()
       {
           if (Container == null) return;
           Container.Dispose();
           Container = null;
       }
    }
}
