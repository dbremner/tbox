using System;
using System.IO;
using System.Linq;
using Interface;
using Interface.Atrributes;
using NUnitRunner.Code.Settings;
using NUnitRunner.Components;
using NUnitRunner.Properties;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFControls.Tools;
using WPFWinForms;

namespace NUnitRunner
{
	[PluginName("NUnit tests runner")]
	[PluginDescription("Small tool to run nunit tests in parallel.")]
	public sealed class NUnitRunner : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly LazyDialog<Dialog> runner;
		private readonly LazyDialog<BatchRunDialog> batchRunner;

		public NUnitRunner()
		{
			runner = new LazyDialog<Dialog>(CreateDialog<Dialog>, "simple");
			batchRunner = new LazyDialog<BatchRunDialog>(CreateDialog<BatchRunDialog>, "batch");
		}

		private T CreateDialog<T>()
			where T : DialogWindow, new() 
		{
			var d = new T();
			d.SetIcon(Icon);
			return d;
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config.DllPathes
						 .Select(x => new UMenuItem
						 {
							 Header = Path.GetFileName(x.Key),
							 OnClick = o => Run(x)
						 })
						 .Concat(new[]
							 {
								 new USeparator(), 
								 new UMenuItem
									 {
										 IsEnabled = Config.DllPathes.Any(),
										 Header = "Batch run...",
										 OnClick = o => BatchRun(Config)
									 }
							 })
						 .ToArray();
		}

		private void BatchRun(Config config)
		{
            batchRunner.Do(Context.DoSync, d => d.ShowDialog(config, NUnitAgentPath), Config.States);
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Resources.Icon;
		}

		private void Run(TestConfig config)
		{
			runner.LoadState(Config.States);
			runner.Value.ShowDialog(config, NUnitAgentPath);
		}

		private static string NUnitAgentPath
		{
			get { return Path.Combine(Environment.CurrentDirectory, "Tools\\NUnitAgent.exe"); }
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if(!autoSaveOnExit)return;
			batchRunner.SaveState(Config.States);
			runner.SaveState(Config.States);
		}

		public void Dispose()
		{
			runner.Dispose();
			batchRunner.Dispose();
		}
	}
}
