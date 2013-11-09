using System;
using System.Collections.Generic;
using System.Linq;
using WPFControls.Code.ControlsDataLoader.Data;

namespace WPFControls.Code.ControlsDataLoader
{
	public class ControlsDataMan : IData
	{
		private readonly List<IData> items = new List<IData>();
		private Action<bool> onChange;
		public void Init(Action<bool> changeAction)
		{
			onChange = changeAction;
			foreach (var item in items)
			{
				item.Init(changeAction);
			}
		}

		public void Add(IData data)
		{
			data.Init(onChange);
			items.Add(data);
		}

		public void Load()
		{
			foreach (var item in items)
			{
				item.Load();
			}
		}

		public void Save()
		{
			foreach (var item in items)
			{
				item.Save();
			}
		}

		public bool Changed{get{ return items.Any(item => item.Changed); }}
	}
}
