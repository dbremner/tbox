using System;
using System.Collections.Generic;
using System.Linq;
using Interface;
using Interface.Atrributes;
using PluginsShared.Tools;
using WPFWinForms;

namespace AppConfigManager
{
	[PluginName("AppSettings manager")]
	[PluginDescription("Ability to change appsettings in the set of app or web configs.\nFor example you can enable/disable features.")]
	public class AppConfigManager : ConfigurablePlugin<Settings, Config>
	{
		private readonly Lazy<FeatureToggler> executor = new Lazy<FeatureToggler>(() => new FeatureToggler());

		public AppConfigManager()
		{
			Icon = Properties.Resources.Icon;
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config.Profiles
				.Where(p => p.Files.Any(o => o.IsChecked) && p.Options.Any(o=>o.IsChecked) )
				.Select(p => new UMenuItem
				{
					Header = p.Key,
					OnClick = o=>executor.Value.Execute(
						p.Files.CheckedItems.Select(x=>x.Key), 
						p.Options.CheckedItems.Select(x=>new KeyValuePair<string, string>(x.Key, x.Value)).ToArray())
				}).ToArray();
		}
	}
}
