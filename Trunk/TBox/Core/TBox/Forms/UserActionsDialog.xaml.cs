using System.Windows;
using TBox.Code.FastStart.Settings;
using TBox.Code.Menu;
using WPFControls.Tools;

namespace TBox.Forms
{
	/// <summary>
	/// Interaction logic for OperationDialog.xaml
	/// </summary>
	public partial class UserActionsDialog 
	{
		public UserActionsDialog()
		{
			InitializeComponent();
		}

		public void ShowDialog(MenuItemsSequence operation, IMenuItemsProvider menuItemsProvider)
		{
			Owner = Application.Current.MainWindow;
			DataContext = operation;
			Pathes.ConfigureInputMenuItem("Select menu actions", 
				operation.MenuItems, 
				menuItemsProvider.GetDialogItems());
			SafeShowDialog();
		}
	}
}
