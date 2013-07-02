using System;
using System.Text;
using System.Windows;
using FileWatcher.Code;
using Interface;
using Interface.Atrributes;
using PluginsShared.Watcher;
using WPFWinForms;

namespace FileWatcher
{
	[PluginName("File watcher")]
	[PluginDescription("Ability to watch for the set of logs at real time.\nAnd see all the information in one stream with tray indicator.")]
	public sealed class FileWatcher : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private LogDialog dialog;
		private const string Title = "File Watcher";
		private Lazy<Worker<LogDialog>> worker; 
		private static readonly string[] StartStop = new[] {"Start", "Stop"}; 
		public FileWatcher()
		{
			Menu = new []{
				new UMenuItem{Header = StartStop[0], OnClick = o=>OnStartStop()},
				new UMenuItem{Header = "Show Logs...", OnClick = o=>OnShowLogs()}, 
				new UMenuItem{Header = "Fill From Clipboard...", OnClick = o=>OnFillFromClipboard()}, 
				new UMenuItem{Header = "Clear", OnClick = o=>OnClear()}
			};
		}

		private void OnFillFromClipboard()
		{
			worker.Value.ClearLogs();
			InitLogDialog();
			worker.Value.Write("clipboard", Clipboard.GetText());
			worker.Value.ShowLogsList();
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Context.GetSystemIcon(55);
			worker = new Lazy<Worker<LogDialog>>(
				() => new Worker<LogDialog>(dialog = new LogDialog(Icon), new DataParser(), Title, Config)); 
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
				InitLogDialog();
				worker.Value.Start(
					Config.RescanLogsInterval,
					Config.ToolTipsTimeOut,
					Config.Watches.Files.CheckedItems,
					Config.ToolTipsEnabled
					);
				worker.Value.SetMenuItems(Menu);
			}
			Config.Started = !Config.Started;
			Context.RebuildMenu();
		}

		private void InitLogDialog()
		{
			worker.Value.Log.Dialog.Title = Title;
			worker.Value.Log.Dialog.MaxEntries = Config.MaxEntriesInLog;
			dialog.Init(Config.EntryRegularExpression);
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
				if(worker.IsValueCreated)worker.Value.Save(Config);
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
