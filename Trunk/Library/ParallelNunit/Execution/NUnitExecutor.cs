﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.Common.Communications.Interprocess;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Interfaces;
using ServiceStack.Text;

namespace Mnk.Library.ParallelNUnit.Execution
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
