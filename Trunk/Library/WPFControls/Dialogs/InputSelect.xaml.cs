using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Dialogs
{
    /// <summary>
    /// Interaction logic for InputSelect.xaml
    /// </summary>
    sealed partial class InputSelect
    {
        private bool isSuccess = false;
        private Func<string, bool> validator;
        public InputSelect()
        {
            InitializeComponent();
        }

        public KeyValuePair<bool, string> ShowDialog(string question, string caption, string value, Func<string, bool> validator, IEnumerable<string> values, Window owner, bool showInTaskBar = false)
        {
            btnOk.ToolTip = string.Empty;
            ShowInTaskbar = showInTaskBar;
            this.validator = validator;
            Owner = owner;
            Icon = owner == null ? null : owner.Icon;
            Title = caption;
            Question.Text = question;
            isSuccess = false;
            Items.ItemsSource = values.Where(validator);
            Items.SelectItemByKey(value);
            Selector_OnSelectionChanged(null, null);
            ShowDialog();
            return new KeyValuePair<bool, string>(isSuccess,
                (Items.SelectedItem != null) ? Items.SelectedItem.ToString().Trim() : string.Empty);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOk.IsEnabled = CanChoose();
            btnOk.ToolTip = btnOk.IsEnabled ? string.Empty : WPFControlsLang.ValueAlreadyExistOrIncorrect;
        }

        private bool CanChoose()
        {
            return Items.SelectedIndex >= 0 &&
                   Items.SelectedValue != null && validator(Items.SelectedValue.ToString().Trim());
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            isSuccess = true;
            Hide();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void OnMouseLeftButton(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            if (CanChoose()) BtnOk_OnClick(sender, e);
        }
    }
}
