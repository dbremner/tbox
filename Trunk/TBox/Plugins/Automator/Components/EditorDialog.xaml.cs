using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Automator.Code;
using Automator.Code.Settings;
using Common.Base;
using Common.Base.Log;
using Interface;
using Localization.Plugins.Automator;
using ScriptEngine.Core;
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
		private Config config;
		private static readonly ILog Log = LogManager.GetLogger<Settings>();
		public IPluginContext Context { get; set; }
		public EditorDialog()
		{
			InitializeComponent();
		}

		public void ShowDialog(IList<string> pathes, Config config)
		{
			if (!IsVisible)
			{
				this.config = config;
				Files.ItemsSource = pathes;
				Files.IsEnabled = Files.Items.Count > 0;
				if (Files.IsEnabled && Files.SelectedIndex == -1) Files.SelectedIndex = 0;
			}
			ShowAndActivate();
		}

		private void OnSelectFile(object sender, SelectionChangedEventArgs e)
		{
			var path = Path.Combine(Context.DataProvider.ReadOnlyDataPath, e.GetNewSelection());
			ExceptionsHelper.HandleException(
				() => Source.Read(path),
				() => "Can't open file:" + path,
				Log);
			Output.Text = string.Empty;
		}

		private void BuildClick(object sender, RoutedEventArgs e)
		{
			var text = Source.Value;
			DialogsCache.ShowProgress(u=>Build(text), AutomatorLang.Building, this);
		}

		private void Build(string text)
		{
			var sw = new Stopwatch();
			sw.Start();
			Func<string> time = () => Environment.NewLine + string.Format(AutomatorLang.CompilationTimeTemplate, sw.ElapsedMilliseconds);
			try
			{
				using (var r = new Runner(config))
				{
					r.Build(text);
				}
				Mt.SetText(Output, AutomatorLang.NoErrors + time());
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
			var path = Path.Combine(Context.DataProvider.ReadOnlyDataPath, Files.Text);
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
