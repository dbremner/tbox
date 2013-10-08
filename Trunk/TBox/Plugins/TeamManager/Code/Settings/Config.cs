using System.Collections.ObjectModel;
using Common.UI.ModelsContainers;

namespace TeamManager.Code.Settings
{
	public class Config
	{
		public CheckableDataCollection<Person> Persons { get; set; }
		public ObservableCollection<string> KnownTags { get; set; }
		public ObservableCollection<string> KnownTypes { get; set; }
		public string UserEmail { get; set; }
		public string UserPassword { get; set; }
		public string ProjectManagerUrl { get; set; }

		public Config()
		{
			Persons = new CheckableDataCollection<Person>();
			KnownTags = new ObservableCollection<string>{"UI", "Service", "DB"};
			KnownTypes = new ObservableCollection<string> {"Dev", "QA", "BA"};
			ProjectManagerUrl = "http://targetprocess.com";
		}
	}
}
