using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mnk.Library.Common.Tools;

namespace Mnk.Library.WPFControls.Components
{
	public class AutoComboBox : ComboBox
	{
		public AutoComboBox()
		{
			LostFocus += OnLostFocus;
			KeyDown += OnKeyDown;
		}

        public bool AutoSort { get; set; }

		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Return)
			{
				OnLostFocus(sender, e);
			}
		}

		private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
		{
			if (!IsEditable || ItemsSource == null) return;
			var collection = ItemsSource as ICollection<string>;
			if (collection == null) return;;
			var id = AddKnownValueIfNeed(collection, Text);
			if(id>=0)
			{
				SelectedIndex = id;
			}
		}

		public int AddKnownValueIfNeed(ICollection<string> collection, string value)
		{
		    if (string.IsNullOrEmpty(value)) value = string.Empty;
			value = value.Trim();
			if (string.IsNullOrEmpty(value) || collection.Contains(value)) return -1;
			if (AutoSort)
			{
				var list = collection as IList<string>;
				if (list != null)
				{
					return list.Insert(value, x => x);
				}
			}
			collection.Add(value);
			return collection.Count - 1;
		}
	}
}
