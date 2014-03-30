using System;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.Base.Log;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Plugins.PasswordsStorage
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : ISettings
    {
        public Settings()
        {
            InitializeComponent();
        }
        
        public UserControl Control
        {
            get { return this; }
        }

        private void CheckChanged(object sender, RoutedEventArgs e)
        {
            Passwords.OnCheckChangedEvent(sender,e);
        }
    }
}
