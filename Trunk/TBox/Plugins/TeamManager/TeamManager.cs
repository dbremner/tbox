using Common.Base.Log;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.TeamManager;
using TeamManager.Code.ProjectManagers;
using TeamManager.Code.Settings;
using WPFWinForms;

namespace TeamManager
{
	[PluginInfo(typeof(TeamManagerLang), 43, PluginGroup.Development)]
	public class TeamManager : ConfigurablePlugin<Settings, Config>
	{
		private static readonly ILog Log = LogManager.GetLogger<TeamManager>();


		public override void OnRebuildMenu()
		{
			Menu = new []
			{
				new UMenuItem
				{
					Header = "GetMyTimeTable",
					OnClick = GetTimeTable
				}
			};

		}

		private void GetTimeTable(object obj)
		{
			var facade = new TargetProcessFacade(Config.UserEmail, Config.UserPassword, Config.ProjectManagerUrl);
			facade.GetAllUserStories();
		}
	}
}
