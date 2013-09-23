using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Input;
using Common.MT;
using Common.Tools;
using Interface;
using WPFControls.Code.Log;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;

namespace PluginsShared.Watcher
{
	public sealed class Worker<TLogDialog> : IDisposable
		where TLogDialog : ILogDialog
	{
		private const string DialogId = "results";
		private readonly TrayIcon trayIcon = new TrayIcon();
		private readonly string workerId;
		private readonly WatchEngine watcher;
		private readonly SafeTimer timer;
		private readonly IconWithText iconWithText;
		private readonly LogCache logCache;
        private TrayLog trayLog = null;
		public TLogDialog Log { get; private set; }

		public Worker(TLogDialog loggedList, IDataParser dataParser, string workerId, IConfigWithDialogStates config, Color background = default(Color))
		{
			iconWithText = new IconWithText(background: background);
			Log = loggedList;
			Log.OnClear += (o,e) => loggedList_OnDataChanged(null,null);
			logCache = new LogCache(Log, loggedList_OnDataChanged);
			this.workerId = workerId;
			watcher = new WatchEngine(workerId, dataParser);
			trayIcon.MouseClick += b => { if (b == MouseButton.Left)ShowLogsList(); };
			timer = new SafeTimer(Work);
			Log.DialogWindow.SetState(config.States.Get(DialogId));
			loggedList_OnDataChanged(null, null);
		}

		public void SetMenuItems(IList<UMenuItem> items)
		{
			trayIcon.SetMenuItems(items, false);
		}

		private void loggedList_OnDataChanged(string caption, string text)
		{
			iconWithText.Create(Log.EntriesCount.ToString(CultureInfo.InvariantCulture));
			trayIcon.Icon = iconWithText.Icon;
			trayIcon.HoverText = workerId + ". Entries count: " + Log.EntriesCount + ".";
			var log = trayLog;
			if (log != null && text!=null)
			{
				log.Write(caption, text);
			}
		}

		private void Work()
		{
			watcher.CheckFiles();
		}

		public void Start(int rescanTime, int toolTipsTimeOut, IEnumerable<DirInfo> dirs, bool toolTipsEnabled )
		{
			Stop();
			if(toolTipsEnabled)
			{
				trayLog = new TrayLog(trayIcon, toolTipsTimeOut);
			}
			watcher.Start(dirs, new MultiCaptionedLog(new ICaptionedLog[] { logCache }));
			trayIcon.IsVisible = true;
			logCache.Start();
			timer.Start(rescanTime);
		}

		public void Stop()
		{
			trayLog = null;
			logCache.Stop();
			timer.Stop();
			trayIcon.IsVisible = false;
			watcher.Stop();
		}

		public void ShowLogsList()
		{
			Log.ShowLogs();
            logCache.Refresh();
		}

		public void Save(IConfigWithDialogStates config)
		{
			config.States[DialogId] = Log.DialogWindow.GetState();
		}

        public void Write(string caption, string text)
        {
            logCache.Write(caption, text);
        }

		public void Dispose()
		{
			Stop();
			timer.Dispose();
			trayIcon.Dispose();
			iconWithText.Dispose();
		}

		public void ClearLogs()
		{
			Log.Clear();
		}
	}
}
