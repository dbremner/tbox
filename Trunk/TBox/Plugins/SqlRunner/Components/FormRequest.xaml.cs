﻿using System;
using System.Windows;
using Mnk.Library.Common.Network;
using Mnk.TBox.Core.PluginsShared.Ddos.Statistic;
using Mnk.TBox.Plugins.SqlRunner.Code;
using Mnk.TBox.Plugins.SqlRunner.Code.Settings;
using Mnk.Library.WPFControls.Code.OS;

namespace Mnk.TBox.Plugins.SqlRunner.Components
{
	/// <summary>
	/// </summary>
	public partial class FormRequest
	{
		private readonly BaseExecutor executor = new BaseExecutor();
		protected StatisticInfo LastResponse = null;
		private string connectionString;
		public FormRequest()
		{
			InitializeComponent();
		}

		public void ShowDialog(Op config, string connection)
		{
			connectionString = connection;
			LastResponse = null;
			Tabs.SelectedIndex = 0;
			Response.Value = string.Empty;
			Owner = Application.Current.MainWindow;
			DataContext = config;
			SafeShowDialog();
		}

		private void BtnTestClick(object sender, RoutedEventArgs e)
		{
			executor.Execute(this, (Op)DataContext, connectionString, 
				r => Mt.Do(this, 
					() =>{
							LastResponse = r;
							Response.Value = r.Status + Environment.NewLine + r.Body;
							Tabs.SelectedIndex = 1;
					}), Icon);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
