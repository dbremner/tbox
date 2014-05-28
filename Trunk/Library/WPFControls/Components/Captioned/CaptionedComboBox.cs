using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Components.Captioned
{
	public sealed class CaptionedComboBox : CaptionedControl
	{
        private readonly ComboBox child = new AutoComboBox { Margin = new Thickness(0) };

		public CaptionedComboBox()
		{
			child.KeyUp += OnValueChanged;
			child.SelectionChanged += OnSelectionChanged;
			Panel.Children.Add(child);
		}

		public ComboBox Child
		{
			get { return child; }
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DpHelper.Create<CaptionedComboBox, IEnumerable>("ItemsSource", (s, v) => s.ItemsSource = v);
		public IEnumerable ItemsSource
		{
			get { return child.ItemsSource; }
			set
			{
				SetValue(ItemsSourceProperty, value);
				child.ItemsSource = value;
				if (!IsEditable) child.IsEnabled = child.Items.Count > 0;
				child.Items.Refresh();
			}
		}

		public ItemCollection Items
		{
			get { return child.Items; }
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<CaptionedComboBox, string>("Value", (s, v) => s.Value = v);
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

		public static readonly DependencyProperty SelectedIndexProperty =
			DpHelper.Create<CaptionedComboBox, int>("SelectedIndex", (s, v) => s.SelectedIndex = v);
		public int SelectedIndex
		{
			get { return child.SelectedIndex; }
			set
			{
				SetValue(SelectedIndexProperty, value);
				child.SelectedIndex = value;
			}
		}

		public bool IsEditable
		{
			get { return child.IsEditable; }
			set { child.IsEditable = value; }
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
			SetValue(SelectedIndexProperty, child.SelectedIndex);
			if (SelectedIndexChanged != null) SelectedIndexChanged(sender, e);
		}
	}
}
