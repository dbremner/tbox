using System;
using System.Windows;
using System.Windows.Controls;
using Interface;
using MarketClient.Code;
using WPFControls.Dialogs;

namespace MarketClient
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ISettings
	{
		public Settings()
		{
			InitializeComponent();
			pluginsUploader.Init();
		}

		public UserControl Control { get { return this; } }

		private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (!((bool)e.NewValue)) return;
			IsEnabled = false;
			Dispatcher.BeginInvoke(
				new Action(() =>
				{
					var ret = false;
					DialogsCache.ShowProgress(u => { ret = Synchronizer.RefreshTables(u); }, "Refreshing..");
					IsEnabled = ret;
				}));
		}
	}
}
