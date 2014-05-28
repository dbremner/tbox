using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Mnk.Library.WpfControls.Components.DropDown
{
	public class TimeBox : BaseDropDownControl
	{
		private const string TimeFormat = "hh\\:mm\\:ss";
		private TimeSpan valueNumber = new TimeSpan(0,0,0);

		public TimeBox()
		{
			ValueText.MaxLength = TimeFormat.Length - 2;
			SaveValueToText();
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<TimeBox, long>("Value", OnValueChanged);
		private static void OnValueChanged(TimeBox o, long value)
		{
			o.Value = value;
			var e = new RoutedPropertyChangedEventArgs<long>(
				value, value, ValueChangedEvent);
			o.OnValueChanged(e);
		}

		public long Value
		{
			get { return valueNumber.Ticks; }
			set
			{
				valueNumber = new TimeSpan(value);
					//new TimeSpan(value.Hours, value.Minutes, value.Seconds);
				SetValue(ValueProperty, value);
				UpdateTextBlock();
			}
		}

		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
			"ValueChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<long>), typeof(TimeBox));
		public event RoutedPropertyChangedEventHandler<long> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<long> args)
		{
			RaiseEvent(args);
		}

		protected override void ChangeValue(int nSteps)
		{
			var start = ValueText.SelectionStart/3;

			var val = valueNumber.Add(new TimeSpan(start == 0 ? nSteps : 0, start == 1 ? nSteps : 0, start == 2 ? nSteps : 0));
			if (val.TotalHours >= 24) Value = new TimeSpan(23, 59, 59).Ticks;
			else if (val.TotalSeconds >= 0) Value = val.Ticks;
			else Value = 0;
		}

		private void UpdateTextBlock()
		{
			SaveValueToText();
			CheckRanges(valueNumber);
		}

		private void SaveValueToText()
		{
			var start = ValueText.SelectionStart;
			ValueText.Text = string.Format(CultureInfo.InvariantCulture, "{0:"+TimeFormat+"}", valueNumber);
			ValueText.SelectionStart = Math.Min(start, ValueText.Text.Length);
		}

		private void CheckRanges(TimeSpan v)
		{
			UpButton.IsEnabled = v.TotalHours < 24;
			DownButton.IsEnabled = v.TotalSeconds > 0;
		}

		protected override void TrySaveValue()
		{
			TimeSpan value;
			if (ParseValue(ValueText.Text, out value))
			{
				Value = value.Ticks;
			}
			else
			{
				SaveValueToText();
			}
		}

		private static bool ParseValue(string text, out TimeSpan value)
		{
			return TimeSpan.TryParseExact(text, TimeFormat, new DateTimeFormatInfo(), out value);
		}

		protected override void OnPreviewTextInput(TextCompositionEventArgs e)
		{
			e.Handled = !e.Text.All(x=>char.IsDigit(x) || x == ':');
			TimeSpan value;
			if (ParseValue(e.Text, out value))
			{
				CheckRanges(value);
			}
			base.OnPreviewTextInput(e);
		}
	}
}
