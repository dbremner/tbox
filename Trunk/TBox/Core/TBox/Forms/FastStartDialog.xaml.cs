using System;
using System.Collections.ObjectModel;
using System.Windows;
using TBox.Code;
using TBox.Code.FastStart;
using TBox.Code.FastStart.Settings;
using TBox.Code.Menu;
using WPFControls.Components.ButtonsView;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for FastStartDialog.xaml
	/// </summary>
	public sealed partial class FastStartDialog
	{
		private RecentItemsCollector recentItemsCollector;
        private ConfigManager cm;
		private readonly Action closeAction;
		public ObservableCollection<IButtonInfo> Items { get; private set; }
		public FastStartDialog(Action closeAction)
		{
			Items = new ObservableCollection<IButtonInfo>();
			this.closeAction = closeAction;
			DataContext = this;
			InitializeComponent();
			Loaded += ParentChanged;
		}

		internal void Init(ConfigManager cm, RecentItemsCollector recentItemsCollector)
		{
		    this.cm = cm;
			this.recentItemsCollector = recentItemsCollector;
		}

		private void ParentChanged(object sender, RoutedEventArgs e)
		{
			Items.Clear();
			recentItemsCollector.GetStatistic(cm.Config.FastStartConfig.MaxCount, Items, closeAction);
            recentItemsCollector.CollectUserActions(cm.Config.FastStartConfig.MenuItemsSequence.CheckedItems, Items, closeAction);
			ItemsList.DataContext = this;
			ItemsList.Refresh();
		}

	}
}
