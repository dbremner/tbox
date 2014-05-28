using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Mnk.Library.WpfControls.Components.DropDown
{
	public class DoubleUpDown: BaseDropDownControl
	{
	    private readonly static char separator = new NumberFormatInfo().NumberDecimalSeparator.FirstOrDefault();
		private double maximum = double.MaxValue;
		private double minimum = double.MinValue;
		private double valueNumber = 0;

		public DoubleUpDown()
		{
			Increment = 1;
			UpdateTextBlock();
		}

		public double Value
		{
			get { return valueNumber; }
			set
			{
				valueNumber = Math.Max(Minimum, Math.Min(Maximum, value));
				SetValue(ValueProperty, valueNumber);
				UpdateTextBlock();
			}
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<DoubleUpDown, double>("Value", OnValueChanged);
		private static void OnValueChanged(DoubleUpDown o, double value)
		{
			o.Value = value;
			var e = new RoutedPropertyChangedEventArgs<double>(
				o.Value, value, ValueChangedEvent);
			o.OnValueChanged(e);
		}

		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
			"ValueChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<double>), typeof(DoubleUpDown));
		public event RoutedPropertyChangedEventHandler<double> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<double> args)
		{
			RaiseEvent(args);
		}

		protected override void ChangeValue(int nSteps)
		{
			Value = Math.Min(maximum, Math.Max(minimum, Value + nSteps * Increment));
		}

		private void UpdateTextBlock()
		{
			ValueText.Text = valueNumber.ToString(CultureInfo.InvariantCulture);
			CheckRanges(valueNumber);
		}

		private void CheckRanges(double v)
		{
			UpButton.IsEnabled = v < Maximum;
			DownButton.IsEnabled = v > Minimum;
		}

		public double Increment { get; set; }

		public static readonly DependencyProperty MinimumProperty =
			DpHelper.Create<DoubleUpDown, double>("Minimum", double.MinValue, (s, v) => s.Minimum = v);
		public double Minimum
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
			DpHelper.Create<DoubleUpDown, double>("Maximum", double.MaxValue, (s, v) => s.Maximum = v);
		public double Maximum
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
			double value;
			if (double.TryParse(ValueText.Text, out value))
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
			double value;
            if (double.TryParse(e.Text, out value) || (e.Text.Length == 1 && (CanAdd(e, '-') || CanAdd(e, separator))))
			{
				e.Handled = false;
				CheckRanges(value);
			}
			else e.Handled = true;
			base.OnPreviewTextInput(e);
		}

	    private bool CanAdd(TextCompositionEventArgs e, char ch)
	    {
            return ValueText.Text.Count(x => x == ch) == 0 && e.Text[0] == ch;
	    }
	}
}
