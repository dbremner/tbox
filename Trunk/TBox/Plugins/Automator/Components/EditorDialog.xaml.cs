using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Common.Base;
using Common.Base.Log;
using Interface;
using ScriptEngine.Core;
using ScriptEngine.Core.Interfaces;
using WPFControls.Code.OS;
using WPFControls.Dialogs;
using WPFControls.Tools;

namespace Automator.Components
{
	/// <summary>
	/// Interaction logic for EditorDialog.xaml
	/// </summary>
	public partial class EditorDialog 
	{
		private readonly ICompiler compiler = new ScriptCompiler();
		private static readonly ILog Log = LogManager.GetLogger<Settings>();
		public IPluginContext Context { get; set; }
		public EditorDialog()
		{
			InitializeComponent();
		}

		public void ShowDialog(IList<string> pathes)
		{
			if (!IsVisible)
			{
				Files.ItemsSource = pathes;
				Files.IsEnabled = Files.Items.Count > 0;
				if (Files.IsEnabled && Files.SelectedIndex == -1) Files.SelectedIndex = 0;
			}
			ShowAndActivate();
		}

		private void OnSelectFile(object sender, SelectionChangedEventArgs e)
		{
			var path = Path.Combine(Context.DataProvider.DataPath, e.GetNewSelection());
			ExceptionsHelper.HandleException(
				() => Source.Value = File.ReadAllText(path),
				() => "Can't open file:" + path,
				Log);
			Output.Text = string.Empty;
		}

		private void BuildClick(object sender, RoutedEventArgs e)
		{
			var text = Source.Value;
			DialogsCache.ShowProgress(u=>Build(text), "Building...", this);
		}

		private void Build(string text)
		{
			var sw = new Stopwatch();
			sw.Start();
			Func<string> time = ()=>Environment.NewLine + "Compilation time: " + sw.ElapsedMilliseconds + "ms.";
			try
			{
				compiler.Build(text);
				Mt.SetText(Output, "No errors" + time());
			}
			catch (CompilerExceptions cex)
			{
				Mt.SetText(Output, cex + time());
			}
			catch (Exception ex)
			{
				Mt.SetText(Output, ExceptionsHelper.Expand(ex) + time());
			}
		}

		private void SaveClick(object sender, RoutedEventArgs e)
		{
			var path = Path.Combine(Context.DataProvider.DataPath, Files.Text);
			ExceptionsHelper.HandleException(
				() => File.WriteAllText(path, Source.Value, Encoding.UTF8),
				() => "Can't save file:" + path,
				Log);
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
