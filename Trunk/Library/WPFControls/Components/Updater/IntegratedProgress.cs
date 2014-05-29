using System;
using System.Windows;
using Mnk.Library.Common.MT;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Components.Updater
{
    public class IntegratedProgress : Progress
    {
        private bool started = false;

        public event RoutedEventHandler OnStartClick;

        public IntegratedProgress()
        {
            PrepareToRun();
        }

        private void PrepareToRun()
        {
            Button.IsEnabled = true;
            Button.Content = WPFControlsLang.Start;
            pbValue.IsIndeterminate = false;
            pbValue.Value = 0;
            SetMessage(string.Empty);
        }

        private bool IsStarted
        {
            get
            {
                lock (Locker)
                {
                    return started;
                }
            }
        }

        internal override void ButtonClick(object sender, RoutedEventArgs e)
        {
            if (IsStarted)
            {
                base.ButtonClick(sender, e);
            }
            else
            {
                OnStartClick(sender, e);
            }
        }

        public override void Start(Action<IUpdater> func, Action end = null)
        {
            lock (Locker)
            {
                started = true;
            }
            Button.Content = WPFControlsLang.Stop;
            base.Start(func,
                () =>
                    {
                        if(end!=null)end();
                        Mt.Do(this, PrepareToRun);
                        lock (Locker)
                        {
                            started = false;
                        }
                    });
        }
    }
}
