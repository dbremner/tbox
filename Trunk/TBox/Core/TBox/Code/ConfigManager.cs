using System.Globalization;
using System.IO;
using Common.SaveLoad;
using TBox.Code.AutoUpdate;
using WPFControls.Localization;

namespace TBox.Code
{
	class ConfigManager
	{
		public Config Config { get; private set; }
		private readonly ParamSerializer<Config> paramSer;
		private readonly string configFile = Path.Combine(Folders.UserFolder, "Config.config");

		public ConfigManager()
		{
			var localUpdater = new LocalFolderUpdater();
			localUpdater.PrepareConfigs(configFile);
			paramSer = new ParamSerializer<Config>(configFile);
			Config = paramSer.Load(Config = new Config());
			Translator.Culture = new CultureInfo(Config.Language);
			localUpdater.Update(Config);
		}

		public void Save()
		{
			paramSer.Save(Config);
		}

		public void Load()
		{
			Config = paramSer.Load(Config = new Config());
		}
	}
}
