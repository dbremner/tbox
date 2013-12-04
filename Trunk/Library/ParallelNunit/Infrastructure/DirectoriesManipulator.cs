using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Base.Log;
using Common.Tools;
using ParallelNUnit.Core;
using ParallelNUnit.Execution;
using ParallelNUnit.Infrastructure.Interfaces;

namespace ParallelNUnit.Infrastructure
{
    class DirectoriesManipulator
    {
        private static readonly ILog Log = LogManager.GetLogger<DirectoriesManipulator>();
        private readonly CopyDeepManager cdManager = new CopyDeepManager();

        public List<string> GenerateFolders(string path, IList<IList<Result>> packages, bool copyToLocalFolders, string[] copyMasks, string dirToCloneTests, IProgressStatus u)
        {
            var dllPaths = new List<string>();
            if (copyToLocalFolders)
            {
                u.Update("Start cloning of the unit tests folder");
                CopyToLocalFolders(path, packages, copyMasks, dirToCloneTests, u, dllPaths);
                u.Update("Cloning of the unit tests folder finished");
            }
            else
            {
                for (var i = 0; i < packages.Count; i++)
                {
                    dllPaths.Add(path);
                }
            }
            return dllPaths;
        }

        private void CopyToLocalFolders(string path, IList<IList<Result>> packages, string[] copyMasks, string dirToCloneTests, IProgressStatus u, List<string> dllPaths)
        {
            copyMasks = cdManager.NormalizePathes(copyMasks);
            var copyDeep = cdManager.CalcCopyDeep(copyMasks);
            var source = new DirectoryInfo(Path.GetDirectoryName(path));
            var name = Path.GetFileName(path);
            var maximumPathes = cdManager.BuildMaximumPathes(source.FullName, copyDeep);
            copyMasks = copyMasks
                .Select(x => cdManager.NormalizePath(x, copyDeep, maximumPathes))
                .ToArray();
            var filters = copyMasks
                .Select(x => new Regex(x.Replace(".", "[.]").Replace("*", ".*").Replace("?", ".").Replace("\\", "\\\\"), RegexOptions.Compiled | RegexOptions.IgnoreCase))
                .ToArray();
            for (var i = 1; i < copyDeep && source.Parent != null; ++i)
            {
                name = Path.Combine(source.Name, name);
                source = source.Parent;
            }
            dirToCloneTests = Path.GetFullPath(dirToCloneTests);
            var fileId = 0;
            DirectoryInfo existCopiedData = null;
            for (var i = 0; i < packages.Count; i++)
            {
                var folder = string.Empty;
                while (true)
                {
                    if (u.UserPressClose) return;
                    folder = Path.Combine(dirToCloneTests, (++fileId).ToString(CultureInfo.InvariantCulture));
                    var destination = new DirectoryInfo(folder);
                    if (destination.Exists || File.Exists(folder)) continue;
                    destination.Create();
                    u.Update("Copy files to: " + destination.FullName);
                    if (existCopiedData == null)
                    {
                        CopyFiles(source, destination, filters);
                        existCopiedData = destination;
                    }
                    else
                    {
                        existCopiedData.CopyFilesTo(destination.FullName, false);
                    }
                    break;
                }
                dllPaths.Add(Path.Combine(folder, name));
            }
        }

        private static void CopyFiles(DirectoryInfo source, DirectoryInfo destination, IEnumerable<Regex> filters)
        {
            foreach (var file in source.GetFiles("*", SearchOption.AllDirectories).Where(x=>filters.Any(o=>o.IsMatch(x.FullName.Substring(source.FullName.Length)))))
            {
                var target = file.FullName.Replace(source.FullName, destination.FullName);
                var folderName = Path.GetDirectoryName(target);
                if (!Directory.Exists(folderName)) Directory.CreateDirectory(folderName);
                file.CopyTo(target);
            }
        }

        public void ClearFolders(IList<string> dllPaths, bool copyToLocalFolders, string[] copyMasks)
        {
            if (!copyToLocalFolders) return;
            var copyDeep = cdManager.CalcCopyDeep(cdManager.NormalizePathes(copyMasks));
            foreach (var t in dllPaths)
            {
                try
                {
                    var name = Path.GetDirectoryName(t);
                    for (var j = 1; j < copyDeep; ++j)
                    {
                        name = Path.GetDirectoryName(name);
                    }
                    name = name ?? string.Empty;
                    if (Directory.Exists(name))
                    {
                        Directory.Delete(name, true);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex, "Can't delete folder: " + t);
                }
            }
        }

    }
}
