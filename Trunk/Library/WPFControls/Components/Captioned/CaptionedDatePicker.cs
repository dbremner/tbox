using System;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public sealed class CaptionedDatePicker : CaptionedControl
	{
        private readonly DatePicker child = new DatePicker { Margin = new Thickness(0) };

        public CaptionedDatePicker()
		{
			child.SelectedDateChanged += OnValueChanged;
            child.SelectedDateChanged += (o, e) => SetValue(ValueProperty, child.SelectedDate);
			Panel.Children.Add(child);
		}

		public static readonly DependencyProperty ValueProperty =
            DpHelper.Create<CaptionedDatePicker, DateTime?>("Value", (s, v) => s.Value = v);
		public DateTime? Value
		{
			get { return child.SelectedDate; }
			set
			{
				SetValue(ValueProperty, value);
				child.SelectedDate = value;
			}
		}

		public new void Focus()
		{
			child.Focus();
		}
	}
}
