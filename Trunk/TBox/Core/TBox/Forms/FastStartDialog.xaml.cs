using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Common.Tools;
using Interface;
using TBox.Code.FastStart;
using TBox.Code.FastStart.Settings;
using TBox.Code.Menu;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using WPFWinForms.Icons;
using Image = System.Windows.Controls.Image;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for FastStartDialog.xaml
	/// </summary>
	public partial class FastStartDialog
	{
		private RecentItemsCollector recentItemsCollector;
		private FastStartConfig config;
		private FastStartShower fastStartShower;
		private IMenuItemsProvider menuItemsProvider;
		public FastStartDialog()
		{
			InitializeComponent();
		}

		internal void Init(RecentItemsCollector itemsCollector, FastStartConfig cfg, FastStartShower startShower, IMenuItemsProvider menuProvider)
		{
			recentItemsCollector = itemsCollector;
			config = cfg;
			fastStartShower = startShower;
			menuItemsProvider = menuProvider;
			this.SetState(config.DialogState);
		}

		protected override void OnShow()
		{
			base.OnShow();
			RecentActions.Children.Clear();
			foreach (var item in recentItemsCollector.GetStatistic(config.MaxCount))
			{
				RecentActions.Children.Add(CreateButton(item, item.Icon));
			}
			UserActions.Children.Clear();
			foreach (var item in config.MenuItemsSequence.CheckedItems)
			{
				var selected = item.MenuItems.CheckedItems.ToArray();
				if (!selected.Any()) continue;
				var items = selected.Select(x => menuItemsProvider.Get(x.Key)).ToArray();
				UserActions.Children.Add(
					CreateButton(
							PrepareItem(item, items),
							items.Select((x,i)=>x.Icon??
								menuItemsProvider.GetRoot(selected[i].Key.Split(new[]{Environment.NewLine},StringSplitOptions.RemoveEmptyEntries).First()).Icon
								).ToArray()
						)
					);
			}
		}

		private static MenuItemStatistic PrepareItem(MenuItemsSequence item, IEnumerable<UMenuItem> items)
		{
			return new MenuItemStatistic
				{
					Path = item.Key,
					OnClick = o=>items.ForEach(x=>x.OnClick(new SchedulerContext()))
				};
		}

		private Control CreateButton(MenuItemStatistic detail, params Icon[] icons)
		{
			var panel = new DockPanel();
			var wp = new WrapPanel{HorizontalAlignment = HorizontalAlignment.Center};
			var size = GetIconSize(icons.Count(x => x != null));
			foreach (var icon in icons)
			{
				if (icon == null) continue;
				var image = new Image
					{
						Source = icon.ToImageSource(),
						Width = size,
						Height = size,
						HorizontalAlignment = HorizontalAlignment.Center
					};
				wp.Children.Add(image);
			}
			DockPanel.SetDock(wp, Dock.Top);
			panel.Children.Add(wp);
			var pathes = detail.Path.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
			panel.Children.Add(new StackPanel
				{
					Children =
						{
							new TextBlock
							{
								TextWrapping = TextWrapping.WrapWithOverflow,
								HorizontalAlignment = HorizontalAlignment.Center,
								Text = pathes[0],
								FontWeight = FontWeights.Bold
							},
							new TextBlock
							{
								TextWrapping = TextWrapping.WrapWithOverflow,
								Text = string.Join(" \\ ", pathes.Skip(1))
							}
						}
				});
			var btn = new Button
				{
					Width = 100,
					Height = 100,
					Margin = new Thickness(5),
					Content = panel,
				};
			btn.Click += (o, e) =>
				{
					Close();
					detail.OnClick(e);
				};
			return btn;
		}

		private static int GetIconSize(int count)
		{
			switch (count)
			{
				case 1:  return 32;
				case 2:  return 32;
				case 3:  return 24;
				case 4:  return 20;
				default: return 16;
			}
		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void ButtonSettings_Click(object sender, RoutedEventArgs e)
		{
			Close();
			fastStartShower.ShowSettings();
		}

		public override void Dispose()
		{
			config.DialogState = this.GetState();
			base.Dispose();
		}
	}
}
