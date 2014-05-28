using System;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.MT;

namespace Mnk.Library.WpfControls.Components.Updater
{
    public class Progress : SimpleProgress
    {
        private string message;
        private readonly Label lMessage = new Label { Padding = new Thickness(5, 5, 5, 0), Height = 20 };
        public Progress()
        {
            var panel = (DockPanel)Content;
            panel.Children.Insert(0, lMessage);
            DockPanel.SetDock(lMessage, Dock.Top);
        }

        protected override void TryHide(object sender, EventArgs e)
        {
            base.TryHide(sender, e);
            if (!string.Equals(lMessage.Content, message))
            {
                lMessage.Content = message;
            }
        }

        protected override void Reset()
        {
            base.Reset();
            lMessage.Content = message = string.Empty;
        }

        public void SetMessage(string value)
        {
            message = value;
        }

        protected override IUpdater CreateUpdater()
        {
            return new Updater(this);
        }
    }
}
