using System;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace TBox.Code.FastStart.Settings
{
	[Serializable]
	public class MenuItemsSequence : CheckableData
	{
		public CheckableDataCollection<CheckableData> MenuItems { get; set; }

		public MenuItemsSequence()
		{
			MenuItems = new CheckableDataCollection<CheckableData>();
		}

		public override object Clone()
		{
			return new MenuItemsSequence
				{
					IsChecked = IsChecked,
					Key = Key,
					MenuItems = MenuItems.Clone()
				};
		}
	}
}
