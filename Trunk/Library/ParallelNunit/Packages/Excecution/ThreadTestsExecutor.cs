using System;
using System.Threading;
using System.Threading.Tasks;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using ServiceStack.Text;

namespace Mnk.Library.ParallelNUnit.Packages.Excecution
{
    public class ThreadTestsExecutor : IThreadTestsExecutor
    {
        private readonly IThreadTestConfig config;
        private readonly ILog log = LogManager.GetLogger<ThreadTestsExecutor>();

        public ThreadTestsExecutor(IThreadTestConfig config)
        {
            this.config = config;
        }

        public Result CollectTests()
        {
            return new NUnitTestStarter().CollectTests(config.TestDllPath, config.RuntimeFramework);
        }

        public int RunTests(string handle)
        {
            using (var cl = new InterprocessClient<INunitRunnerClient>(handle))
            {
                var str = cl.Instance.GiveMeConfig();
                var cfg = JsonSerializer.DeserializeFromString<TestRunConfig>(str);
                Parallel.For(0, cfg.TestsToRun.Count, i =>
                    {
                        var path = cfg.DllPaths[i];
                        var items = cfg.TestsToRun[i].ToArray();
                        if (i > 0 && cfg.StartDelay > 0)
                        {
                            Thread.Sleep(i*cfg.StartDelay);
                        }
                        ProcessMessage(s => s.Run(handle, path, items, !config.NeedSynchronizationForTests, config.NeedOutput, config.RuntimeFramework));
                    });
            }
            return 1;
        }

        private void ProcessMessage<T>(Func<NUnitTestStarter, T> action)
        {
            var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            domain.AssemblyResolve += config.ResolveEventHandler;
            try
            {
                var cl = (NUnitTestStarter)domain.CreateInstanceAndUnwrap("Mnk.Library.ParallelNunit", "Mnk.Library.ParallelNUnit.Core.NUnitTestStarter");
                action(cl);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected exception");
            }
            finally
            {
                AppDomain.Unload(domain);
            }
        }

    }
}
