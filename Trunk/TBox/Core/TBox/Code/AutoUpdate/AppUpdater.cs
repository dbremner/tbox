using System;
using System.Diagnostics;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;
using WPFControls.Code.OS;

namespace TBox.Code.AutoUpdate
{
	public class AppUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<AppUpdater>();
		private readonly Window owner;
		private readonly Config config;

		public AppUpdater(Window owner, Config config)
		{
			this.owner = owner;
			this.config = config;
		}

		public bool TryUpdate(bool manual = false)
		{
			if (!manual && config.Update.Interval == UpdateInterval.Never) return false;
			if (string.IsNullOrWhiteSpace(config.Update.Directory)) return false;
			var updater = new DirectoryApplicationUpdater(config.Update.Directory);
			try
			{
				if(Merger.CheckNeedUpdate(updater) && 
					(manual || Merger.CheckDate(config.Update.Last, config.Update.Interval)))
				{
					Merger.Merge(updater);
				}
				else return false;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't update application.");
				return false;
			}
			config.Update.Last = DateTime.Now;
			OneInstance.App.AfterExit += AfterExit;
			Mt.Do(owner, DoExit);
			return true;
		}

		private static void AfterExit(object sender, EventArgs e)
		{
			using(Process.Start(Process.GetCurrentProcess().MainModule.FileName)){}
		}

		private static void DoExit()
		{
			var w = Application.Current.MainWindow as MainWindow;
			if (w != null)
			{
				w.MenuClose();
			}
			Application.Current.Shutdown();
		}
	}
}
