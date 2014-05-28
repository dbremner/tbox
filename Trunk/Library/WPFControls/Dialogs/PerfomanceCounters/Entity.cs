using System.Collections.ObjectModel;

namespace Mnk.Library.WpfControls.Dialogs.PerfomanceCounters
{
	public class Entity
	{
		public string Title { get; set; }
		public Entity Parent { get; set; }
		public string ToolTip { get; set; }
		public ObservableCollection<string> Instances { get; set; }
		public ObservableCollection<Entity> Children { get; set; }

		public Entity()
		{
			Children = new ObservableCollection<Entity>();
			Instances = new ObservableCollection<string>();
		}
	}
}
