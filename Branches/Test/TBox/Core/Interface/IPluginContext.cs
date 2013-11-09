﻿using System;
using System.Drawing;

namespace Interface
{
	public interface IPluginContext
	{
		IDataProvider DataProvider { get; }
		void RebuildMenu();
		Icon GetIcon(string path, int id);
		Icon GetSystemIcon(int id);
		void DoSync(Action action);
		void AddTypeToWarmingUp(Type t);
	}
}