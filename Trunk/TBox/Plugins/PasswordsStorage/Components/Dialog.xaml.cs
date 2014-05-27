﻿using System;
using System.Collections.ObjectModel;
using System.Web.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mnk.Library.Common.Base.Log;
using Mnk.Library.WPFControls.Components.Captioned;
using Mnk.Library.WPFControls.Tools;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Plugins.PasswordsStorage.Code.Settings;

namespace Mnk.TBox.Plugins.PasswordsStorage.Components
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog
    {
        private ILog log = LogManager.GetLogger<Dialog>();
        private IConfigManager<Config> cm;
        public Dialog()
        {
            InitializeComponent();
        }

        public void ShowDialog(IConfigManager<Config> cm, Profile p, Window owner)
        {
            this.cm = cm;
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
            var text = textbox != null ? textbox.Text : ((CaptionedPasswordBox) sender).Value.DecryptPassword();
            if (!string.IsNullOrEmpty(text))
            {
                Clipboard.SetText(text);
            }
        }

        private void CopyAll(object sender, RoutedEventArgs e)
        {
            var info = GetLoginInfo(sender);
            Clipboard.SetText(
                info.Key + Environment.NewLine +
                (string.IsNullOrEmpty(info.Comment) ? string.Empty : (info.Comment + Environment.NewLine)) +
                info.Login + Environment.NewLine +
                info.Password.DecryptPassword()
                );
        }

        private static LoginInfo GetLoginInfo(object sender)
        {
            return (LoginInfo) ((Button) sender).DataContext;
        }

        private void NewPassword(object sender, RoutedEventArgs e)
        {
            try
            {
                var info = GetLoginInfo(sender);
                var password = Membership.GeneratePassword(cm.Config.PasswordLength,
                    cm.Config.PasswordNonAlphaCharacters);
                info.Password = password.EncryptPassword();
                Clipboard.SetText(password);
                Passwords.Items.Items.Refresh();
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected issue");
            }
        }
    }
}