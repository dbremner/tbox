﻿using System;
using System.Windows;
using System.Windows.Controls;
using Common.Tools;
using Interface;
using Requestor.Code.Settings;
using Requestor.Components;

namespace Requestor
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ISettings
	{
		public Lazy<FormRequest> Requestor { get; set; }
		public Settings()
		{
			InitializeComponent();
		}

		private void BtnRequestClick(object sender, RoutedEventArgs e)
		{
			var selectedKey = ((Button) sender).Tag.ToString();
			var profile = (Profile)Profile.SelectedValue;
			var id = profile.Ops.GetExistIndexByKeyIgnoreCase(selectedKey);
			Requestor.Value.ShowDialog(profile.Ops[id]);
		}

		public UserControl Control {get { return this; }}
	}
}

