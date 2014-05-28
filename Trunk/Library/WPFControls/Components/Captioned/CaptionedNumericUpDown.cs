using System;
using System.Windows;
using Mnk.Library.WpfControls.Components.DropDown;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public sealed class CaptionedNumericUpDown : CaptionedControl
	{
		private readonly NumericUpDown child = new NumericUpDown {Margin = new Thickness(0)};

		public CaptionedNumericUpDown()
		{
			child.ValueChanged += OnValueChanged;
			child.ValueChanged += (o,e) => SetValue(ValueProperty, child.Value);
			Panel.Children.Add(child);
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<CaptionedNumericUpDown, int>("Value", (s, v) => s.Value = v);
		public int Value
		{
			get
			{
				return child.Value;
			}
			set
			{
				child.Value = Math.Min(Maximum, Math.Max(Minimum, value));
				SetValue(ValueProperty, value);
			}
		}

		public int Increment
		{
			set { child.Increment = value; }
			get { return child.Increment; }
		}

		public int Maximum
		{
			set { child.Maximum = value; }
			get { return child.Maximum; }
		}

		public int Minimum
		{
			set { child.Minimum = value; }
			get { return child.Minimum; }
		}

		public new void Focus()
		{
			child.Focus();
		}
	}
}
