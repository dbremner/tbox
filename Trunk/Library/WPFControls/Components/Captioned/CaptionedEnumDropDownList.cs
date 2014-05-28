using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public class CaptionedEnumDropDownList : CaptionedControl
	{
		private readonly ComboBox child = new ComboBox
			                                  {
				                                  Margin = new Thickness(0),
												  IsEditable = false,
			                                  };

		public CaptionedEnumDropDownList()
		{
			child.KeyUp += OnValueChanged;
			child.SelectionChanged += OnSelectionChanged;
			Panel.Children.Add(child);
		}

		public ComboBox Child
		{
			get { return child; }
		}

		public static readonly DependencyProperty SourceEnumTypeProperty =
			DpHelper.Create<CaptionedEnumDropDownList, Type>("SourceEnumType", (s, v) => s.SourceEnumType = v);
		public Type SourceEnumType
		{
			get
			{
				return (Type)GetValue(SourceEnumTypeProperty);
			}
			set
			{
				SetValue(SourceEnumTypeProperty, value);
				child.ItemsSource = Enum.GetValues(value).Cast<object>().Select(x => x.ToString());
			    if (Value != null) Value = Value;
			}
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<CaptionedEnumDropDownList, string>("Value", (s, v) => s.Value = v);
		public string Value
		{
			get
			{
				return (child.SelectedIndex == -1) ?
					child.Text :
					child.SelectedItem.ToString();
			}
			set
			{
				SetValue(ValueProperty, value);
				var id = child.Items.IndexOf(value);
				if (id != -1)
				{
					child.SelectedIndex = id;
				}
				else
				{
					child.Text = value;
				}
			}
		}

		public new void Focus()
		{
			child.Focus();
		}

		public event RoutedEventHandler SelectedIndexChanged;
		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				SetValue(ValueProperty, e.AddedItems[0].ToString());
			}
			if (SelectedIndexChanged != null) SelectedIndexChanged(sender, e);
		}
	}
}
