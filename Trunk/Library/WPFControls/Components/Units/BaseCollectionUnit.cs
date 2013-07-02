using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Common.UI.Model;
using WPFControls.Code.Dialogs;
using WPFControls.Code.EditPanel;
using WPFControls.Components.EditButtons;
using WPFControls.Components.Units.Properties;
using WPFControls.Controls;

namespace WPFControls.Components.Units
{
	public abstract class BaseCollectionUnit : UserControl, IUnit
	{
		protected readonly UnitConfigurator unitConfigurator;
		public EditButtonsPanel Buttons { get; private set; }
		public Selector Items { get; private set; }
		protected readonly DockPanel Panel;

		protected BaseCollectionUnit()
		{
			unitConfigurator = CreateConfigurator();
			Panel = new DockPanel();
			Panel.Children.Add(Buttons = CreateEditButtonsPanel());
			Panel.Children.Add(Items = CreateItems());
			Content = Panel;
		}

		protected abstract Selector CreateItems();

		protected virtual UnitConfigurator CreateConfigurator()
		{
			return new UnitConfigurator(this, () => { });
		}

		protected virtual EditButtonsPanel CreateEditButtonsPanel()
		{
			var p = new EditButtonsPanel { Margin = new Thickness(3), TabIndex = 1 };
			DockPanel.SetDock(p, Dock.Left);
			return p;
		}

		public virtual void Configure<T>(Collection<T> items, BaseDialog dialog)
			where T : Data, ICloneable, new()
		{
			Items.ConfigureSelector(items, Buttons, dialog);
		}

		public virtual void Unconfigure()
		{
			Items.ItemsSource = null;
		}

		[Bindable(true)]
		public DataTemplate ItemTemplate
		{
			get { return Items.ItemTemplate; }
			set { Items.ItemTemplate = value; }
		}

		[Bindable(true)]
		public Style ItemContainerStyle
		{
			get { return Items.ItemContainerStyle; }
			set { Items.ItemContainerStyle = value; }
		}

		public static readonly DependencyProperty TitleProperty =
			DpHelper.Create<BaseCollectionUnit, string>("Title", (s, v) => s.Title = v);
		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set
			{
				SetValue(TitleProperty, value);
				unitConfigurator.SetTitle(value);
			}
		}

		public static readonly DependencyProperty ItemsSourceProperty =
			DpHelper.Create<BaseCollectionUnit, ICollection>("ItemsSource", (s, v) => s.ItemsSource = v);
		public ICollection ItemsSource
		{
			get { return (ICollection)GetValue(ItemsSourceProperty); }
			set
			{
				SetValue(ItemsSourceProperty, value);
				unitConfigurator.SetCollection(value);
			}
		}

		public static readonly DependencyProperty UnitTypeProperty =
			DpHelper.Create<BaseCollectionUnit, UnitTypes>("UnitType", (s, v) => s.UnitType = v);
		public UnitTypes UnitType
		{
			get { return (UnitTypes)GetValue(UnitTypeProperty); }
			set
			{
				SetValue(UnitTypeProperty, value);
				unitConfigurator.SetUnitType(value);
			}
		}

		public bool SmoothScrolling
		{
			get { return !(bool)Items.GetValue(ScrollViewer.CanContentScrollProperty); }
			set
			{
				Items.SetValue(ScrollViewer.CanContentScrollProperty, !value);
			}
		}

	}
}
