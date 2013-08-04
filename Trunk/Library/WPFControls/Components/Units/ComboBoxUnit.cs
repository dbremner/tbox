using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Common.UI.Model;
using WPFControls.Components.EditButtons;
using WPFControls.Components.Units.Properties;
using WPFControls.Controls;
using WPFControls.Tools;

namespace WPFControls.Components.Units
{
	public sealed class ComboBoxUnit : BaseCollectionUnit
	{
		public ComboBoxUnit()
		{
			Init();
			Items.SelectionChanged += ItemsOnSelectionChanged;
		}

		protected override Selector CreateItems()
		{
			return new ComboBox{Margin = new Thickness(3), TabIndex = 0};
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

		private void ItemsOnSelectionChanged(object o, EventArgs e)
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

		public static readonly DependencyProperty SelectedValueProperty =
			DpHelper.Create<ComboBoxUnit, object>("SelectedValue", (s, v) => s.SelectedValue = v);
		public object SelectedValue
		{
			get { return Items.SelectedItem; }
			set
			{
				SetValue(SelectedValueProperty, value);
				Items.SelectedValue = value;
			}
		}

		public static readonly DependencyProperty IsSelectedProperty =
			DpHelper.Create<ComboBoxUnit, bool>("IsSelected");
		public bool IsSelected
		{
			get { return (bool)GetValue(IsSelectedProperty); }
		}

	}
}
