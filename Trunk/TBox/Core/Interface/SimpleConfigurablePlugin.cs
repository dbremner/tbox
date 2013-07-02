using System;
using System.Windows.Controls;

namespace Interface
{
	public class SimpleConfigurablePlugin<TConfig> : SimplePlugin, IConfigurablePlugin
		where TConfig: new()
	{
		protected TConfig Config { get; set; }

		protected SimpleConfigurablePlugin()
		{
			Config = new TConfig();
		}

		public Type ConfigType
		{
			get { return typeof (TConfig); }
		}

		public object ConfigObject
		{
			get { return Config; }
			set { Config = (TConfig)value; }
		}

		protected virtual void OnConfigUpdated()
		{
			OnRebuildMenu();
		}

		public virtual void Load()
		{
			OnConfigUpdated();
		}

		public virtual void Save(bool autoSaveOnExit)
		{
			if (autoSaveOnExit) return;
			OnConfigUpdated();
		}

		public virtual void OnRebuildMenu()
		{
		}

		public virtual Func<Control> SettingsGetter{get{return null;}}
	}
}
