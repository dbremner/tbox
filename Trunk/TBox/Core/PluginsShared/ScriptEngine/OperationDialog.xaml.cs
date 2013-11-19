using System.Collections.Generic;
using System.Windows;
using Localization.PluginsShared;
using ScriptEngine;
using WPFControls.Tools;

namespace PluginsShared.ScriptEngine
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

		public void ShowDialog(IList<string> pathes, MultiFileOperation operation)
		{
			Owner = Application.Current.MainWindow;
			DataContext = operation;
			Pathes.ConfigureInputSelect(PluginsSharedLang.SelectSctipt, operation.Pathes, pathes);
			SafeShowDialog();
		}
	}
}
