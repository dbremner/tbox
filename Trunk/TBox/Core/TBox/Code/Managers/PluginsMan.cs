using System;
using System.Linq;
using Common.Plugins;
using Interface;

namespace TBox.Code.Managers
{
	class PluginsMan : PlugMan<IPlugin>
	{
		private readonly Action<Action> sync;
		public PluginsMan(Factory<IPlugin> factory, string pluginsConfigDir, Action<Action> sync) :
			base(factory, pluginsConfigDir)
		{
			this.sync = sync;
		}

		protected override void LoadItem(Info item)
		{
			base.LoadItem(item);
			var obj = item.ObjConf as IConfigurablePlugin;
			if (obj != null) sync(obj.Load);
		}

		protected override void SaveItem(Info item, bool autoSaveOnExit)
		{
			var obj = item.ObjConf as IConfigurablePlugin;
			if (obj != null) sync(() => obj.Save(autoSaveOnExit));
			base.SaveItem(item, autoSaveOnExit);
		}

		public void RebuildMenu()
		{
			foreach (var cp in Items.Select(item => item.Value.Obj).OfType<IConfigurablePlugin>())
			{
				cp.OnRebuildMenu();
			}
		}
	}
}
