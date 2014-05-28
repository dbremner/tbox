using System;
using System.Windows.Controls;

namespace Mnk.TBox.Core.Contracts
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
            s.Control.DataContext = ConfigManager.Config;
			return s;
		}

		public override void Load()
		{
			if (Settings.IsValueCreated)
                Settings.Value.Control.DataContext = ConfigManager.Config;
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
