using System;
using System.Drawing;
using System.Windows;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.RequestsWatcher;
using PluginsShared.Watcher;
using RequestsWatcher.Code;
using RequestsWatcher.Code.Parser;
using RequestsWatcher.Components;
using WPFWinForms;
using WPFWinForms.Icons;

namespace RequestsWatcher
{
	[PluginInfo(typeof(RequestsWatcherLang), 14, PluginGroup.Web)]
    public sealed class RequestsWatcher : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private Lazy<Worker<LogDialog>> worker;
        private readonly DataParser parser = new DataParser();
        private static readonly string[] StartStop = new[] { RequestsWatcherLang.Start, RequestsWatcherLang.Stop };
		public RequestsWatcher()
		{
			Menu = new []{
				new UMenuItem{Header = StartStop[0], OnClick = o=>OnStartStop()},
				new UMenuItem{Header = RequestsWatcherLang.ShowLogs, OnClick = o=>OnShowLogs()}, 
				new UMenuItem{Header = RequestsWatcherLang.FillFromClipboard, OnClick = o=>OnFillFromClipboard()}, 
				new UMenuItem{Header = RequestsWatcherLang.Clear, OnClick = o=>OnClear()}
			};
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			worker = new Lazy<Worker<LogDialog>>(
				() => new Worker<LogDialog>(new LogDialog(Icon.ToImageSource()), parser, RequestsWatcherLang.PluginName, Config, Color.Green)); 
		}

		private void OnFillFromClipboard()
		{
		    InitWorker();
            worker.Value.ClearLogs();
			foreach (var line in Clipboard.GetText().Split(new[]{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
			{
				string result;
				if (parser.TryParse(RequestsWatcherLang.Clipboard, line, out result))
				{
                    worker.Value.Write(RequestsWatcherLang.Clipboard, result);
				}
				
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
            worker.Value.Log.Title = RequestsWatcherLang.PluginName;
			worker.Value.Log.MaxEntriesInLog = Config.MaxEntriesInLog;
			parser.Reset();
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
			worker.Value.Log.Dispose();
			worker.Value.Dispose();
		}
    }
}
