using System.Windows;
using System.Windows.Input;

namespace WPFControls.Components.ButtonsView
{
	/// <summary>
	/// Interaction logic for GroupedList.xaml
	/// </summary>
	public partial class GroupedList
	{
		public GroupedList()
		{
			InitializeComponent();
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
	}
}
