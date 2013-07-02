using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Common.Base.Log;
using Common.Tools;
using NUnitRunner.Code.Settings;
using NUnitRunner.Code.Updater;

namespace NUnitRunner.Code
{
	class DirectoriesManipulator
	{
		private static readonly ILog Log = LogManager.GetLogger<DirectoriesManipulator>();

		public List<string> GenerateFolders(string path, IList<IList<Result>> packages, bool copyToLocalFolders, int copyDeep, IProgressStatus u)
		{
			var dllPathes = new List<string>();
			if (copyToLocalFolders)
			{
				CopyToLocalFolders(path, packages, copyDeep, u, dllPathes);
			}
			else
			{
				for (var i = 0; i < packages.Count; i++)
				{
					dllPathes.Add(path);
				}
			}
			return dllPathes;
		}

		private static void CopyToLocalFolders(string path, IList<IList<Result>> packages, int copyDeep, IProgressStatus u, List<string> dllPathes)
		{
			var dir = new DirectoryInfo(Path.GetDirectoryName(path));
			var name = Path.GetFileName(path);
			for (var i = 1; i < copyDeep && dir.Parent != null; ++i)
			{
				name = Path.Combine(dir.Name, name);
				dir = dir.Parent;
			}
			var temp = Path.GetTempPath();
			var fileId = 0;
			for (var i = 0; i < packages.Count; i++)
			{
				var folder = string.Empty;
				while (true)
				{
					folder = Path.Combine(temp, (++fileId).ToString(CultureInfo.InvariantCulture));
					var info = new DirectoryInfo(folder);
					if (info.Exists || File.Exists(folder)) continue;
					info.Create();
					u.Update("Copy files to: " + info.FullName);
					dir.CopyFilesTo(info.FullName);
					break;
				}
				dllPathes.Add(Path.Combine(folder, name));
			}
		}

		public void ClearFolders(IList<string> dllPathes, bool copyToLocalFolders, int copyDeep)
		{
			if (!copyToLocalFolders) return;
			foreach (var t in dllPathes)
			{
				try
				{
					var name = Path.GetDirectoryName(t);
					for (var j = 1; j < copyDeep; ++j)
					{
						name = Path.GetDirectoryName(name);
					}
					Directory.Delete(name, true);
				}
				catch (Exception ex)
				{
					Log.Write(ex, "Can't delete folder: " + t);
				}
			}
		}
	}
}
