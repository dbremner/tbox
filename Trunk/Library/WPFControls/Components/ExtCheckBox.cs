using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components
{
	public sealed class ExtCheckBox : CheckBox
	{
		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtCheckBox));
		public event RoutedEventHandler ValueChanged
		{
			add {AddHandler(ValueChangedEvent, value);}
			remove {RemoveHandler(ValueChangedEvent, value);}
		}

		public ExtCheckBox()
		{
			Checked += ExtCheckBoxChecked;
			Unchecked += ExtCheckBoxChecked;
		}

		void ExtCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			RaiseEvent(new RoutedEventArgs(ValueChangedEvent, sender));
		}
	}
}
