using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Code.Content;
using Mnk.Library.WpfControls.Code.Dialogs;
using Mnk.Library.WpfControls.Components.EditButtons;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Code
{
    public static class ControllerExtensions
    {
        public static void ConfigureSelector<T>(this Selector selector,
            Collection<T> collection, EditButtonsPanel editButtonsPanel, BaseDialog dialog, IDataManager dataManager, Action<object> renameAction = null)
            where T : Data, ICloneable, new()
        {
            var controller = new Controller(o =>
            {
                if (renameAction!=null) renameAction(o);
                selector.Items.Refresh();
            });
            controller.Config = new Configuration(collection,
                    () => (selector is ListBox) ? ((ListBox)selector).GetSelectedIds().ToArray() : new[] { selector.SelectedIndex },
                    (selector is ComboBox) ? SelectCb(selector) : Select(selector),
                    dialog,
                    dataManager,
                    controller);
            selector.ItemsSource = collection;
            selector.SelectionChanged +=
                (o, args) => editButtonsPanel.OnSelectionChanged();
            editButtonsPanel.SetConfig(controller.Config);
        }

        public static void ConfigureSelector<T>(this Selector selector,
            Collection<T> collection, EditButtonsPanel editButtonsPanel, BaseDialog dialog, Action<object> renameAction = null)
            where T : Data, ICloneable, new()
        {
            selector.ConfigureSelector(collection, editButtonsPanel, dialog, new DataItemManager<T>(), renameAction);
        }

        private static Action<int> Select(Selector cb)
        {
            return id => cb.SelectedIndex = id;
        }

        private static Action<int> SelectCb(Selector cb)
        {
            return id =>
            {
                var i = id;
                if (cb.SelectedIndex == i)
                    cb.SelectedIndex = -1;
                cb.SelectedIndex = i;
            };
        }
    }
}