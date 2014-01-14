using System;
using System.Collections.Generic;
using System.Linq;

namespace Mnk.Library.ParallelNUnit.Infrastructure
{
    class CopyDeepManager
    {
        private const string Divider = "\\";

        public string[] NormalizePathes(IEnumerable<string> pathes)
        {
            return pathes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => string.Join(Divider, 
                    x.Replace("/", Divider).Trim().Split(new[] { Divider }, StringSplitOptions.RemoveEmptyEntries)))
                .ToArray();
        }

        public int CalcCopyDeep(IEnumerable<string> pathes)
        {
            return pathes.Max(x => CalcCopyDeep(x));
        }

        private static int CalcCopyDeep(string path)
        {
            var deep = 1;
            while (path.StartsWith("..\\"))
            {
                ++deep;
                path = path.Substring(3);
            }
            return deep;
        }

        public string[] BuildMaximumPathes(string directory, int copyDeep)
        {
            return directory
                .Split(new[] { Divider }, StringSplitOptions.RemoveEmptyEntries)
                .Reverse().Take(copyDeep - 1).Reverse()
                .ToArray();
        }

        public string NormalizePath(string path, int copyDeep, string[] maximumPathes)
        {
            var deep = CalcCopyDeep(path);
            return string.Join(Divider, 
                    maximumPathes
                    .Take(copyDeep - deep)
                    .Concat(
                        path
                        .Split(new []{Divider},StringSplitOptions.RemoveEmptyEntries)
                        .Skip(deep-1)));
        }
    }
}
