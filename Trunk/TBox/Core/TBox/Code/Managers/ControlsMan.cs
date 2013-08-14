using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Common.Base.Log;
using Common.Tools;
using TBox.Code.Objects;

namespace TBox.Code.Managers
{
	sealed class ControlsMan
	{
		private static readonly ILog Log = LogManager.GetLogger<ControlsMan>();
		private static readonly ILog InfoLog = LogManager.GetInfoLogger<ControlsMan>();
		private readonly ContentControl owner;
		private readonly Action<PluginName> onPluginChanged;
		private readonly ListBox itemsList;
 		private readonly List<ItemContainer> items = new List<ItemContainer>();

		public ControlsMan(ListBox itemsList, ContentControl owner, Action<PluginName> onPluginChanged)
		{
			this.itemsList = itemsList;
			this.owner = owner;
			this.onPluginChanged = onPluginChanged;
			this.itemsList.ItemsSource = items;
			this.itemsList.SelectionChanged += SelectPlugin;
		}

		public int Count
		{
			get { return itemsList.Items.Count; }
		}

		private void SelectPlugin(object sender, SelectionChangedEventArgs e)
		{
			var id = itemsList.SelectedIndex;
			if (id < 0) return;
			var time = Environment.TickCount;
			var item = items[id];
			owner.Content = item.Getter();
			InfoLog.Write("Open settings for '{0}', time: {1}", item.Key.Name, Environment.TickCount - time);
			onPluginChanged(item.Key);
		}

		private ItemContainer FindContainer(string name)
		{
			return items.FirstOrDefault(x => x.ToString() == name);
		}

		public void Add(PluginName name, Func<Control> ctrlGetter)
		{
			var item = FindContainer(name.Name);
			if( item!=null )
			{
				Log.Write("Duplicate item: {0}!", name.Name);
				return;
			}
			items.Insert(new ItemContainer(name , ctrlGetter), x=>x.Key.Name, 1, items.Count);
		}

		public void Remove(string name)
		{
			var find = FindContainer(name);
			if(find!=null)
			{
				items.Remove(find);
			}
		}

		public void UpdateSelection()
		{
			if (itemsList.Items.Count == 0)
			{
				Log.Write("Can't select first item!");
				return;
			}
			if(itemsList.SelectedIndex == -1)
			{
				itemsList.SelectedIndex = 0;
			}
		}

		public void Refresh()
		{
			itemsList.Items.Refresh();
		}
	}
}
