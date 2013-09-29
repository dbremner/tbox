using Common.Base.Log;
using Interface;
using Interface.Atrributes;
using TeamManager.Code.ProjectManagers;
using TeamManager.Code.Settings;
using WPFWinForms;

namespace TeamManager
{
	[PluginName("Team manager")]
	[PluginDescription("Plugin to simplify team managment.")]
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

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(43);
		}
	}
}
