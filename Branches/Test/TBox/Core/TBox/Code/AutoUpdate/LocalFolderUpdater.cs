using System;
using System.IO;
using System.Reflection;
using Common.Base.Log;
using Common.Tools;

namespace TBox.Code.AutoUpdate
{
	class LocalFolderUpdater
	{
        private const string Config = "Config";
        private const string ConfigFile = "Config.config";
	    private readonly string targetPath;
	    private readonly string oldPath;
	    private static readonly ILog Log = LogManager.GetLogger<Engine>();

	    public LocalFolderUpdater(string targetPath, string oldPath)
	    {
	        this.targetPath = targetPath;
	        this.oldPath = oldPath;
	    }

	    public void PrepareConfigs()
		{
			try
			{
				new FileInfo(Path.Combine(oldPath, ConfigFile))
                    .MoveIfExist(Path.Combine(targetPath, ConfigFile));
			}
			catch { }

			try
			{
				new DirectoryInfo(Path.Combine(oldPath, Config))
					.MoveIfExist(Path.Combine(targetPath, Config));
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't migrate configuration files");
			}
		}

		public void Update(Config config)
		{
			try
			{
				if (!config.UpdateFromSharedlFolder)return;
				if (string.IsNullOrEmpty(config.Update.Directory)) return;
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
				if (currentVersion <= new Version(config.LastKnownVersion)) return;
				new DirectoryInfo(config.Update.Directory)
					.CopyFilesTo(AppDomain.CurrentDomain.BaseDirectory, false);
				config.LastKnownVersion = currentVersion.ToString();
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't update TBox from shared folder");
			}	
		}
	}
}
