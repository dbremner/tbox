﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
	public sealed partial class Settings : ISettings, IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger<Settings>();
		public ObservableCollection<string> KnownAttributesValues { get; set; }
		private readonly BuildDialog buildDialog = new BuildDialog(); 

		public Settings()
		{
			KnownAttributesValues = new ObservableCollection<string>();
			DataContextChanged += OnDataContextChanged;
			InitializeComponent();
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

		public void Dispose()
		{
			buildDialog.Dispose();
		}
	}
}