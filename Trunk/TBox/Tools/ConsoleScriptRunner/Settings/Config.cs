using System;
using System.Collections.ObjectModel;

namespace ConsoleScriptRunner.Settings
{
	[Serializable]
	public sealed class Config
	{
		public string SelectedProfile { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }

		public Config()
		{
			Profiles = new ObservableCollection<Profile>();
		}
	}
}
