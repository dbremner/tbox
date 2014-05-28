using System.Windows;
using System.Windows.Media;

namespace Mnk.Library.WpfControls.Components.ButtonsView
{
	public interface IButtonInfo
	{
		string Name { get; }
		ImageSource Icon { get; }
		RoutedEventHandler Handler { get; }
		string GroupName { get; }
        int Order { get; }
	}

	public class ButtonInfo : IButtonInfo
	{
		public string Name { get; set; }
		public string GroupName { get; set; }
		public ImageSource Icon { get; set; }
		public RoutedEventHandler Handler { get; set; }
        public int Order { get; set; }
	}

}
