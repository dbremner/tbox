using System.Collections.Generic;
using System.Windows;
using Automator.Code.Settings;
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
			Pathes.ConfigureInputSelect("Select sctipt", operation.Pathes, pathes);
			SafeShowDialog();
		}
	}
}
