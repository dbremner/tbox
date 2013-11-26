using Interface;
using Interface.Atrributes;
using Localization.Plugins.Market;
using MarketClient.Code;

namespace MarketClient
{
	[PluginInfo(typeof(MarketLang), 96, PluginGroup.Other)]
	public class Market : ConfigurablePlugin<Settings, Config>
	{
		protected override void OnConfigUpdated()
		{
			Synchronizer.Init(Config.Client);
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(false);
		}
	}
}
