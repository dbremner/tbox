using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Common.Base;
using Common.Base.Log;
using Common.Tools;
using Interface;
using AppConfigManager.Code;
using AppConfigManager.Components;

namespace AppConfigManager
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ISettings
	{
		private static readonly ILog Log = LogManager.GetLogger<Settings>();
		public ObservableCollection<string> KnownAttributesValues { get; set; }
		private readonly BuildDialog buildDialog = new BuildDialog(); 

		public Settings()
		{
			KnownAttributesValues = new ObservableCollection<string>();
			DataContextChanged += OnDataContextChanged;
			InitializeComponent();
			var btn = new Button {Content = "Sort", Margin = new Thickness(2), MinWidth = 40};
			btn.Click += SortClick;
			Options.Buttons.SpPanel.Children.Add(btn);
		}

		private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			var config = (Config) e.NewValue;
			KnownAttributesValues.MergeIgnoreCase(
				config.Profiles.SelectMany(x => x.Options.Select(o => o.Value)).ToArray());
		}

		public UserControl Control
		{
			get { return this; }
		}

		private void BuildClick(object sender, RoutedEventArgs e)
		{
			try
			{
				buildDialog.DataContext = DataContext;
				buildDialog.ShowDialog(Application.Current.MainWindow, ((Profile)Profile.SelectedValue).Options, KnownAttributesValues);
				Options.Refresh();
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't build template");
			}
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			Options.OnCheckChangedEvent(sender, e);
		}

		private void SortClick(object sender, RoutedEventArgs e)
		{
			var config = (Profile) Options.DataContext;
			config.Options.Sort<Option>(
				(x,y) => string.Compare(x.Key, y.Key, StringComparison.CurrentCultureIgnoreCase), 
				config.Options.Count);
		}
	}
}