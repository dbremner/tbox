using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ScriptEngine;

namespace Solution.Scripts
{
	public class ClearFolders : IScript
	{
		private const int MaxFilesInError = 32;

		[DirectoryList]
		public IList<string> RootDirectories { get; set; }

		[StringList("bin", "obj", "log", "logs")]
		public IList<string> DirectoryNamesToClean { get; set; }

		[StringList("libs")]
		public IList<string> StopOnFolders { get; set; }

		public void Run()
		{
			var errors = new List<string>();
			foreach (var dir in RootDirectories.Select(x => new DirectoryInfo(x)))
			{
				foreach (var name in DirectoryNamesToClean)
				{
					foreach (var subdir in FindSubDir(dir, name))
					{
						Clear(subdir, errors);
					}
				}
			}
			if (errors.Count <= 0) return;
			if (errors.Count > MaxFilesInError)
			{
				errors.RemoveRange(MaxFilesInError, errors.Count - MaxFilesInError);
				errors.Add("...");
			}
			MessageBox.Show("Can't remove next files: " + Environment.NewLine + string.Join(Environment.NewLine, errors));
		}

		private IEnumerable<DirectoryInfo> FindSubDir(DirectoryInfo dir, string name)
		{
			foreach (var folder in StopOnFolders)
			{
				if (Equals(folder, dir.Name))
				{
					return new DirectoryInfo[0];
				}
			}
			if (Equals(dir, name))
			{
				return new[] { dir };
			}
			return dir.GetDirectories()
				.SelectMany(subdir => FindSubDir(subdir, name))
				.Where(ret => ret != null);
		}

		private static bool Equals(DirectoryInfo dir, string name)
		{
			return string.Equals(dir.Name, name, StringComparison.OrdinalIgnoreCase);
		}

		private static void Clear(DirectoryInfo dir, IList<string> errors)
		{
			foreach (var file in dir.GetFiles("*.*", SearchOption.AllDirectories))
			{
				try
				{
					file.Delete();
				}
				catch{ errors.Add(file.FullName); }
			}
			foreach (var subdir in dir.GetDirectories())
			{
				try
				{
					subdir.Delete(true);
				}
				catch { errors.Add(subdir.FullName); }
			}
		}
	}
}
