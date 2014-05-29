using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Code.Dialogs;
using Mnk.Library.WpfControls.Components.EditButtons;

namespace Mnk.Library.WpfControls.Components.Units
{
    public abstract class BaseCollectionUnit : UserControl, IUnit
    {
        protected UnitConfigurator UnitConfigurator;
        public EditButtonsPanel Buttons { get; private set; }
        public Selector Items { get; private set; }
        protected DockPanel Panel;
        public event Action<object> EditHandler;

        protected virtual void OnEditHandler(object obj)
        {
            Action<object> handler = EditHandler;
            if (handler != null) handler(obj);
        }

        protected void Init()
        {
            UnitConfigurator = CreateConfigurator();
            Panel = new DockPanel();
            Panel.Children.Add(Buttons = CreateEditButtonsPanel());
            Panel.Children.Add(Items = CreateItems());
            Content = Panel;
            Items.SelectionChanged += ItemsOnSelectionChanged;
        }

        protected abstract Selector CreateItems();

        protected virtual UnitConfigurator CreateConfigurator()
        {
            return new UnitConfigurator(this, () => ItemsOnSelectionChanged(Items, null));
        }

        protected virtual void ItemsOnSelectionChanged(object o, EventArgs e)
        {
            var selected = ((ListBox)o);
            SetValue(SelectedValueProperty, selected.SelectedItem);
            SetValue(SelectedIndexProperty, selected.SelectedIndex);
            SetValue(IsSelectedProperty, selected.SelectedItem != null);
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
            Items.ConfigureSelector(items, Buttons, dialog, OnEditHandler);
        }

        public virtual void Unconfigure()
        {
            Items.ItemsSource = null;
        }

        public Control Control { get { return this; } }

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
                UnitConfigurator.SetTitle(value);
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
                UnitConfigurator.SetCollection(value);
            }
        }

        public static readonly DependencyProperty UnitTypeProperty =
            DpHelper.Create<BaseCollectionUnit, UnitType>("UnitType", (s, v) => s.UnitType = v);
        public UnitType UnitType
        {
            get { return (UnitType)GetValue(UnitTypeProperty); }
            set
            {
                SetValue(UnitTypeProperty, value);
                UnitConfigurator.SetUnitType(value);
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

        public static readonly DependencyProperty SelectedValueProperty =
            DpHelper.Create<BaseCollectionUnit, object>("SelectedValue", (s, v) => s.SelectedValue = v);
        public object SelectedValue
        {
            get { return Items.SelectedItem; }
            set
            {
                SetValue(SelectedValueProperty, value);
                Items.SelectedValue = value;
            }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DpHelper.Create<BaseCollectionUnit, int>("SelectedIndex", (s, v) => s.SelectedIndex = v);
        public int SelectedIndex
        {
            get { return Items.SelectedIndex; }
            set
            {
                SetValue(SelectedIndexProperty, value);
                Items.SelectedIndex = value;
            }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DpHelper.Create<BaseCollectionUnit, bool>("IsSelected");
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
        }
    }
}
