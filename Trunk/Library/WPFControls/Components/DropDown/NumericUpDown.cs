using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Mnk.Library.WpfControls.Components.DropDown
{
	public class NumericUpDown : BaseDropDownControl
	{
		private int maximum = int.MaxValue;
		private int minimum = int.MinValue;
		private int valueNumber = 0;

		public NumericUpDown()
		{
			Increment = 1;
			UpdateTextBlock();
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<NumericUpDown, int>("Value", OnValueChanged);
		private static void OnValueChanged(NumericUpDown o, int value)
		{
			o.Value = value;
			var e = new RoutedPropertyChangedEventArgs<int>(
				o.Value, value, ValueChangedEvent);
			o.OnValueChanged(e);
		}
		public int Value
		{
			get { return valueNumber; }
			set
			{
				valueNumber = Math.Max(Minimum, Math.Min(Maximum, value));
				SetValue(ValueProperty, valueNumber);
				UpdateTextBlock();
			}
		}

		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
			"ValueChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));
		public event RoutedPropertyChangedEventHandler<int> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
		{
			RaiseEvent(args);
		}

		protected override void ChangeValue(int nSteps)
		{
			Value = Math.Min(maximum, Math.Max(minimum, Value + nSteps*Increment));
		}

		private void UpdateTextBlock()
		{
			ValueText.Text = valueNumber.ToString(CultureInfo.InvariantCulture);
			CheckRanges(valueNumber);
		}

		private void CheckRanges(int v)
		{
			UpButton.IsEnabled = v < Maximum;
			DownButton.IsEnabled = v > Minimum;
		}

		public int Increment { get; set; }

		public static readonly DependencyProperty MinimumProperty =
			DpHelper.Create<NumericUpDown, int>("Minimum", int.MinValue, (s, v) => s.Minimum = v);
		public int Minimum
		{
			get { return minimum; }
			set
			{
				SetValue(MinimumProperty, value);
				minimum = value;
				Value = Value;
			}
		}

		public static readonly DependencyProperty MaximumProperty =
			DpHelper.Create<NumericUpDown, int>("Maximum", int.MaxValue, (s, v) => s.Maximum = v);
		public int Maximum
		{
			get { return maximum; }
			set
			{
				SetValue(MaximumProperty, value);
				maximum = value;
				Value = Value;
			}
		}

		protected override void TrySaveValue()
		{
			int value;
			if (int.TryParse(ValueText.Text, out value))
			{
				Value = value;
			}
			else
			{
				ValueText.Text = Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			e.Handled = !e.Text.All(char.IsDigit);
			int value;
			if (int.TryParse(e.Text, out value))
			{
				CheckRanges(value);
			}
			base.OnPreviewTextInput(e);
		}
	}
}
