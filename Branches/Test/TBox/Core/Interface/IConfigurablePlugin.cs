using System;
using System.Windows.Controls;
using Common.Plugins;

namespace Interface
{
	public interface IConfigurablePlugin : IPlugin, IConfigurable
	{
		void Load();
		void Save(bool autoSaveOnExit);
		void OnRebuildMenu();
		Func<Control> SettingsGetter { get; }
	}
}
