using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace TeamManager.Code.Settings
{
	public class Task : CheckableData
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public CheckableDataCollection<Person> Persons { get; set; }
		public CheckableDataCollection<CheckableData> Tags { get; set; }

		public Task()
		{
			Persons = new CheckableDataCollection<Person>();
			Tags = new CheckableDataCollection<CheckableData>();
		}

		public override object Clone()
		{
			return new Task
			{
				Key = Key,
				IsChecked = IsChecked,
				Title = Title,
				Comment = Comment,
				Description = Description,
				Persons = Persons.Clone(),
				Tags = Tags.Clone()
			};
		}

	}
}
