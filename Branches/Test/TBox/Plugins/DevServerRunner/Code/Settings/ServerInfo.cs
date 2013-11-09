using System;
using Common.UI.Model;

namespace DevServerRunner.Code.Settings
{
	[Serializable]
	public class ServerInfo : CheckableData
	{
		public int Port { get; set; }
		public string VPath { get; set; }
		public bool AdminPrivilegies { get; set; }

		public ServerInfo()
		{
			Port = 80;
			VPath = "/";
			AdminPrivilegies = false;
		}

		public override object Clone()
		{
			return new ServerInfo
			{
				IsChecked = IsChecked,
				Key = Key,
				Port = Port,
				VPath = VPath,
				AdminPrivilegies = AdminPrivilegies
			};
		}
	}
}
