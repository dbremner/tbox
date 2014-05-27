using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mnk.Library.Common.Models;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.SaveLoad;

namespace Mnk.Library.Common.Plugins
{
	public class PluginsManager<TInterface> : IEnumerable<Pair<string, TInterface>>
		where TInterface : class 
	{
		protected sealed class Info
		{
			public TInterface Obj { get; set; }
			public AnySerializer Ser { get; set; }
			public IConfigurable ObjConf { get; set; }
		}

		public Factory<TInterface> Factory { get; private set; }
		private readonly string pluginsConfigDir;
		protected IDictionary<string,Info> Items { get; private set; }

		public PluginsManager(Factory<TInterface> factory, string pluginsConfigDir)
		{
			Items = new Dictionary<string, Info>();
			Factory = factory;
			this.pluginsConfigDir = pluginsConfigDir;
			if(!Directory.Exists(pluginsConfigDir))
			{
				Directory.CreateDirectory(pluginsConfigDir);
			}
		}

		public TInterface Create(string name)
		{
			var plugin = Factory.Create(name);
			if (plugin!=null)
			{
				var info = new Info
				{
					Obj = plugin.Value,
				};
				var serObj = info.Obj as IConfigurable;
				if (serObj!=null)
				{
					info.ObjConf = serObj;
					info.Ser = new AnySerializer(
						Path.Combine(pluginsConfigDir, name+".config"), 
						serObj.ConfigType);
				}
				lock (Items)
				{
					Items.Add(name, info);
				}
				return info.Obj;
			}
			return null;
		}

		public bool Remove(string name)
		{
			var item = Items.Get(name);
			if (item != null)
			{
				var plg = item.Obj as IDisposable;
				if (plg != null)
				{
					plg.Dispose();
				}
			}
			return Items.Remove(name);
		}

		public bool Have(string name)
		{
			return Items.ContainsKey(name);
		}

		public TInterface Get(string name)
		{
			return Items.First(x => string.Equals(x.Key, name)).Value.Obj;
		}

		private void Work(IUpdater updater, Action<Info> action)
		{
			var list = Items.Where(x => x.Value.ObjConf != null).ToArray();
			var count = list.Count();
			var id = 0;
			Parallel.ForEach(list,
				item =>
				{
					if (updater != null) updater.Update(item.Key, (id++) / (float)count);
					action(item.Value);
				});
		}

		public virtual void LoadItem(string name)
		{
			var item = Items.Get(name);
			if(item!=null && item.Obj is IConfigurable)
			{
				LoadItem(item);
			}
		}

		protected virtual void LoadItem(Info item)
		{
			item.ObjConf.ConfigObject = item.Ser.Load(item.ObjConf.ConfigObject);
		}

		public void Load(IUpdater updater)
		{
			Work(updater, LoadItem);
		}

		protected virtual void SaveItem( Info item, bool autoSaveOnExit )
		{
			item.Ser.Save(item.ObjConf.ConfigObject);
		}

		public void Save( IUpdater updater, bool autoSaveOnExit )
		{
			Work( updater, info => SaveItem( info, autoSaveOnExit ) );
		}

		public IEnumerator<Pair<string, TInterface>> GetEnumerator()
		{
			return Items.
				Select(x => new Pair<string, TInterface>(x.Key, x.Value.Obj)).
				GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
