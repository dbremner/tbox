using System;
using Common.Base.Log;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.TeamManager;
using TeamManager.Code;
using TeamManager.Code.Settings;
using TeamManager.Forms;
using WPFControls.Code;
using WPFWinForms;

namespace TeamManager
{
	[PluginInfo(typeof(TeamManagerLang), 160, PluginGroup.Development)]
	public class TeamManager : ConfigurablePlugin<Settings, Config>, IDisposable
	{
	    private readonly Runner runner;
	    private readonly LazyDialog<TimeReportDialog> timeReportDialog;

	    public TeamManager()
	    {
	        runner = new Runner(ConfigManager);
            timeReportDialog = new LazyDialog<TimeReportDialog>(()=>new TimeReportDialog(ConfigManager, runner), "timeReportDialog");
	    }

		public override void OnRebuildMenu()
		{
			Menu = new []
			{
				new UMenuItem
				{
					Header = TeamManagerLang.GetTimeReport,
					OnClick = GetTimeTable
				}
			};
		}

		private void GetTimeTable(object obj)
		{
            timeReportDialog.Value.ShowReportDialog();
		}

	    public void Dispose()
	    {
	        timeReportDialog.Dispose();
	    }
	}
}
