using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Common.Tools;
using Interface;
using PluginsShared.ScriptEngine;
using ScriptEngine;
using TeamManager.Code.Scripts;
using TeamManager.Code.Settings;
using WPFControls.Code;
using WPFControls.Tools;

namespace TeamManager
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public sealed partial class Settings : ISettings, IDisposable
	{
        public LazyDialog<OperationDialog> OperationsDialog { get; set; }
        public LazyDialog<ScriptsConfigurator> ScriptsConfigurator { get; set; }

		public Settings()
		{
			InitializeComponent();
		}

		public UserControl Control { get { return this; } }
        internal Func<IList<string>> FilePathesGetter { get; set; }
        internal ReportScriptRunner ReportScriptRunner { get; set; }
        internal Config Config { get { return ((Config)DataContext); } }

        private void BtnSetScriptsClick(object sender, RoutedEventArgs e)
        {
            OperationsDialog.Value.ShowDialog(FilePathesGetter(), GetSelectedOperation(sender));
        }

        private void BtnSetParamtersClick(object sender, RoutedEventArgs e)
        {
            ScriptsConfigurator.Value.ShowDialog(GetSelectedOperation(sender), ReportScriptRunner, this.GetParentWindow());
        }

	    private Operation GetSelectedOperation(object sender)
	    {
	        var selectedKey = ((TextBlock) ((DockPanel) ((Button) sender).Parent).Children[2]).Text;
            var profile = (Profile)Profile.SelectedValue;
	        var id = profile.Operations.GetExistIndexByKeyIgnoreCase(selectedKey);
            return profile.Operations[id];
	    }

	    public void Dispose()
	    {
            OperationsDialog.Dispose();
	        ScriptsConfigurator.Dispose();
	    }

	    private void OnCheckChangedEvent(object sender, RoutedEventArgs e)
	    {
	        PersonsNames.OnCheckChangedEvent(sender, e);
	    }
	}
}
