using Interface;
using Interface.Atrributes;
using MarketClient.Code;

namespace MarketClient
{
	[PluginName("Market client")]
	[PluginDescription("Simple plugins market. Allows you to download and upload plugins\nand send feedback for its creators.")]
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
