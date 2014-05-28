using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Dialogs
{
    /// <summary>
    /// Interaction logic for InputListBox.xaml
    /// </summary>
    public partial class InputListUnit
    {
        public InputListUnit()
        {
            InitializeComponent();
        }

        public void ShowDialog(string question, string caption, Collection<Data> value, IList<string> values, Window owner, bool showInTaskBar = false)
        {
            ShowInTaskbar = showInTaskBar;
            Owner = owner;
            Icon = owner == null ? null : owner.Icon;
            Title = caption;
            Question.Text = question;
            Items.ConfigureInputTextList(question, value, values, isReadOnly:true, owner:owner);
            ShowDialog();
        }

        private void BtnClose_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
