using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Mnk.Library.Common.MT;

namespace Mnk.Library.WpfControls.Components.Updater
{
    /// <summary>
    /// Interaction logic for SimpleProgress.xaml
    /// </summary>
    public partial class SimpleProgress
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private bool isEnd = false;
        private Action endAction = null;

        protected object Locker { get; private set; }
        protected bool UserPressClose { get; set; }
        public double Value { get; set; }

        public SimpleProgress()
        {
            Locker = new object();
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += TryHide;
        }

        protected virtual void TryHide(object sender, EventArgs e)
        {
            if (isEnd)
            {
                timer.Stop();
                endAction();
            }
            else
            {
                if (pbValue.Value < Value)
                {
                    pbValue.IsIndeterminate = false;
                    pbValue.Value = Value;
                }
            }
        }

        protected virtual void Reset()
        {
            UserPressClose = false;
            Button.IsEnabled = true;
            pbValue.Value = Value = 0;
            pbValue.IsIndeterminate = true;
            isEnd = false;
            timer.Start();
        }

        internal bool IsUserPressClose()
        {
            lock (Locker)
            {
                return UserPressClose;
            }
        }

        protected virtual IUpdater CreateUpdater()
        {
            return new SimpleUpdater<SimpleProgress>(this);
        }

        public virtual void Start(Action<IUpdater> func, Action end = null)
        {
            endAction = end;
            Reset();
            var u = CreateUpdater();
            var culture = Thread.CurrentThread.CurrentUICulture;
            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.CurrentThread.CurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                try
                {
                    func(u);
                }
                finally
                {
                    isEnd = true;
                    u.Dispose();
                }
            });
        }

        internal virtual void ButtonClick(object sender, RoutedEventArgs e)
        {
            lock (Locker)
            {
                UserPressClose = true;
            }
            Button.IsEnabled = false;
        }
    }
}
