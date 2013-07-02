using System;
using Common.MT;
using WPFControls.Code.OS;

namespace WPFControls.Components.Updater
{
	class SimpleUpdater<T> : IUpdater
		where T : SimpleProgress
	{
		protected readonly T Owner;
		private readonly Action onUpdate;

		public SimpleUpdater(T owner, Action onUpdate)
		{
			Owner = owner;
			this.onUpdate = onUpdate;
		}

		public void Update(float value)
		{
			if (value > 1) value = 1;
			if (value < 0) return;
			Mt.Do(Owner.pbValue,
				() =>
					{
						Owner.pbValue.IsIndeterminate = false;
						Owner.pbValue.Value = value;
					}
				);
			onUpdate();
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
			Update(current/(float)total);
		}
	}
}
