using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Components
{
	public class CheckableListBox : ExtListBox, ICheckableItemsView
	{
		public CheckableListBox()
		{
			var template = new DataTemplate { DataType = typeof(CheckableData) };
			var panel = new FrameworkElementFactory(typeof(DockPanel));
			var chb = new FrameworkElementFactory(typeof(ExtCheckBox));
			chb.SetBinding(ToggleButton.IsCheckedProperty, new Binding("IsChecked"));
			chb.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
			chb.SetValue(PaddingProperty, new Thickness(5, 0, 5, 0));
			chb.AddHandler(ExtCheckBox.ValueChangedEvent, new RoutedEventHandler(OnCheckChangedEvent));
			panel.AppendChild(chb);
			var tb = new FrameworkElementFactory(typeof(TextBlock));
			tb.SetBinding(TextBlock.TextProperty, new Binding("Key"));
			tb.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
			tb.SetValue(PaddingProperty, new Thickness(0));
			tb.SetValue(TextBlock.TextTrimmingProperty, TextTrimming.CharacterEllipsis);
			panel.AppendChild(tb);
			template.VisualTree = panel;
			ItemTemplate = template;
		}

		private ICheckableDataCollection items;

		private void RefreshAction(object sender, EventArgs e)
		{
			Refresh();
		}

		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			if (items != null) items.CollectionChanged -= RefreshAction;
			ItemsSource = newValue;
			items = (ICheckableDataCollection)newValue;
			if (items != null) items.CollectionChanged += RefreshAction;
			base.OnItemsSourceChanged(oldValue, newValue);
			Refresh();
		}

		public event RoutedEventHandler OnCheckChanged;
		public void OnCheckChangedEvent(object sender, RoutedEventArgs e)
		{
            var item = sender as CheckBox;
            if (item != null )
            {
                var selected = this.GetSelectedIds().ToArray();
                if (selected.Length > 1)
                {
                    foreach (var i in selected)
                    {
                        items.SetCheck(i, item.IsChecked.HasValue && item.IsChecked.Value);
                    }
                    Refresh();
                }
            }
			if (OnCheckChanged != null) OnCheckChanged(sender, e);
		}

		public void Refresh()
		{
			Items.Refresh();
			OnCheckChangedEvent(null, null);
		}
	}
}
