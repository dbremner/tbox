using System.Collections.Generic;
using System.Windows;
using Localization.Plugins.Automator;
using ScriptEngine;
using WPFControls.Tools;

namespace Automator.Components
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

		public void ShowDialog(IList<string> pathes, Operation operation)
		{
			Owner = Application.Current.MainWindow;
			DataContext = operation;
			Pathes.ConfigureInputSelect(AutomatorLang.SelectSctipt, operation.Pathes, pathes);
			SafeShowDialog();
		}
	}
}
