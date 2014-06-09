using System;
using System.IO;
using System.Reflection;
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
        private readonly ILog log = LogManager.GetLogger<ThreadTestsExecutor>();

        public int RunTests(IThreadTestConfig config, string handle)
        {
            using (var cl = new InterprocessClient<INunitRunnerClient>(handle))
            {
                var str = cl.Instance.GiveMeConfig();
                var cfg = JsonSerializer.DeserializeFromString<TestRunConfig>(str);
                if(cfg == null)
                    throw new ArgumentNullException("Can't deserialize config: " + str);
                Parallel.For(0, cfg.TestsToRun.Count, i =>
                {
                    var path = cfg.DllPaths[i];
                    var items = cfg.TestsToRun[i].ToArray();
                    if (i > 0 && cfg.StartDelay > 0)
                    {
                        Thread.Sleep(i * cfg.StartDelay);
                    }
                    new NUnitTestStarter().Run(handle, path, items, !config.NeedSynchronizationForTests,
                        config.NeedOutput, config.RuntimeFramework);
                });
            }
            return 1;
        }

    }
}
