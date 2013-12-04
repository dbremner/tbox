using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Base.Log;
using Common.Communications.Interprocess;
using ParallelNUnit.Core;
using ParallelNUnit.Interfaces;
using ServiceStack.Text;

namespace ParallelNUnit.Execution
{
    public class NUnitExecutor
    {
        private readonly ILog log = LogManager.GetLogger<NUnitExecutor>();
        private readonly ResolveEventHandler loadFromSameFolder;

        public NUnitExecutor(ResolveEventHandler loadFromSameFolder)
        {
            this.loadFromSameFolder = loadFromSameFolder;
        }

        public Result CollectTests(string path)
        {
            return new NUnitTestStarter().CollectTests(path);
        }

        public int RunTests(string handle, bool fast, bool needOutput)
        {
            using (var cl = new InterprocessClient<INunitRunnerClient>(handle))
            {
                var str = cl.Instance.GiveMeConfig();
                var config = JsonSerializer.DeserializeFromString<TestRunConfig>(str);
                Parallel.For(0, config.TestsToRun.Count, i =>
                    {
                        var path = config.DllPathes[i];
                        var items = config.TestsToRun[i].ToArray();
                        if (i > 0 && config.StartDelay > 0)
                        {
                            Thread.Sleep(i*config.StartDelay);
                        }
                        ProcessMessage(s => s.Run(handle, path, items, fast, needOutput));
                    });
            }
            return 1;
        }

        private void ProcessMessage<T>(Func<NUnitTestStarter, T> action)
        {
            var domain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
            domain.AssemblyResolve += loadFromSameFolder;
            try
            {
                var cl = (NUnitTestStarter)domain.CreateInstanceAndUnwrap("ParallelNUnit", "ParallelNUnit.Core.NUnitTestStarter");
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
