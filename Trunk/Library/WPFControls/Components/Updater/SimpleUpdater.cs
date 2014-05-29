using System;
using System.Windows;
using System.Windows.Shell;
using Mnk.Library.Common.MT;
using Mnk.Library.WpfControls.Tools;

namespace Mnk.Library.WpfControls.Components.Updater
{
    class SimpleUpdater<T> : IUpdater
        where T : SimpleProgress
    {
        protected readonly T Owner;
        private Window parent;
        private TaskbarItemInfo tbInfo;

        public SimpleUpdater(T owner)
        {
            Owner = owner;
        }

        public void Update(float value)
        {
            if (value > 1) value = 1;
            if (value < 0) return;
            Owner.Value = value;
            Mt.Do(Owner, () =>
            {
                if (tbInfo == null)
                {
                    parent = Owner.GetParentWindow();
                    while (parent.Owner != null) parent = parent.Owner;
                    if (parent == null) return;
                    if (parent.TaskbarItemInfo == null) parent.TaskbarItemInfo = new TaskbarItemInfo();
                    tbInfo = parent.TaskbarItemInfo;
                    tbInfo.ProgressState = TaskbarItemProgressState.Normal;
                }
                if (tbInfo != null) tbInfo.ProgressValue = value;
            });
        }

        public virtual void Update(string caption, float value)
        {
            Update(value);
        }

        public bool UserPressClose
        {
            get { return Owner.IsUserPressClose(); }
        }

        public virtual void Update(Func<int, string> caption, int current, int total)
        {
            Update(current / (float)total);
        }

        public void Dispose()
        {
            if (tbInfo != null)
            {
                Mt.Do(Owner, () => tbInfo.ProgressState = TaskbarItemProgressState.None);
            }
        }
    }
}
