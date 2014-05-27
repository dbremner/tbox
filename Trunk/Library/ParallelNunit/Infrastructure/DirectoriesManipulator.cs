using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;

namespace Mnk.Library.ParallelNUnit.Infrastructure
{
    class DirectoriesManipulator
    {
        private static readonly ILog Log = LogManager.GetLogger<DirectoriesManipulator>();
        private readonly CopyDirGenerator cdGenerator = new CopyDirGenerator();

        public List<string> GenerateFolders(string path, int count, bool copyToLocalFolders, string[] copyMasks, string dirToCloneTests, IProgressStatus u)
        {
            var dllPaths = new List<string>();
            if (copyToLocalFolders)
            {
                u.Update("Start cloning of the unit tests folder");
                CopyToLocalFolders(path, count, copyMasks, dirToCloneTests, u, dllPaths);
                u.Update("Cloning of the unit tests folder finished");
            }
            else
            {
                for (var i = 0; i < count; i++)
                {
                    dllPaths.Add(path);
                }
            }
            return dllPaths;
        }

        private void CopyToLocalFolders(string path, int count, string[] copyMasks, string dirToCloneTests, IProgressStatus u, List<string> dllPaths)
        {
            string name;
            string sourceDir;
            var files = cdGenerator.GetFiles(path, copyMasks, out name, out sourceDir);
            foreach (var folder in GenerateFolders(count, dirToCloneTests))
            {
                u.Update("Copy files to: " + folder);
                foreach (var item in files)
                {
                    if(u.UserPressClose)break;
                    var target = Path.Combine(folder, item.Key);
                    if (!Directory.Exists(target)) Directory.CreateDirectory(target);
                    foreach (var file in item.Value)
                    {
                        File.Copy(Path.Combine(sourceDir, item.Key, file), Path.Combine(target, file));
                    }
                }
                if (u.UserPressClose) break;
                dllPaths.Add(Path.Combine(folder, name));
            }
        }

        private static IEnumerable<string> GenerateFolders(int count, string dirToCloneTests)
        {
            var folders = new List<string>(count);
            var fileId = 0;
            for (var i = 0; i < count; ++i)
            {
                string folder;
                while (true)
                {
                    folder = Path.Combine(dirToCloneTests, (++fileId).ToString(CultureInfo.InvariantCulture));
                    if (!File.Exists(folder) && !Directory.Exists(folder)) break;
                }
                folders.Add(folder);
            }
            return folders;
        }

        public void ClearFolders(IList<string> dllPaths, bool copyToLocalFolders, string[] copyMasks)
        {
            if (!copyToLocalFolders) return;
            var copyDeep = cdGenerator.CalcCopyDeep(cdGenerator.NormalizePaths(copyMasks));
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
