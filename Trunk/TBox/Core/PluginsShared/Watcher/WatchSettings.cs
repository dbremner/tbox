using System;
using Common.UI.ModelsContainers;

namespace PluginsShared.Watcher
{
	[Serializable]
	public sealed class WatchSettings
	{
		public CheckableDataCollection<DirInfo> Files { get; set; }
		public WatchSettings()
		{
			Files = new CheckableDataCollection<DirInfo>();
		}
	}
}
