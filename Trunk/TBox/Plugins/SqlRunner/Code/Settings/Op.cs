using System;
using Common.UI.Model;
using PluginsShared.Ddos.Settings;

namespace SqlRunner.Code.Settings
{
	[Serializable]
	public sealed class Op : CheckableData, IOperation
	{
		public int Threads { get; set; }
		public int Delay { get; set; }
		public int Timeout { get; set; }
		public string Command { get; set; }

		public Op()
		{
			Delay = 0;
			Threads = 1;
			Timeout = 120;
		}

		public override object Clone()
		{
			var p = new Op
			{
				Key = Key,
				IsChecked = IsChecked,
				Threads = Threads,
				Delay = Delay,
				Timeout = Timeout,
				Command = Command
			};
			return p;
		}
	}
}
