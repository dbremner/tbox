using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mnk.Library.WpfWinForms.Icons
{
	public sealed class IconsCache : IDisposable
	{
		private readonly IDictionary<string, IDictionary<int, Icon>> icons = 
			new Dictionary<string, IDictionary<int, Icon>>();

		public Icon Get(string path, int id)
		{
			IDictionary<int, Icon> dict;
			if (icons.TryGetValue(path, out dict))
			{
				Icon icon;
				if (dict.TryGetValue(id, out icon))
				{
					return icon;
				}
			}
			return null;
		}

		public void Add(string path, int id, Icon icon)
		{
			IDictionary<int, Icon> dict;
			if (!icons.TryGetValue(path, out dict))
			{
				dict = new Dictionary<int, Icon>();
				icons[path] = dict;
			}
			dict[id] = icon;
		}

		public bool Exist(string path, int id)
		{
			IDictionary<int, Icon> dict;
			return icons.TryGetValue(path, out dict) && dict.ContainsKey(id);
		}

		public void Dispose()
		{
			foreach (var i in icons.SelectMany(icon => icon.Value.Where(i => i.Value!=null)))
			{
				i.Value.Dispose();
			}
			icons.Clear();
		}
	}
}
