using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Common.Tools;
using Interface;
using Localization.Plugins.TeamManager;
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
        public LazyDialog<ScriptsConfigurator> ScriptsConfigurator { get; set; }

		public Settings()
		{
			InitializeComponent();
		}

		public UserControl Control { get { return this; } }
        public IList<string> FilePathes { get; set; }
        internal IScriptConfigurator ScriptConfigurator { get; set; }
        internal Config Config { get { return ((Config)DataContext); } }

        private void BtnSetParamtersClick(object sender, RoutedEventArgs e)
        {
            var op = GetSelectedOperation(sender);
            if (string.IsNullOrEmpty(op.Path))
            {
                MessageBox.Show(TeamManagerLang.PleaseSpecifyScriptPath);
                return;
            }
            ScriptsConfigurator.Value.ShowDialog(op, ScriptConfigurator, this.GetParentWindow());
        }

	    private SingleFileOperation GetSelectedOperation(object sender)
	    {
	        var selectedKey = ((TextBlock) ((DockPanel) ((Button) sender).Parent).Children[3]).Text;
            var profile = (Profile)Profile.SelectedValue;
	        var id = profile.Operations.GetExistIndexByKeyIgnoreCase(selectedKey);
            return profile.Operations[id];
	    }

	    public void Dispose()
	    {
	        ScriptsConfigurator.Dispose();
	    }
	}
}
