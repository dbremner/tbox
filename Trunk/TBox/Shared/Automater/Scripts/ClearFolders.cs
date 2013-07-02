using System.Collections.Generic;
using System.IO;
using System.Linq;
using ScriptEngine;

namespace Solution.Scripts
{
	public class ClearFolders : IScript
	{
		[DirectoryList]
		public IList<string> DirectoriesToClean { get; set; }

		public void Run()
		{
			foreach (var dir in DirectoriesToClean.Select(x => new DirectoryInfo(x)))
			{
				foreach (var file in dir.GetFiles())
				{
					file.Delete();
				}
				foreach (var subdir in dir.GetDirectories())
				{
					subdir.Delete(true);
				}
			}
		}
	}
}
