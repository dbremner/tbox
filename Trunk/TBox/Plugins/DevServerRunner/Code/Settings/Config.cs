using System;
using Common.UI.ModelsContainers;

namespace DevServerRunner.Code.Settings
{
	[Serializable]
	public class Config 
	{
		public string PathToDevServer { get; set; }
		public string PathToBrowser { get; set; }
		public bool RunBrowser { get; set; }

		public CheckableDataCollection<ServerInfo> ServerInfos { get; set; }

		public Config()
		{
			PathToDevServer = @"c:\Program Files\Common Files\microsoft shared\DevServer\11.0\WebDev.WebServer40.EXE";
			PathToBrowser = "iexplore";
			RunBrowser = true;
			ServerInfos = new CheckableDataCollection<ServerInfo>();
		}
	}
}
