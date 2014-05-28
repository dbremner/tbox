using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Dialogs.Menu
{
	public class MenuDialogItem
	{
		public ImageSource Icon { get; set; }
		public string Name { get; private set; }
		public MenuDialogItem Parent { get; private set; }
		public ObservableCollection<MenuDialogItem> Children { get; private set; }

		public MenuDialogItem(string name, MenuDialogItem parent)
		{
			Name = name;
			Parent = parent;
			Children = new ObservableCollection<MenuDialogItem>();
		}
	}
}
