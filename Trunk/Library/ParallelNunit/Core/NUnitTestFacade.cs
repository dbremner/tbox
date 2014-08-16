using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mnk.Library.Common.Communications;
using Mnk.Library.Common.Log;
using Mnk.Library.ParallelNUnit.Contracts;
using NUnit.Core;
using NUnit.Util;
using Mnk.Library.Common.Tools;
namespace Mnk.Library.ParallelNUnit.Core
{
    public sealed class NUnitTestFacade : INUnitTestFacade
    {
        private static readonly object Sync = new object();
        private readonly ILog log = LogManager.GetLogger<NUnitTestFacade>();

        public int RunTests(ITestsConfig config, string handle)
        {
            using (var cl = new InterprocessClient<INunitRunnerClient>(handle))
            {
                var str = cl.Instance.GiveMeConfig();
                var cfg = JsonSerializer.DeserializeFromString<TestRunConfig>(str);
                if (cfg == null)
                    throw new ArgumentNullException("Can't deserialize config: " + str);
                Parallel.For(0, cfg.TestsToRun.Count, i =>
                {
                    var path = cfg.DllPaths[i];
                    var items = cfg.TestsToRun[i].ToArray();
                    if (i > 0 && cfg.StartDelay > 0)
                    {
                        Thread.Sleep(i * cfg.StartDelay);
                    }
                    Run(handle, path, items, !config.NeedSynchronizationForTests,
                        config.NeedOutput, config.RuntimeFramework);
                });
            }
            return 0;
        }

        public int Run(string handle, string path, int[] items, bool fast, bool needOutput, string runtimeFramework)
        {
            var result = -1;
            Execute(path, runtimeFramework,
                runner =>
                {
                    runner.Run(new RemoteListener { Handle = handle, Fast = fast, Needoutput = needOutput },
                                new Filter { Items = new HashSet<int>(items) },
                                false,
                                LoggingThreshold.Off
                        );
                    result = items.Length;
                });
            return result;
        }

        public Result CollectTests(string path, string runtimeFramework)
        {
            Result result = null;
            Execute(path, runtimeFramework,
                runner => result = CollectResults(runner.Test, new string[0]));
            return result;
        }

        private void Execute(string path, string runtimeFramework, Action<TestRunner> action)
        {
            var p = NUnitBase.CreatePackage(path, runtimeFramework);
            using (var runner = new DefaultTestRunnerFactory().MakeTestRunner(p))
            {
                lock (Sync)
                {
                    if (!runner.Load(p))
                    {
                        log.Write("Can't load: " + path);
                        return;
                    }
                }
                try
                {
                    action(runner);
                }
                finally
                {
                    runner.Unload();
                }
            }
        }

        private static Result CollectResults(ITest result, IEnumerable<string> ownerCategories)
        {
            var categories = ownerCategories.Concat(GetCategories(result)).Distinct().ToArray();
            var item = CreateResult(result, categories);
            if (result.Tests != null)
            {
                foreach (ITest r in result.Tests)
                {
                    item.Children.Add(CollectResults(r, categories));
                }
            }
            return item;
        }

        private static Result CreateResult(ITest result, string[] categories)
        {
            return new Result
            {
                Id = int.Parse(result.TestName.TestID.ToString(), CultureInfo.InvariantCulture),
                Key = result.TestName.Name,
                FullName = result.TestName.FullName,
                Categories = categories,
                Type = result.TestType,
            };
        }

        private static IEnumerable<string> GetCategories(ITest result)
        {
            return (result.Categories == null ?
                new object[0] : result.Categories.Cast<object>())
                .Select(x => x.ToString())
                .ToArray();
        }

    }
}
