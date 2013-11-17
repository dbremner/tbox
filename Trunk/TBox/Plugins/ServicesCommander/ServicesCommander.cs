using System;
using System.Linq;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.ServicesCommander;
using ServicesCommander.Code;
using WPFControls.Dialogs;
using WPFWinForms;
using WPFWinForms.Icons;

namespace ServicesCommander
{
	[PluginInfo(typeof(ServicesCommanderLang), 71, PluginGroup.Desktop)]
	public class ServicesCommander : ConfigurablePlugin<Settings, Config>
	{
		private Runner runner;
		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config
				.Profiles.Where(x=>x.Services.CheckedItems.Any())
				.Select(p => new UMenuItem
					{
						Header = p.Key, 
						Items = p.Services.CheckedItems
								.SelectMany(s=>new[]{
										new UMenuItem
											{
												Header = s.Key + " - " + (runner.IsRunning(s) ? ServicesCommanderLang.Stop : ServicesCommanderLang.Start),
												OnClick = o =>DoWithProgress(s.Key + " - " + (runner.IsRunning(s) ? ServicesCommanderLang.Stop : ServicesCommanderLang.Start), ()=>runner.ToggleService(s)) 
											},
										new UMenuItem
											{
												Header = s.Key + " - " + ServicesCommanderLang.Restart,
												OnClick = o =>DoWithProgress(s.Key + " - " + ServicesCommanderLang.Restart, ()=>runner.RestartService(s))
											}
									})
									.Concat(new[]
										{
											new USeparator(), 
											new UMenuItem
												{
													Header = ServicesCommanderLang.StartAll,
													OnClick = o=>runner.StartAll(p)
												},
											new UMenuItem
											{
												Header = ServicesCommanderLang.StopAll,
												OnClick = o=>runner.StopAll(p)
											}
										})
									.ToArray()
					} )
				.Concat(new[]{new UMenuItem
					{
						Header = ServicesCommanderLang.Refresh,
						OnClick = o=>Context.RebuildMenu()
					} })
				.ToArray();
		}

		private void DoWithProgress(string title, Action op)
		{
			DialogsCache.ShowProgress(u=>op(), title, null, icon: Icon.ToImageSource());
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			runner = new Runner(context);
		}
	}
}
