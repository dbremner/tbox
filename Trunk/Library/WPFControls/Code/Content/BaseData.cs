using System;

namespace Mnk.Library.WpfControls.Code.Content
{
	public abstract class BaseData<TControl> : IData
	{
		protected Action<bool> OnChange{ get; set; }
		protected TControl Control { get; private set; }

		protected BaseData(TControl ctrl)
		{
			Control = ctrl;
		}

		public void Init(Action<bool> changeAction)
		{
			OnChange = changeAction;
		}

		public abstract void Load();
		public abstract void Save();
		public abstract bool Changed { get; }
		
		protected void DoChanged()
		{
			OnChange(Changed);
		}
	}
}
