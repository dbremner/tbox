using System;
using System.Collections.ObjectModel;
using AppConfigManager.Code;

namespace AppConfigManager
{
	[Serializable]
	public class Config
	{
		public string SelectedProfile { get; set; }
		public string ItemSourceTemplate { get; set; }
		public string ItemResultTemplate { get; set; }
		public string DefaultValue { get; set; }
		public string LastBuildValue { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }

		public Config()
		{
			ItemSourceTemplate = "showfeature{item}";
			ItemResultTemplate = "Show.Feature.{item}";
			Profiles = new ObservableCollection<Profile>();
		}
	}
}
