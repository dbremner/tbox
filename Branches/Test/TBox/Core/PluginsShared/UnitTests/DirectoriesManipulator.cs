using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Common.Base.Log;
using Common.Tools;
using Localization.PluginsShared;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;

namespace PluginsShared.UnitTests
{
	class DirectoriesManipulator
	{
		private static readonly ILog Log = LogManager.GetLogger<DirectoriesManipulator>();

		public List<string> GenerateFolders(string path, IList<IList<Result>> packages, bool copyToLocalFolders, int copyDeep, string dirToCloneTests, IProgressStatus u)
		{
			var dllPaths = new List<string>();
			if (copyToLocalFolders)
			{
                u.Update(PluginsSharedLang.StartClonningUnitTestsFolder);
				CopyToLocalFolders(path, packages, copyDeep, dirToCloneTests, u, dllPaths);
                u.Update(PluginsSharedLang.FinishClonningUnitTestsFolder);
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

        private static void CopyToLocalFolders(string path, IList<IList<Result>> packages, int copyDeep, string dirToCloneTests, IProgressStatus u, List<string> dllPaths)
		{
			var dir = new DirectoryInfo(Path.GetDirectoryName(path));
			var name = Path.GetFileName(path);
			for (var i = 1; i < copyDeep && dir.Parent != null; ++i)
			{
				name = Path.Combine(dir.Name, name);
				dir = dir.Parent;
			}
            dirToCloneTests = Path.GetFullPath(dirToCloneTests);
			var fileId = 0;
			for (var i = 0; i < packages.Count; i++)
			{
				var folder = string.Empty;
				while (true)
				{
                    if(u.UserPressClose)return;
                    folder = Path.Combine(dirToCloneTests, (++fileId).ToString(CultureInfo.InvariantCulture));
					var info = new DirectoryInfo(folder);
					if (info.Exists || File.Exists(folder)) continue;
					info.Create();
					u.Update("Copy files to: " + info.FullName);
					dir.CopyFilesTo(info.FullName);
					break;
				}
				dllPaths.Add(Path.Combine(folder, name));
			}
		}

		public void ClearFolders(IList<string> dllPaths, bool copyToLocalFolders, int copyDeep)
		{
			if (!copyToLocalFolders) return;
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
