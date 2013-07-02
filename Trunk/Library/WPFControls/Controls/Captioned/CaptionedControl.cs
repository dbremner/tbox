using System.Windows;
using System.Windows.Controls;

namespace WPFControls.Controls.Captioned
{
	public class CaptionedControl : UserControl
	{
		private readonly TextBlock lCaption = new TextBlock { Padding = new Thickness(5, 0, 5, 0) };
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
			}
		}

		public event RoutedEventHandler ValueChanged;

		protected void OnValueChanged(object sender, RoutedEventArgs e)
		{
			if (ValueChanged != null) ValueChanged(sender, e);
		}

	}
}
