using System.Collections.Generic;
using System.Windows;
using Mnk.TBox.Locales.Localization.PluginsShared;
using Mnk.Library.ScriptEngine;
using Mnk.Library.WPFControls.Tools;

namespace Mnk.TBox.Core.PluginsShared.ScriptEngine
{
	/// <summary>
	/// Interaction logic for OperationDialog.xaml
	/// </summary>
	public partial class OperationDialog 
	{
		public OperationDialog()
		{
			InitializeComponent();
		}

		public void ShowDialog(IList<string> paths, MultiFileOperation operation)
		{
			Owner = Application.Current.MainWindow;
			DataContext = operation;
			Paths.ConfigureInputSelect(PluginsSharedLang.SelectSctipt, operation.Paths, paths);
			SafeShowDialog();
		}
	}
}
