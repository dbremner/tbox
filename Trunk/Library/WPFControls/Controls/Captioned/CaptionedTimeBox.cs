using System.Windows;
using WPFControls.Controls.DropDown;

namespace WPFControls.Controls.Captioned
{
	public class CaptionedTimeBox : CaptionedControl
	{
        private readonly TimeBox child = new TimeBox { Margin = new Thickness(0) };

		public CaptionedTimeBox()
		{
			child.ValueChanged += OnValueChanged;
			child.ValueChanged += (o, e) => SetValue(ValueProperty, child.Value);
			Panel.Children.Add(child);
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<CaptionedTimeBox, long>("Value", (s, v) => s.Value = v);
		public long Value
		{
			get { return child.Value; }
			set
			{
				SetValue(ValueProperty, value);
				child.Value = value;
			}
		}

		public new void Focus()
		{
			child.Focus();
		}
	}
}
