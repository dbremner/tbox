﻿using System;

namespace WFControls.ControlsDataLoader.Data
{
	public interface IData
	{
		void Init(Action<bool> onChange);
		void Load();
		void Save();
		bool Changed { get; }
	}
}
