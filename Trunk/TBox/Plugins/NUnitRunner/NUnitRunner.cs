﻿using System;
using System.IO;
using System.Linq;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.NUnitRunner;
using NUnitRunner.Code.Settings;
using NUnitRunner.Components;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using WPFWinForms.Icons;

namespace NUnitRunner
{
	[PluginInfo(typeof(NUnitRunnerLang), typeof(Properties.Resources), PluginGroup.Development)]
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
            return new T { Icon = Icon.ToImageSource() };
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
										 Header = NUnitRunnerLang.BatchRun,
										 OnClick = o => BatchRun(Config)
									 }
							 })
						 .ToArray();
		}

		private void BatchRun(Config config)
		{
            batchRunner.Do(Context.DoSync, d => d.ShowDialog(config, NUnitAgentPath, RunAsx86Path), Config.States);
		}

		private void Run(TestConfig config)
		{
			runner.LoadState(Config.States);
            runner.Value.ShowDialog(config, NUnitAgentPath, RunAsx86Path);
		}

		private string NUnitAgentPath
		{
			get { return Path.Combine(Context.DataProvider.ToolsPath, "NUnitAgent.exe"); }
		}

        private string RunAsx86Path
		{
            get { return Path.Combine(Context.DataProvider.ToolsPath, "RunAsx86.exe"); }
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
