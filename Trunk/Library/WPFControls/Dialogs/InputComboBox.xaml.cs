using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.Tools;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Dialogs
{
    /// <summary>
    /// Interaction logic for InputComboBox.xaml
    /// </summary>
    sealed partial class InputComboBox
    {
        private bool isSuccess = false;
        private Func<string, bool> validator;
        private IList<string> itemsSource;
        private string initValue;

        public InputComboBox()
        {
            InitializeComponent();
            edData.KeyUp += cbText_TextChanged;
            edData.SelectionChanged += EdDataOnSelectionChanged;
        }

        public string Value
        {
            get { return edData.Text; }
            set { edData.Text = value.Trim(); }
        }

        public KeyValuePair<bool, string> ShowDialog(string question, string caption, string value, Func<string, bool> validator, IList<string> source, Window owner, bool isReadOnly)
        {
            btnOk.ToolTip = string.Empty;
            initValue = value??string.Empty;
            this.validator = validator ?? (v => !v.EqualsIgnoreCase(initValue.Trim()));
            edData.IsEditable = !isReadOnly;
            edData.ItemsSource = itemsSource = source;
            Add(value);
            Value = value;
            Owner = owner;
            Icon = owner == null ? null : owner.Icon;
            Title = caption;
            lbCaption.Content = question;
            cbText_TextChanged(null, null);
            isSuccess = false;
            cbText_TextChanged(null, null);
            edData.SetFocus();
            ShowDialog();
            return new KeyValuePair<bool, string>(isSuccess, Value);
        }

        private void BtnOk_OnClick(object sender, EventArgs e)
        {
            isSuccess = true;
            Add(Value);
            Close();
        }

        private void Add(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            var items = itemsSource;
            if(items.IsReadOnly)return;
            var id = items.IndexOf(value);
            if (id == 0) return;
            if (id != -1)
            {
                items.RemoveAt(id);
            }
            items.Insert(0, value);
            edData.Items.Refresh();
            edData.SelectedIndex = 0;
        }

        private void cbText_TextChanged(object sender, EventArgs e)
        {
            Validate(Value);
        }

        private void EdDataOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0) Validate((string)e.AddedItems[0]);
        }

        private void Validate(string value)
        {
            btnOk.IsEnabled = (value.Length > 0) && validator(value);
            btnOk.ToolTip = btnOk.IsEnabled ? string.Empty : WPFControlsLang.ValueAlreadyExistOrIncorrect;
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                Dispatcher.BeginInvoke(new Func<bool>(() => edData.Focus()));
        }
    }
}
