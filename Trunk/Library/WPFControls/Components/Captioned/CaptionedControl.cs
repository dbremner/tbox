using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public class CaptionedControl : UserControl
	{
		private readonly TextBlock lCaption = new TextBlock { Padding = new Thickness(0), Visibility = Visibility.Collapsed};
		protected DockPanel Panel { get; private set; }
		public CaptionedControl()
		{
			Panel = new DockPanel();
			DockPanel.SetDock(lCaption, Dock.Top);
			Panel.Children.Add(lCaption);
			Content = Panel;
		}

		public string Caption
		{
			get { return lCaption.Text; }
			set
			{
				lCaption.Text = value;
			    lCaption.Visibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
			}
		}

		public event RoutedEventHandler ValueChanged;

		protected void OnValueChanged(object sender, RoutedEventArgs e)
		{
			if (ValueChanged != null) ValueChanged(sender, e);
		}

	}
}
