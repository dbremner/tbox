using System.Collections.Generic;
using System.Linq;
using Mnk.Library.Common.Tools;
using Mnk.Library.WPFWinForms;

namespace Mnk.Library.WPFControls.Dialogs.Menu
{
	public static class MenuExtensions
	{
		public static UMenuItem GetMenuItem(this IEnumerable<UMenuItem> items, IEnumerable<string> path)
		{
			UMenuItem item = null;
			foreach (var name in path)
			{
				if (items == null) return null;
				item = items.FirstOrDefault(x => x.Header.EqualsIgnoreCase(name));
				if (item == null) return null;
				items = item.Items;
			}
			return item;
		}
	}
}
