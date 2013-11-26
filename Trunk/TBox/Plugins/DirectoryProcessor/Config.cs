using System;
using Common.UI.ModelsContainers;
using DirectoryProcessor.Code;

namespace DirectoryProcessor
{
	[Serializable]
	public class Config
	{
		public CheckableDataCollection<DirInfo> Directories { get; set; }

		public Config()
		{
			Directories = new CheckableDataCollection<DirInfo>
				{
					new DirInfo
						{
							IsChecked=false,
							Key = "c:\\data",
							Executable = "explorer.exe"
						}
				};
		}
	}
}
