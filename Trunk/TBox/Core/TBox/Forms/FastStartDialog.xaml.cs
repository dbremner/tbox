using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Common.Data;
using Common.Tools;
using Interface;
using TBox.Code.FastStart;
using TBox.Code.FastStart.Settings;
using TBox.Code.Menu;
using WPFControls.Components.ButtonsView;
using WPFWinForms;
using WPFWinForms.Icons;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for FastStartDialog.xaml
	/// </summary>
	public sealed partial class FastStartDialog
	{
		private RecentItemsCollector recentItemsCollector;
		private FastStartConfig config;
		private IMenuItemsProvider menuItemsProvider;
		private Action closeAction;
		public FastStartDialog()
		{
			InitializeComponent();
			Loaded += ParentChanged;
		}

		private void ParentChanged(object sender, RoutedEventArgs e)
		{
			RecentActions.ItemsSource = recentItemsCollector.GetStatistic(config.MaxCount, closeAction);
			UserActions.ItemsSource = CollectRecentItems(config.MenuItemsSequence.CheckedItems, closeAction).ToArray();
		}

		internal void Init(RecentItemsCollector itemsCollector, FastStartConfig cfg, IMenuItemsProvider menuProvider, Action closeAction)
		{
			this.closeAction = closeAction;
			recentItemsCollector = itemsCollector;
			config = cfg;
			menuItemsProvider = menuProvider;
		}

		private IEnumerable<IButtonInfo> CollectRecentItems(IEnumerable<MenuItemsSequence> checkedItems, Action closeAction)
		{
			foreach (var item in checkedItems)
			{
				var selected = item.MenuItems.CheckedItems.ToArray();
				if (!selected.Any()) continue;
				var items = selected.Select(o =>
					new Pair<UMenuItem, UMenuItem>(
						menuItemsProvider.Get(o.Key),
						menuItemsProvider.GetRoot(o.Key.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
						)).ToArray();
				if (items.Any(x => x.Key == null || x.Value == null)) continue;
				var last = items.LastOrDefault(x => x.Key.Icon != null || x.Value.Icon != null);
				if(last == null)continue;
				yield return new ButtonInfo
				{
					Name = item.Key,
					Icon = (last.Key.Icon ?? last.Value.Icon).ToImageSource(),
					Handler = (o, e) =>
					{
						closeAction();
						items.ForEach(x => x.Key.OnClick(new NonUserRunContext()));
					}
				};
			}
		}
	}
}
