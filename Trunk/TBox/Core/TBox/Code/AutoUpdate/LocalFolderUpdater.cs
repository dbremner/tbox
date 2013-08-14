using System;
using System.IO;
using System.Reflection;
using Common.Base.Log;
using Common.Tools;

namespace TBox.Code.AutoUpdate
{
	class LocalFolderUpdater
	{
		private static readonly ILog Log = LogManager.GetLogger<Engine>();

		public void PrepareConfigs(string configFile)
		{
			try
			{
				new FileInfo(Path.Combine(Environment.CurrentDirectory, "Config.config"))
					.MoveIfExist(configFile);
			}
			catch { }

			try
			{
				new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Config"))
					.MoveIfExist(Path.Combine(Folders.UserFolder, "Config"));
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't migrate config files");
			}
		}

		public void Update(Config config)
		{
			try
			{
				if (!config.UpdateFromSharedlFolder)return;
				if (string.IsNullOrEmpty(config.Update.Directory)) return;
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
				if (!string.IsNullOrEmpty(config.LastKnownVersion) && currentVersion <= new Version(config.LastKnownVersion)) return;
				new DirectoryInfo(config.Update.Directory)
					.CopyFilesTo(AppDomain.CurrentDomain.BaseDirectory, false);
				new DirectoryInfo(Path.Combine(config.Update.Directory, "Data"))
					.CopyFilesTo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data"));
				config.LastKnownVersion = currentVersion.ToString();
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't update TBox from shared folder");
			}	
		}
	}
}
