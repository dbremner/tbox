using System;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;
using WPFControls.Code.OS;

namespace TBox.Code.AutoUpdate
{
	public class ApplicationUpdater : IAutoUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<ApplicationUpdater>();
		private readonly Window owner;
		private readonly Config config;
		private readonly IUpdater updater;

		public ApplicationUpdater(Window owner, Config config, IUpdater updater)
		{
			this.owner = owner;
			this.config = config;
			this.updater = updater;
		}

		public bool TryUpdate(bool manual = false)
		{
			if (!manual && config.Update.Interval == UpdateInterval.Never) return false;
			if (string.IsNullOrWhiteSpace(config.Update.Directory)) return false;
			try
			{
				if (updater.NeedUpdate() && 
					(manual || Merger.CheckDate(config.Update.Last, config.Update.Interval)))
				{
					updater.Update();
				}
				else return false;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't update application.");
				return false;
			}
			config.Update.Last = DateTime.Now;
			Mt.Do(owner, DoExit);
			return true;
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
