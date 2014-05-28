using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Dialogs.Menu;

namespace Mnk.Library.WpfControls.Dialogs
{
    public static class DialogsCache
    {
        public static void ShowProgress(Action<IUpdater> function, string title, Window owner, bool topmost = true, bool showInTaskBar = false, ImageSource icon = null)
        {
            new ProgressDialog().ShowDialog(function, title, owner, topmost, showInTaskBar, icon);
        }

        public static KeyValuePair<bool, string> ShowInputBox(string question, string caption, string value, Func<string, bool> validator, Window owner)
        {
            return new InputTextBox().ShowDialog(question, caption, value, validator, owner);
        }

        public static KeyValuePair<bool, string> ShowInputDate(string question, string caption, string value, Func<string, bool> validator, Window owner)
        {
            return new InputDateBox().ShowDialog(question, caption, value, validator, owner);
        }

        public static KeyValuePair<bool, string> ShowMemoBox(string question, string caption, string value, Func<string, bool> validator, Window owner)
        {
            return new InputMemoBox().ShowDialog(question, caption, value, validator, owner);
        }

        public static KeyValuePair<bool, string> ShowInputComboBox(string question, string caption, string value, Func<string, bool> validator, IList<string> source, Window owner, bool isReadOnly = true)
        {
            return new InputComboBox().ShowDialog(question, caption, value, validator, source, owner, isReadOnly);
        }

        public static void ShowInputListUnit(string question, string caption, Collection<Data> value, IList<string> values, Window owner, bool showInTaskBar = false)
        {
            new InputListUnit().ShowDialog(question, caption, value, values, owner, showInTaskBar);
        }

        public static KeyValuePair<bool, string> ShowInputSelect(string question, string caption, string value, Func<string, bool> validator, IList<string> source, Window owner, bool showInTaskBar = false)
        {
            return new InputSelect().ShowDialog(question, caption, value, validator, source, owner, showInTaskBar);
        }

        public static KeyValuePair<bool, string> ShowInputMenuItem(string question, string caption, string value, Func<string, bool> validator, IEnumerable<MenuDialogItem> values, Window owner, bool showInTaskBar = false)
        {
            return new InputMenuItem().ShowDialog(question, caption, value, validator, values, owner, showInTaskBar);
        }

        private static readonly IDictionary<Type, IList<IDialog>> Dialogs = new Dictionary<Type, IList<IDialog>>();

        public static KeyValuePair<bool, string[]> ShowInputFilePath(string caption, string value, Window owner, string filter = "", bool allowSelectMany = false)
        {
            return GetDialog<InputFilePath>().ShowDialog(caption, value, filter, owner, allowSelectMany);
        }

        public static KeyValuePair<bool, string> ShowInputFolderPath(string caption, string value, Window owner)
        {
            return GetDialog<InputFolderPath>().ShowDialog(caption, value, owner);
        }

        public static T GetDialog<T>()
            where T : IDialog
        {
            lock (Dialogs)
            {
                var type = typeof(T);
                IList<IDialog> list;
                if (!Dialogs.TryGetValue(type, out list))
                {
                    Dialogs[type] = list = new List<IDialog>();
                }
                var available = list.FirstOrDefault(x => !x.IsVisible);
                if (available == null)
                {
                    available = (IDialog)Activator.CreateInstance(type);
                    list.Add(available);
                }
                return (T)available;
            }
        }
    }
}
