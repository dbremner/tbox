using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Mnk.Library.Common.Tools
{
    public class CopyDirGenerator
    {
        private const string Divider = "\\";

        public IDictionary<string, IList<string>> GetFiles(string path, string[] copyMasks, out string name, out string sourceDir)
        {
            var source = new DirectoryInfo(Path.GetDirectoryName(path));
            name = Path.GetFileName(path);
            int copyDeep;
            var filters = GetFilters(copyMasks, source, out copyDeep);
            for (var i = 1; i < copyDeep && source.Parent != null; ++i)
            {
                name = Path.Combine(source.Name, name);
                source = source.Parent;
            }
            sourceDir = source.FullName;
            return GenerateDictionary(source, filters);
        }

        private Regex[] GetFilters(string[] copyMasks, DirectoryInfo source, out int copyDeep)
        {
            copyMasks = NormalizePathes(copyMasks);
            var deep = CalcCopyDeep(copyMasks);
            var maximumPathes = BuildMaximumPathes(source.FullName, deep);
            var filters = copyMasks
                .Select(x => NormalizePath(x, deep, maximumPathes))
                .Select(
                    x =>
                        new Regex(x.Replace(".", "[.]").Replace("*", ".*").Replace("?", ".").Replace("\\", "\\\\"),
                            RegexOptions.Compiled | RegexOptions.IgnoreCase))
                .ToArray();
            copyDeep = deep;
            return filters;
        }

        private static IDictionary<string, IList<string>> GenerateDictionary(DirectoryInfo source, Regex[] filters)
        {
            var result = new Dictionary<string, IList<string>>();
            foreach (
                var file in
                    source.EnumerateFiles("*", SearchOption.AllDirectories)
                        .Where(x => filters.Any(o => o.IsMatch(x.FullName.Substring(source.FullName.Length)))))
            {
                if (CheckAttributes(file) )continue;
                var target = file.Directory.FullName.Replace(source.FullName, string.Empty);
                if (!string.IsNullOrEmpty(target)) target = target.Substring(1);
                IList<string> list;
                if (!result.TryGetValue(target, out list))
                {
                    list = new List<string>();
                    result[target] = list;
                }
                list.Add(file.Name);
            }
            return result;
        }

        private static bool CheckAttributes(FileInfo file)
        {
            if (CheckAttributes(file.Attributes)) return true;
            var dir = file.Directory;
            if (dir.Parent == null) return false;
            while (!CheckAttributes(dir.Attributes))
            {
                dir = dir.Parent;
                if (dir.Parent == null) return false;
            }
            return true;
        }

        private static bool CheckAttributes(FileAttributes a)
        {
            return a.HasFlag(FileAttributes.Hidden) || a.HasFlag(FileAttributes.System) || a.HasFlag(FileAttributes.ReadOnly);
        }

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
            while (path.StartsWith("..\\", StringComparison.OrdinalIgnoreCase))
            {
                ++deep;
                path = path.Substring(3);
            }
            return deep;
        }

        private static IEnumerable<string> BuildMaximumPathes(string directory, int copyDeep)
        {
            return directory
                .Split(new[] { Divider }, StringSplitOptions.RemoveEmptyEntries)
                .Reverse().Take(copyDeep - 1).Reverse()
                .ToArray();
        }

        private static string NormalizePath(string path, int copyDeep, IEnumerable<string> maximumPathes)
        {
            var deep = CalcCopyDeep(path);
            return string.Join(Divider,
                    maximumPathes
                    .Take(copyDeep - deep)
                    .Concat(
                        path
                        .Split(new[] { Divider }, StringSplitOptions.RemoveEmptyEntries)
                        .Skip(deep - 1)));
        }
    }
}
