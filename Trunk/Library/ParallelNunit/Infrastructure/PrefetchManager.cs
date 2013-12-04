using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.Base.Log;
using ParallelNUnit.Core;
using ServiceStack.Text;

namespace ParallelNUnit.Infrastructure
{
    class PrefetchManager
    {
        private readonly static string PrefetchFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox", "Tools", "NUnitRunner");
        private static readonly ILog Log = LogManager.GetLogger<PrefetchManager>();

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
                Log.Write(ex, "Unexpected error");
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
                Log.Write(ex, "Unexpected error");
            }
        }

        private static string GetFilePath(string path)
        {
            if (!Directory.Exists(PrefetchFolder)) Directory.CreateDirectory(PrefetchFolder);
            return Path.Combine(PrefetchFolder, Path.GetFileNameWithoutExtension(path) + ".config");
        }
    }
}
