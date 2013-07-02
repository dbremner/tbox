using System;
using System.Linq;
using Interface;
using Interface.Atrributes;
using ServicesCommander.Code;
using WPFControls.Dialogs;
using WPFWinForms;

namespace ServicesCommander
{
	[PluginName("Services commander")]
	[PluginDescription("Tool to start and stop any of the selected services.")]
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
												Header = s.Key + " - " + (runner.IsRunning(s) ? "Stop" : "Start"),
												OnClick = o =>DoWithProgress(s.Key + " - " + (runner.IsRunning(s) ? "Stop" : "Start"), ()=>runner.ToggleService(s)) 
											},
										new UMenuItem
											{
												Header = s.Key + " - Restart",
												OnClick = o =>DoWithProgress(s.Key + " - Restart", ()=>runner.RestartService(s))
											}
									})
									.Concat(new[]
										{
											new USeparator(), 
											new UMenuItem
												{
													Header = "Start All",
													OnClick = o=>runner.StartAll(p)
												},
											new UMenuItem
											{
												Header = "Stop All",
												OnClick = o=>runner.StopAll(p)
											}
										})
									.ToArray()
					} )
				.Concat(new[]{new UMenuItem
					{
						Header = "Refresh",
						OnClick = o=>Context.RebuildMenu()
					} })
				.ToArray();
		}

		private static void DoWithProgress(string title, Action op)
		{
			DialogsCache.ShowProgress(u=>op(), title);
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = Context.GetSystemIcon(71);
			runner = new Runner(context);
		}
	}
}
