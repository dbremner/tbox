using System;
using System.Windows;
using System.Windows.Controls;
using Common.Tools;
using Interface;
using SqlRunner.Code.Settings;
using SqlRunner.Components;

namespace SqlRunner
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
			Requestor.Value.ShowDialog(profile.Ops[id], ((Config)DataContext).ConnectionString);
		}
		
		public UserControl Control {get { return this; }}
	}
}

