using System;
using System.Windows;
using System.Windows.Media;
using Mnk.Library.Common.MT;
using Mnk.Library.WpfControls.Components.Updater;

namespace Mnk.Library.WpfControls.Dialogs
{
    class ProgressDialog : ClosableDialogWindow, IDialog
    {
        private Progress progress;
        public double Value { get { return progress.Value; } }
        protected bool CanClose { get; set; }

        public ProgressDialog()
        {
            CanClose = false;
            Width = 400;
            Height = 104;
            MaxHeight = 104;
            MinHeight = 104;
            MinWidth = 200;
            ShowInTaskbar = false;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
        }

        public void SetMessage(string value)
        {
            progress.SetMessage(value);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!CanClose)
            {
                e.Cancel = true;
                progress.ButtonClick(null, null);
            }
            base.OnClosing(e);
        }

        private Action<IUpdater> func;
        public bool? ShowDialog(Action<IUpdater> function, string title = "", Window owner = null, bool topmost = true, bool showInTaskBar = false, ImageSource icon = null)
        {
            Icon = icon ?? (owner == null ? null : owner.Icon);
            ShowInTaskbar = showInTaskBar;
            Topmost = topmost;
            Owner = owner;
            Title = title;
            func = function;
            Content = progress = new Progress { Margin = new Thickness(5) };
            progress.Start(func, Dispose);
            return ShowDialog();
        }

        public void Dispose()
        {
            CanClose = true;
            Close();
        }
    }
}
