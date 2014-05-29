using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Code.Content;
using Mnk.Library.WpfControls.Code.Dialogs;

namespace Mnk.Library.WpfControls.Components.Units
{
    public abstract class BaseCheckableCollectionUnit : BaseCollectionUnit, ICheckableUnit
    {
        protected readonly PanelToICheckable Checkable;
        public event Action<BaseDialog> OnConfigured;
        public IDataManager CustomDataManager { get; set; }

        protected BaseCheckableCollectionUnit()
        {
            Init();
            Checkable = new PanelToICheckable { Margin = new Thickness(1) };
            Panel.Children.Insert(1, Checkable);
            DockPanel.SetDock(Checkable, Dock.Bottom);
        }

        public override void Configure<T>(Collection<T> items, BaseDialog dialog)
        {
            if (CustomDataManager != null)
            {
                Items.ConfigureSelector(items, Buttons, dialog, CustomDataManager, OnEditHandler);
            }
            else
            {
                Items.ConfigureSelector(items, Buttons, dialog, OnEditHandler);
            }
            Checkable.View = (ICheckableItemsView)Items;
            if (OnConfigured != null) OnConfigured(dialog);
        }

        public override void Unconfigure()
        {
            base.Unconfigure();
            Checkable.View = null;
        }

        public void OnCheckChangedEvent(object sender, RoutedEventArgs routedEventArgs)
        {
            ((CheckableListBox)Items).OnCheckChangedEvent(sender, routedEventArgs);
        }

        public void Refresh()
        {
            ((CheckableListBox)Items).Refresh();
        }
    }
}
