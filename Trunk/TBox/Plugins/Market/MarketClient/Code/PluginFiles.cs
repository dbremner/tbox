﻿using System;
using System.IO;
using System.Linq;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.SaveLoad;
using Mnk.TBox.Plugins.Market.Client.ServiceReference;

namespace Mnk.TBox.Plugins.Market.Client.Code
{
	class PluginFiles
	{
		private static readonly ILog Log = LogManager.GetLogger<PluginFiles>();

		private static string GetPluginDir(Plugin plugin)
		{
			return Path.Combine(Path.Combine(Constants.PluginsFolder, plugin.Author), plugin.Name);
		}

		public bool Save(Plugin plugin, DataContract data, Action<int> proc)
		{
			try
			{
				var dir = GetPluginDir(plugin);

				if (Directory.Exists(dir)) Directory.Delete(dir, true);
				Directory.CreateDirectory(dir);

				ExtFile.SaveDirectoryFiles(dir, data.Descriptions, data.FileByteStream,
					ExtFile.DefaultBufferSize, proc);
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error saving plugin");
				return false;
			}
			return true;
		}

		public bool Delete(Plugin plugin)
		{
			try
			{
				var dir = new DirectoryInfo(GetPluginDir(plugin));
				dir.Delete(true);
				if (dir.Parent != null)
				{
					if (dir.Parent.GetDirectories().All(x => x.Attributes == FileAttributes.Hidden))
					{
						dir.Parent.Delete();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error deleting plugin");
				return false;
			}
			return true;
		}
	}
}
