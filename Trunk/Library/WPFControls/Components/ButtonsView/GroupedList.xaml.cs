using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Mnk.Library.WpfControls.Components.ButtonsView
{
	/// <summary>
	/// Interaction logic for GroupedList.xaml
	/// </summary>
	public partial class GroupedList
	{
		public GroupedList()
		{
		    IconSize = 24;
		    CellSize = 68;
			InitializeComponent();
		}

		public void Refresh()
		{
			if(DataContext == null)return;
			var dview = (CollectionView)CollectionViewSource.GetDefaultView(ItemsSource);
		    if (dview.GroupDescriptions == null || dview.GroupDescriptions.Count != 0) return;
		    dview.GroupDescriptions.Add(new PropertyGroupDescription("GroupName"));
		    dview.SortDescriptions.Add(new SortDescription{ PropertyName = "GroupName" });
		    dview.SortDescriptions.Add(new SortDescription { PropertyName = "Order" });
		    dview.SortDescriptions.Add(new SortDescription { PropertyName = "Name" });
		}

		private void ClickItem(object sender, RoutedEventArgs e)
		{
			var fe = (FrameworkElement)sender;
			((IButtonInfo)fe.DataContext).Handler(sender, e);
		}

		private void KeyUpItem(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Space || e.Key == Key.Return)
				ClickItem(sender, e);
		}

        public int IconSize { get; set; }
        public int CellSize { get; set; }
	}
}
