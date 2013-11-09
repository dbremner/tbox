using System;
using System.Globalization;
using System.IO;
using Common.SaveLoad;
using Interface;
using TBox.Code.AutoUpdate;
using WPFControls.Localization;

namespace TBox.Code.Configs
{
	class ConfigManager : IConfigManager<Config>, IConfigsManager
	{
		public Config Config { get; private set; }
		private ParamSerializer<Config> paramSer;
		private const string ConfigFileName = "Config.config";
	    private readonly string localFolder = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string userFolder = Folders.UserFolder;

		public ConfigManager()
		{
            var localConfig = Path.Combine(localFolder, ConfigFileName);
            var userConfig = Path.Combine(userFolder, ConfigFileName);
            if (!TryLoad(localConfig, userConfig, true))
            {
                if (!TryLoad(userConfig, localConfig, false))
                {
                    paramSer = new ParamSerializer<Config>(userConfig);
                    Config = paramSer.Load(Config = new Config());
                }
            }
			var localUpdater = CreateUpdater();
			localUpdater.PrepareConfigs();
			
			Translator.Culture = new CultureInfo(Config.Language);
			localUpdater.Update(Config);
		}

	    private LocalFolderUpdater CreateUpdater()
	    {
	        return Config.Configuration.PortableMode ? 
                new LocalFolderUpdater(Root = localFolder, userFolder) :
                new LocalFolderUpdater(Root = userFolder, localFolder);
	    }

	    private bool TryLoad(string targetConfig, string alternativeConfig, bool portability)
        {
            if (File.Exists(targetConfig))
            {
                paramSer = new ParamSerializer<Config>(targetConfig);
                Config = paramSer.Load(Config = new Config());
                if (Config.Configuration.PortableMode != portability)
                {
                    paramSer = new ParamSerializer<Config>(alternativeConfig);
                }
                return true;
            }
            return false;
        }

		public void Save()
		{
			paramSer.Save(Config);
		}

		public void Load()
		{
			Config = paramSer.Load(Config = new Config());
		}

	    public string Root { get; private set; }
	}
}
