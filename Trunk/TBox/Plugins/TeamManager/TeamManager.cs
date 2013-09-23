using Common.Base.Log;
using Interface;
using Interface.Atrributes;
using TeamManager.Code.Settings;

namespace TeamManager
{
	[PluginName("Team manager")]
	[PluginDescription("Plugin to simplify team managment.")]
	public class TeamManager : ConfigurablePlugin<Settings, Config>
	{
		private static readonly ILog Log = LogManager.GetLogger<TeamManager>();
	
		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(43);
		}
	}
}
