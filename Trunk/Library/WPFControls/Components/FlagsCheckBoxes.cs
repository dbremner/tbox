using System;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WPFControls.Components
{
	public class FlagsCheckBoxes : UserControl
	{
		private readonly object locker = new object();
		private int flagValues = int.MinValue;
		private Type baseType = null;
		private readonly GroupBox groupBox = new GroupBox();
		private readonly StackPanel panel = new StackPanel();
		public FlagsCheckBoxes()
		{
			Content = groupBox;
			groupBox.Content = new ScrollViewer
			{
				Content = panel,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			};
		}

		public string Caption
		{
			get { return groupBox.Header.ToString(); }
			set { groupBox.Header = value; }
		}

		public Orientation Orientation
		{
			get { return panel.Orientation; }
			set { panel.Orientation = value; }
		}

		public Type BaseType
		{
			get { return baseType; }
			set
			{
				lock(locker)
				{
					baseType = value;
					RefreshData();
				}
			}
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<FlagsCheckBoxes, int>("Value", int.MinValue, OnValueChanged);
		private static void OnValueChanged(FlagsCheckBoxes o, int value)
		{
			o.Value = value;
			var e = new RoutedPropertyChangedEventArgs<int>(
				value, value, ValueChangedEvent);
			o.OnValueChanged(e);
		}

		public int Value
		{
			get { return flagValues; }
			set
			{
				lock (locker)
				{
					flagValues = value;
					SetValue(ValueProperty, value);
					RefreshData();
				}
			}
		}

		public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
			"ValueChanged", RoutingStrategy.Bubble,
			typeof(RoutedPropertyChangedEventHandler<int>), typeof(FlagsCheckBoxes));
		public event RoutedPropertyChangedEventHandler<int> ValueChanged
		{
			add { AddHandler(ValueChangedEvent, value); }
			remove { RemoveHandler(ValueChangedEvent, value); }
		}

		protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
		{
			RaiseEvent(args);
		}

		private void RefreshData()
		{
			panel.Children.Clear();
			if(baseType == null || flagValues == int.MinValue)return;
			foreach (var value in Enum.GetValues(baseType))
			{
				var el = new StackPanel {Margin = new Thickness(2), Orientation = Orientation.Horizontal};
				var chb = new ExtCheckBox {HorizontalAlignment = HorizontalAlignment.Center};
				el.Children.Add(chb);
				el.Children.Add(new TextBlock { Text = Enum.GetName(baseType, value), HorizontalAlignment = HorizontalAlignment.Center});
				panel.Children.Add(el);
				var local = (int)value;
				chb.IsChecked = (flagValues & local) != 0;
				chb.Checked += (o, e) => Value |= local;
				chb.Unchecked += (o, e) => Value &= ~local;
			}
		}
	}
}
