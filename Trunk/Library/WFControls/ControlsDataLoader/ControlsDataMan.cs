using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WFControls.ControlsDataLoader.Data;

namespace WFControls.ControlsDataLoader
{
	public class ControlsDataMan : IData
	{
		private readonly List<IData> m_items = new List<IData>();
		private Action<bool> m_onChange;
		public void Init(Action<bool> onChange)
		{
			m_onChange = onChange;
			foreach (var item in m_items)
			{
				item.Init(onChange);
			}
		}

		public void Add(IData data)
		{
			data.Init(m_onChange);
			m_items.Add(data);
		}

		public void Load()
		{
			foreach (var item in m_items)
			{
				item.Load();
			}
		}

		public void Save()
		{
			foreach (var item in m_items)
			{
				item.Save();
			}
		}

		public bool Changed{get{ return m_items.Any(item => item.Changed); }}
	}
}
