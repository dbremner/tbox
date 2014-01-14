using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.TBox.Plugins.Searcher.Code.Checkers;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Scanner;
using Mnk.TBox.Plugins.Searcher.Code.Finders.Search;

namespace Mnk.TBox.Plugins.Searcher.Code.Search
{
	class FileInformer : IFileInformer
	{
		private string[] dirs;
		private Dictionary<string, SearchFileInfo[]> files;
		public const int MaxFilesPerType = 10000000;

		public bool Find(ISet<string> types, IFileChecker checker, int maxFiles, ICollection<int> list)
		{
			var count = 0;
			foreach (var data in files)
			{
				if (types.Contains(data.Key))
				{
					for (var i = 0; i < data.Value.Length; ++i)
					{
						if (!checker.Check(data.Value[i].Name)) continue;
						list.Add(i + count);
						if (list.Count >= maxFiles)
							return false;
					}
				}
				count += MaxFilesPerType;
			}
			return true;
		}

		public string GetFilePath(int id)
		{
			foreach (var data in files)
			{
				if (data.Value.Length > id)
				{
					var list = data.Value[id].Dir;
					var path = string.Empty;
					if (list.Count > 0)
					{
						path = dirs[list[0]];
						for (var i = 1; i < list.Count; i++)
						{
							path = Path.Combine(path, dirs[list[i]]);
						}
					}
					return Path.Combine(path, string.Format("{0}.{1}", data.Value[id].Name, data.Key));
				}
				id -= MaxFilesPerType;
			}
			return string.Empty;
		}

		public string GetFileExt(int id)
		{
			foreach (var data in files)
			{
				if (data.Value.Length > id)
				{
					return data.Key;
				}
				id -= MaxFilesPerType;
			}
			return string.Empty;
		}

		public void Clear()
		{
			if(files!=null)files.Clear();
			dirs = new string[0];
		}

		public void Load(Scanner scanner)
		{
			dirs = scanner.Dirs.ToArray();
			files = scanner.Files.ToDictionary(x=>x.Key, x=>x.Value.ToArray());
		}

		public void Load(string path, CheckableDataCollection<CheckableData> fileTypes)
		{
			Clear();
			if (fileTypes == null) return;
			var dirName = path;
			dirs = dirs.Load(Path.Combine(dirName, Folders.DirPath));
			dirName = Path.Combine(dirName, Folders.FilesPath);
			files = new Dictionary<string, SearchFileInfo[]>(fileTypes.Count);
			foreach (var s in fileTypes)
			{
				files.Add(s.Key, LoadFiles(Path.Combine(dirName, s.Key)));
			}
		}

		public static SearchFileInfo[] LoadFiles(string fileName)
		{
			using (var s = File.OpenRead(fileName))
			{
				using (var stream = new BinaryReader(s))
				{
					var result = new SearchFileInfo[stream.ReadInt32()];
					for (var i = 0; i < result.Length; ++i)
					{
						var name = stream.ReadString();
						var dir = new int[stream.ReadInt32()];
						for (var j = 0; j < dir.Length; j++)
						{
							dir[j] = stream.ReadInt32();
						}
						result[i] = new SearchFileInfo(name, dir);
					}
					return result;
				}
			}
		}
	}
}
