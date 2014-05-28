using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Code.Dialogs;
using Mnk.Library.WpfControls.Components.Units;
using Mnk.Library.WpfControls.Dialogs.Menu;

namespace Mnk.Library.WpfControls.Tools
{
    public static class SelectorExtensions
    {
        public static void SelectItemByKey<T>(this Selector selector, string value)
            where T : Data
        {
            foreach (var item in selector.Items.Cast<Data>()
                                    .Where(item => item.Key.EqualsIgnoreCase(value)))
            {
                selector.SelectedValue = item;
                return;
            }
            selector.SelectedIndex = -1;
        }

        public static void SelectItemByKey(this Selector selector, string value)
        {
            foreach (var item in selector.Items.Cast<string>()
                                    .Where(item => item.EqualsIgnoreCase(value)))
            {
                selector.SelectedValue = item;
                return;
            }
            selector.SelectedIndex = -1;
        }

        public static string GetSelectKey<T>(this Selector selector)
            where T : Data
        {
            var o = selector.GetSelectObject<T>();
            return (o == null) ? string.Empty : o.Key;
        }

        public static T GetSelectObject<T>(this Selector selector)
        {
            return (T)selector.SelectedValue;
        }

        public static IEnumerable<int> GetSelectedIds(this ListBox selector)
        {
            var i = 0;
            var selected = selector.SelectedItems;
            foreach (var item in selector.Items)
            {
                if (selected.Contains(item)) yield return i;
                ++i;
            }
        }

        public static void AddRange<T>(this ItemCollection collection, IEnumerable<T> values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }

        public static void AddRange(this ItemCollection collection, params object[] values)
        {
            foreach (var value in values)
            {
                collection.Add(value);
            }
        }

        public static void ConfigureInputText<T>(this IUnit unit, string caption, Collection<T> items, Templates templates = null, Func<string, bool> validator = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputText(caption, templates ?? Templates.Default, validator ?? items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)));
        }

        public static void ConfigureInputTextList<T>(this IUnit unit, string caption, Collection<T> items, Templates templates = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputTextList(caption, templates ?? Templates.Default, items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)));
        }

        public static void ConfigureInputTextList<T>(this IUnit unit, string caption, Collection<T> items, IList<string> itemSource, Templates templates = null, Window owner = null, bool isReadOnly = false)
            where T : Data, new()
        {
            unit.Configure(items, new InputTextList(caption, templates ?? Templates.Default, items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit), isReadOnly) { ItemsSource = itemSource });
        }

        public static void ConfigureInputSelect<T>(this IUnit unit, string caption, Collection<T> items, IList<string> itemSource = null, Templates templates = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputSelect(caption, templates ?? Templates.Default, items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)) { ItemsSource = itemSource });
        }

        public static void ConfigureInputFilePath<T>(this IUnit unit, string caption, Collection<T> items, PathTemplates templates = null, Func<string, bool> validator = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputFilePath(caption, templates ?? PathTemplates.Default, validator ?? items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)));
        }

        public static void ConfigureInputFolderPath<T>(this IUnit unit, string caption, Collection<T> items, PathTemplates templates = null, Func<string, bool> validator = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputFolderPath(caption, templates ?? PathTemplates.Default, validator ?? items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)));
        }

        public static void ConfigureInputMenuItem<T>(this IUnit unit, string caption, Collection<T> items, IList<MenuDialogItem> itemSource, Templates templates = null, Window owner = null, Func<string, bool> validator = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputMenuDialog(caption, templates ?? Templates.Default, validator ?? items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)) { ItemsSource = itemSource });
        }

        public static void ConfigureInputDate<T>(this IUnit unit, string caption, Collection<T> items, Templates templates = null, Func<string, bool> validator = null, Window owner = null)
            where T : Data, new()
        {
            unit.Configure(items, new InputDate(caption, templates ?? Templates.Default, validator ?? items.IsUniqueIgnoreCase, () => owner ?? GetOwner(unit)));
        }


        public static string GetNewSelection(this SelectionChangedEventArgs e)
        {
            return e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : string.Empty;
        }

        private static Window GetOwner(IUnit unit)
        {
            if (unit == null || unit.Control == null) return null;
            return unit.Control.GetParentWindow();
        }
    }
}
