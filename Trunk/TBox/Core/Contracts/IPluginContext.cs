using System;
using System.Drawing;

namespace Mnk.TBox.Core.Contracts
{
	public interface IPluginContext
	{
		IDataProvider DataProvider { get; }
		void RebuildMenu();
		Icon GetIcon(string path, int id);
		Icon GetSystemIcon(int id);
		void DoSync(Action action);
		void AddTypeToWarmingUp(Type type);
	}
}
