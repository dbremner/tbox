using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace TeamManager.Code.Settings
{
	public class Project : CheckableData
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public CheckableDataCollection<Person> Persons { get; set; }

		public Project()
		{
			Persons = new CheckableDataCollection<Person>();
		}

		public override object Clone()
		{
			return new Project
			{
				Key = Key,
				IsChecked = IsChecked,
				Title = Title,
				Comment = Comment,
				Description = Description,
				Persons = Persons.Clone()
			};
		}
	}
}
