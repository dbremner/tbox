using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mnk.Library.WPFControls.Components.Captioned;
using Mnk.Library.WPFControls.Tools;
using Mnk.TBox.Plugins.PasswordsStorage.Code.Settings;

namespace Mnk.TBox.Plugins.PasswordsStorage.Components
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog 
    {
        public Dialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(Profile p, Window owner)
        {
            Title = p.Key;
            DataContext = p;
            Owner = owner;
            if (owner != null)
            {
                SafeShowDialog();
            }
            else ShowAndActivate();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void CellClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var textbox = sender as TextBox;
            Clipboard.SetText(textbox != null ? textbox.Text : ((CaptionedPasswordBox)sender).Value.DecryptPassword());
        }

        private void CopyAll(object sender, RoutedEventArgs e)
        {
            var info = (LoginInfo) ((Button) sender).DataContext;
            Clipboard.SetText(
                info.Key + Environment.NewLine +
                (string.IsNullOrEmpty(info.Comment) ? string.Empty : (info.Comment + Environment.NewLine)) +
                info.Login + Environment.NewLine +
                info.Password.DecryptPassword()
                );
        }
    }
}
