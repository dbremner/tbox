using System;
using System.Drawing;
using System.Windows.Controls;
using Common.Base.Log;
using TBox.Code.Managers;
using WPFControls.Code.OS;
using WPFWinForms.Icons;

namespace TBox.Code.Objects
{
	class PluginsContextShared
	{
		private static readonly ILog Log = LogManager.GetLogger<PluginsContextShared>();
		private readonly Control sync;
		private readonly IconsCache iconsCache;
		private readonly IconsExtractor iconsExtractor;
		private readonly WarmingUpManager warmingUpManager;

		public PluginsContextShared(IconsCache iconsCache, IconsExtractor iconsExtractor, Control sync, WarmingUpManager warmingUpManager)
		{
			this.iconsCache = iconsCache;
			this.sync = sync;
			this.warmingUpManager = warmingUpManager;
			this.iconsExtractor = iconsExtractor;
		}

		public void DoSync(Action action)
		{
			Mt.Do(sync, action);
		}

		public void AddTypeToWarmingUp(Type t)
		{
			warmingUpManager.TryAdd(t);
		}

		public Icon GetIcon(string path, int id)
		{
			try
			{
				lock (iconsExtractor)
				{
					var icon = iconsCache.Get(path, id);
					if (icon == null)
					{
						icon = iconsExtractor.GetIcon(path, id, true);
						iconsCache.Add(path, id, icon);
					}
					return icon;
				}
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't load icon: " + path);
				return null;
			}
		}

		public Icon GetSystemIcon(int id)
		{
			return GetIcon("C:/Windows/System32/shell32.dll", id);
		}
	}
}
