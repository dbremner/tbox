using System;
using System.Collections.Generic;
using System.Linq;
using Common.Base.Log;
using NUnit.Core;
using NUnit.Util;

namespace ParallelNUnit.Core
{
    public sealed class NUnitTestStarter : MarshalByRefObject
    {
        private readonly ILog log = LogManager.GetLogger<NUnitTestStarter>();

        public int Run(string handle, string path, int[] items, bool fast, bool needOutput)
        {
            var p = NUnitBase.CreatePackage(path);
            using (var runner = new TestDomain())
            {
                if (!runner.Load(p))
                {
                    log.Write("Can't load: " + path);
                    return -1;
                }
                try
                {
                    runner.Run(new RemoteListener { Handle = handle, Fast = fast, Needoutput = needOutput },
                                new Filter { Items = new HashSet<int>(items) },
                                false,
                                LoggingThreshold.Off
                        );
                    return items.Length;
                }
                finally
                {
                    runner.Unload();
                }
            }
        }

        public Result CollectTests(string path)
        {
            using (var runner = new TestDomain())
            {
                var p = NUnitBase.CreatePackage(path);
                if (!runner.Load(p)) return null;
                try
                {
                    return CollectResults(runner.Test, new string[0]);
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
                Id = int.Parse(result.TestName.TestID.ToString()),
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
