using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mnk.Library.WpfWinForms.Icons
{
	public sealed class IconsCollector
	{
		private readonly IDictionary<string, ISet<int>> tasks = new Dictionary<string, ISet<int>>();
		private readonly IconsExtractor iconsExtractor = new IconsExtractor();
		private readonly IconsCache iconsCache;
		public IconsCollector(IconsCache iconsCache)
		{
			this.iconsCache = iconsCache;
		}

		public void AddTask(string path, params int[] indexes)
		{
			lock (tasks)
			{
				var ids = indexes.Where(i => !iconsCache.Exist(path, i)).ToArray();
				if (ids.Length <= 0) return;
				ISet<int> set;
				if (!tasks.TryGetValue(path, out set))
				{
					tasks[path] = set = new HashSet<int>();
				}
				foreach (var id in ids)
				{
					set.Add(id);
				}
			}
		}

		public bool CollectTasks()
		{
			lock (tasks)
			{
				if (tasks.Count == 0) return false;
				Work();
				tasks.Clear();
			}
			return true;
		}

		private void Work()
		{
			foreach (var task in tasks)
			{
				ExtractIcons(task.Key, task.Value);
			}
		}

		private void ExtractIcons(string path, IEnumerable<int> ids)
		{
			if (!File.Exists(path))return;
			foreach (var id in ids)
			{
				lock (tasks)
				{
					if (iconsCache.Exist(path, id)) continue;
					iconsCache.Add(path, id, iconsExtractor.GetIcon(path, id));
				}
			}
		}
	}
}
