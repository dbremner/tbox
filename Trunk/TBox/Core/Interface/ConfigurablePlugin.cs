using System;
using System.Windows.Controls;

namespace Interface
{
	public abstract class ConfigurablePlugin<TSettings, TConfig> : SimpleConfigurablePlugin<TConfig>, IConfigurablePlugin
		where TSettings: ISettings, new()
		where TConfig: new()
	{
		protected Lazy<TSettings> Settings { get; set; }

		protected ConfigurablePlugin()
		{
			Settings = new Lazy<TSettings>(CreateSettings);
		}

		protected virtual TSettings CreateSettings()
		{
			var s = new TSettings();
			s.Control.DataContext = Config;
			return s;
		}

		public override void Load()
		{
			if (Settings.IsValueCreated) 
				Settings.Value.Control.DataContext = Config;
			base.Load();
		}

		public override Func<Control> SettingsGetter
		{
			get
			{
				return () => Settings.Value.Control;
			}
		}
	}
}
