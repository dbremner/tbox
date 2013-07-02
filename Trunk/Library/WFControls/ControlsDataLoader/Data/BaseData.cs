using System;

namespace WFControls.ControlsDataLoader.Data
{
	public abstract class BaseData<TControl> : IData
	{
		protected Action<bool> m_onChange;
		protected TControl m_ctrl;

		protected BaseData(TControl ctrl)
		{
			m_ctrl = ctrl;
		}

		public void Init(Action<bool> onChange)
		{
			m_onChange = onChange;
		}

		public abstract void Load();
		public abstract void Save();
		public abstract bool Changed { get; }
		
		protected void DoChanged()
		{
			m_onChange(Changed);
		}
	}
}
