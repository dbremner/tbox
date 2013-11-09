﻿using System;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;
using Interface;
using WPFControls.Code.OS;

namespace TBox.Code.AutoUpdate
{
	public class ApplicationUpdater : IAutoUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<ApplicationUpdater>();
        private readonly IConfigManager<Config> cm;
		private readonly IApplicationUpdater applicationUpdater;

		public ApplicationUpdater(IConfigManager<Config> cm, IApplicationUpdater applicationUpdater)
		{
			this.cm = cm;
			this.applicationUpdater = applicationUpdater;
		}

		public bool TryUpdate(bool manual = false)
		{
			if (!manual && cm.Config.Update.Interval == UpdateInterval.Never) return false;
			if (string.IsNullOrWhiteSpace(cm.Config.Update.Directory)) return false;
			try
			{
				if (applicationUpdater.NeedUpdate() &&
                    (manual || Merger.CheckDate(cm.Config.Update.Last, cm.Config.Update.Interval)))
				{
					applicationUpdater.Update();
				}
				else return false;
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't update application.");
				return false;
			}
            cm.Config.Update.Last = DateTime.Now;
            Mt.Do(Application.Current.MainWindow, DoExit);
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