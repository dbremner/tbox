using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public sealed class CaptionedTextBox : CaptionedControl
	{
		private readonly TextBox child = new TextBox{Margin = new Thickness(0)};

		public CaptionedTextBox()
		{
			child.TextChanged += OnValueChanged;
			child.TextChanged += (o, e) => SetValue(ValueProperty, child.Text);
			Panel.Children.Add(child);
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<CaptionedTextBox, string>("Value", (s, v) => s.Value = v);
		public string Value
		{
			get { return child.Text; }
			set
			{
				SetValue(ValueProperty, value);
				child.Text = value;
			}
		}

		public new void Focus()
		{
			child.Focus();
		}
	}
}
