using System;
using System.Drawing;
using Interface;

namespace TBox.Code.Objects
{
	class PluginContext : IPluginContext
	{
		private readonly Action onMenuChanged;
		private readonly PluginsContextShared pluginsContextShared;

		public IDataProvider DataProvider { get; private set; }
		public PluginContext(IDataProvider dataProvider, Action onMenuChanged, PluginsContextShared pluginsContextShared)
		{
			this.onMenuChanged = onMenuChanged;
			DataProvider = dataProvider;
			this.pluginsContextShared = pluginsContextShared;
		}

		public void DoSync(Action action)
		{
			pluginsContextShared.DoSync(action);
		}

		public void AddTypeToWarmingUp(Type t)
		{
			pluginsContextShared.AddTypeToWarmingUp(t);
		}

		public void RebuildMenu()
		{
			onMenuChanged();
		}

		public Icon GetIcon(string path, int id)
		{
			return pluginsContextShared.GetIcon(path, id);
		}

		public Icon GetSystemIcon(int id)
		{
			return pluginsContextShared.GetSystemIcon(id);
		}

	}
}
