using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;

namespace Mnk.Library.ParallelNUnit.Packages.Common
{
    class OrderOptimizationManager : IOrderOptimizationManager
    {
        private readonly static string PrefetchFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox", "Tools", "NUnitRunner");
        private readonly ILog log = LogManager.GetLogger<OrderOptimizationManager>();

        public IList<Result> Optimize(string path, IList<Result> tests)
        {
            try
            {
                var cache = GetFilePath(path);
                if (!File.Exists(cache)) return tests;
                using (var f = File.OpenRead(cache))
                {
                    var optimized = JsonSerializer.DeserializeFromStream<IList<int>>(f);
                    return tests.OrderBy(x =>
                    {
                        var id = optimized.IndexOf(x.Id);
                        return id < 0 ? int.MaxValue : id;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected error");
            }
            return tests;
        }

        public void SaveStatistic(string path, IList<Result> tests)
        {
            try
            {
                var cache = GetFilePath(path);
                if(File.Exists(cache))File.Delete(cache);
                using (var f = File.OpenWrite(cache))
                {
                    JsonSerializer.SerializeToStream(tests.OrderBy(x=>x.Time).Select(x=>x.Id).ToArray(), f);
                }
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected error");
            }
        }

        private static string GetFilePath(string path)
        {
            if (!Directory.Exists(PrefetchFolder)) Directory.CreateDirectory(PrefetchFolder);
            return Path.Combine(PrefetchFolder, Path.GetFileNameWithoutExtension(path) + ".config");
        }
    }
}
