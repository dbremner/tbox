using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFControls.Components
{
	public class ExtListBox : ListBox
	{
		public ExtListBox()
		{
            SelectionMode = SelectionMode.Single;
			AlternationCount = 2;
			HorizontalContentAlignment = HorizontalAlignment.Stretch;
			SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);

            EventManager.RegisterClassHandler(typeof(ListBoxItem),
                GotKeyboardFocusEvent,
                new RoutedEventHandler(OnListBoxItemContainerFocused));
		}

	    public void OnListBoxItemContainerFocused(object sender, RoutedEventArgs e)
	    {
	        var item = sender as ListBoxItem;
	        if (item == null || item.IsSelected) return;
	        if (!Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
	        {
	            if (SelectionMode != SelectionMode.Single)
	            {
	                SelectedItems.Clear();
	            }
	        }
	        item.IsSelected = true;
	    }
	}
}
