using System;
using AvailabilityChecker.Code;
using Interface;
using Interface.Atrributes;
using WPFWinForms;

namespace AvailabilityChecker
{
	[PluginName("Availability checker")]
	[PluginDescription("Check by timer, availability of the shared folders and web sites.")]
	public sealed class AvailabilityChecker : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly Lazy<Worker> worker;

		public AvailabilityChecker()
		{
			worker = new Lazy<Worker>(() =>
				{
					var w = new Worker();
					w.Load(Config);
					return w;
				});
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			Icon = context.GetSystemIcon(10);
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			var enabled = Config.Items.CheckedValuesCount > 0 || Config.Started;
			Menu = new[] {
					new UMenuItem {
							IsEnabled = enabled,
							Header = Config.Started ? "Stop" : "Start",
							OnClick = o =>
								{
									worker.Value.OnStartStop();
									Context.RebuildMenu();
								}
						}
				};
			if (worker.IsValueCreated)
			{
				worker.Value.OnUpdateMenu(Menu);
			}
		}

		public override void Load()
		{
			base.Load();
			if (!Config.Started && !worker.IsValueCreated) return;
			worker.Value.Load(Config);
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!Config.Started && !worker.IsValueCreated) return;
			worker.Value.Save(Config, autoSaveOnExit);
		}

		public void Dispose()
		{
			if (worker.IsValueCreated)
			{
				worker.Value.Dispose();
			}
		}
	}
}
