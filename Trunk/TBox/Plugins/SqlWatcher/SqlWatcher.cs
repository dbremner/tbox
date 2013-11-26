using System;
using System.Drawing;
using System.Windows;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.SqlWatcher;
using PluginsShared.Watcher;
using SqlWatcher.Code;
using WPFSyntaxHighlighter;
using WPFWinForms;
using WPFWinForms.Icons;

namespace SqlWatcher
{
	[PluginInfo(typeof(SqlWatcherLang), typeof(Properties.Resources), PluginGroup.Database)]
	public sealed class SqlWatcher : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private Lazy<Worker<LogDialog>> worker;
        private readonly DataParser parser = new DataParser();
		private static readonly string[] StartStop = {SqlWatcherLang.Start, SqlWatcherLang.Stop}; 
		public SqlWatcher()
		{
			Menu = new []{
				new UMenuItem{Header = StartStop[0], OnClick = o=>OnStartStop()},
				new UMenuItem{Header = SqlWatcherLang.ShowLogs, OnClick = o=>OnShowLogs()}, 
				new UMenuItem{Header = SqlWatcherLang.FillFromClipboard, OnClick = o=>OnFillFromClipboard()}, 
				new UMenuItem{Header = SqlWatcherLang.Clear, OnClick = o=>OnClear()}
			};
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
			worker = new Lazy<Worker<LogDialog>>(
				() => new Worker<LogDialog>(new LogDialog(Icon.ToImageSource()), parser, SqlWatcherLang.PluginName, Config, Color.Blue)); 
		}

		private void OnFillFromClipboard()
		{
		    InitWorker();
            worker.Value.ClearLogs();
		    foreach (var line in Clipboard.GetText()
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
		    {
                worker.Value.Write(SqlWatcherLang.Clipboard, parser.Parse(line));
		    }
            worker.Value.ShowLogsList();
			
		}

		private void OnClear()
		{
			worker.Value.ClearLogs();
		}

		private void OnShowLogs()
		{
			worker.Value.ShowLogsList();
		}

		private void OnStartStop()
		{
			if (Config.Started)
			{
				Menu[0].Header = StartStop[0];
				if (worker.IsValueCreated)
				{
					worker.Value.Stop();
				}
			}
			else
			{
				Menu[0].Header = StartStop[1];
				InitWorker();
			    worker.Value.Start(
					Config.RescanLogsInterval,
					Config.ToolTipsTimeOut,
					Config.Watches.Files.CheckedItems,
					Config.ToolTipsEnabled);
                worker.Value.SetMenuItems(Menu);
			}
			Config.Started = !Config.Started;
			Context.RebuildMenu();
		}

	    private void InitWorker()
        {
            parser.RemoveTypeInfo = Config.RemoveTypeInfo;
            worker.Value.Log.Dialog.Title = SqlWatcherLang.PluginName;
	        worker.Value.Log.Dialog.Format = "mssql";
	    }

	    public override void Load()
		{
			base.Load();
			Config.Started = !Config.Started;
			OnStartStop();
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (autoSaveOnExit)
			{
				if (worker.IsValueCreated) worker.Value.Save(Config);
				return;
			}
			Config.Started = !Config.Started;
			OnStartStop();
		}

		public void Dispose()
		{
			if (!worker.IsValueCreated) return;
			worker.Value.Log.Dialog.Close();
			worker.Value.Dispose();
		}
	}
}
