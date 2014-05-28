using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Components.EditButtons;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Components.Units
{
	public sealed class ComboBoxUnit : BaseCollectionUnit
	{
		public ComboBoxUnit()
		{
			Init();
		}

		protected override Selector CreateItems()
		{
			return new ComboBox{Margin = new Thickness(5), TabIndex = 0};
		}

		protected override EditButtonsPanel CreateEditButtonsPanel()
		{
			var p = new EditButtonsPanel { Margin = new Thickness(3), Orientation = Orientation.Horizontal, TabIndex = 1 };
			DockPanel.SetDock(p, Dock.Right);
			return p;
		}

		protected override UnitConfigurator CreateConfigurator()
		{
			return new UnitConfigurator(this, () =>
			{
				ItemsOnSelectionChanged(Items, null);
				ValueCaption = (string)GetValue(ValueCaptionProperty);
			});
		}

		protected override void ItemsOnSelectionChanged(object o, EventArgs e)
		{
			var selected = ((ComboBox)o).SelectedItem;
			Items.IsEnabled = Items.Items.Count > 0;
			if (Items.IsEnabled && selected != null)
			{
				SetValue(ValueCaptionProperty, selected.ToString());
			}
			SetValue(SelectedValueProperty, selected);
			SetValue(IsSelectedProperty, selected != null);
		}

		public static readonly DependencyProperty ValueCaptionProperty =
			DpHelper.Create<ComboBoxUnit, string>("ValueCaption", (s, v) => s.ValueCaption = v);
		public string ValueCaption
		{
			get { return (string)GetValue(ValueCaptionProperty); }
			set
			{
				SetValue(ValueCaptionProperty, value);
				Items.SelectItemByKey<Data>(value);
			}
		}

	}
}
