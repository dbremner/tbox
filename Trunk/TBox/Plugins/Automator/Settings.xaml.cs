using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.Library.WPFControls.Code;
using Mnk.TBox.Plugins.Automator.Code.Settings;

namespace Mnk.TBox.Plugins.Automator
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public sealed partial class Settings : ISettings, IDisposable
	{
		private readonly LazyDialog<OperationDialog> operationsDialog = 
			new LazyDialog<OperationDialog>(()=>new OperationDialog(), "operation");
		public Settings()
		{
			InitializeComponent();
		}

		public void Dispose()
		{
			operationsDialog.Dispose();
		}

		public UserControl Control { get { return this; } }
		public Func<IList<string>> FilePathesGetter { get; set; }

		private void BtnEditClick(object sender, RoutedEventArgs e)
		{
			var selectedKey = ((TextBlock)((DockPanel)((Button)sender).Parent).Children[1]).Text;
			var profile = (Profile)Profile.SelectedValue;
			var id = profile.Operations.GetExistIndexByKeyIgnoreCase(selectedKey);
			operationsDialog.Value.ShowDialog(FilePathesGetter(), profile.Operations[id]);
		}
	}
}
