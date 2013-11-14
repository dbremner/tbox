using System;
using System.Windows;
using Common.Network;
using PluginsShared.Ddos.Statistic;
using SqlRunner.Code;
using SqlRunner.Code.Settings;
using WPFControls.Code.OS;

namespace SqlRunner.Components
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
