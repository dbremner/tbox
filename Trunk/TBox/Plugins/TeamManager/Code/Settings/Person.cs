using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;

namespace TeamManager.Code.Settings
{
	public class Person : CheckableData
	{
		public string Image { get; set; }
		public string Type { get; set; }
		public ObservableCollection<string> Tags { get; set; }

		public Person()
		{
			Tags = new ObservableCollection<string>();
		}

		public override object Clone()
		{
			return new Person
			{
				Key = Key,
				IsChecked = IsChecked,
				Image = Image,
				Tags = Tags.Clone()
			};
		}
	}
}
