using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Mnk.Library.Common.Log;
using NUnit.Core;
using NUnit.Util;

namespace Mnk.Library.ParallelNUnit.Core
{
    public sealed class NUnitTestStarter 
    {
        private static readonly object Sync = new object();
        private readonly ILog log = LogManager.GetLogger<NUnitTestStarter>();

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
