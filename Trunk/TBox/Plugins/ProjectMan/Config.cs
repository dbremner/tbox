using System;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace ProjectMan
{
	[Serializable]
	public class Config
	{
		public string PathToMsBuild { get; set; }
		public string PathToSvn { get; set; }
		public string SvnUserName { get; set; }
		public CheckableDataCollection<CheckableData> Dirs { get; set; }

		public Config()
		{
			PathToMsBuild = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe";
			PathToSvn = @"C:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";
			SvnUserName = "mnk";
			Dirs = new CheckableDataCollection<CheckableData>();

		}
	}
}
