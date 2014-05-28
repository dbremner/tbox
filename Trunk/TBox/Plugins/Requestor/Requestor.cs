using System;
using System.Linq;
using System.Windows;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Core.PluginsShared.LoadTesting.Components;
using Mnk.TBox.Locales.Localization.Plugins.Requestor;
using Mnk.TBox.Plugins.Requestor.Code;
using Mnk.TBox.Plugins.Requestor.Code.Settings;
using Mnk.TBox.Plugins.Requestor.Components;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfSyntaxHighlighter;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.Requestor
{
	[PluginInfo(typeof(RequestorLang), 13, PluginGroup.Web)]
	public sealed class Requestor : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly LazyDialog<FormLoadTesting> formDdos;
		private readonly Lazy<Executor> executor;

		public Requestor()
		{
			formDdos = new LazyDialog<FormLoadTesting>(CreateForm, "ddos");
			executor = new Lazy<Executor>(() => new Executor());
		}

		private FormLoadTesting CreateForm()
		{
			var dialog = new FormLoadTesting();
			dialog.Init(Icon.ToImageSource(), Icon, new LoadTester());
			return dialog;
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config.Profiles
				.Select(p => new UMenuItem
				{
					Header = p.Key,
					Items = p.Ops
						.Select(x => new UMenuItem
						{
							Header = x.Key,
							OnClick = o => executor.Value.Execute(Application.Current.MainWindow, x, Config, null, Icon.ToImageSource())
						})
						.Concat(
						new[]
							{
								new USeparator(), 
								new UMenuItem{
									IsEnabled = p.Ops.Count>0,
									Header = RequestorLang.Ddos, 
									OnClick = o=>RunDdos(p)
								}, 
							}
						).ToArray()
				}).ToArray();
		}

		private void RunDdos(Profile profile)
		{
			formDdos.Do(Context.DoSync, d => d.ShowDialog(profile), Config.States);
		}

		public void Dispose()
		{
			if(executor.IsValueCreated)executor.Value.Dispose();
			formDdos.Dispose();
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		protected override Settings CreateSettings()
		{
			var s = base.CreateSettings();
			s.Requestor = new Lazy<FormRequest>(()=>FillHelpInfo(new FormRequest(Icon.ToImageSource())));
			return s;
		}

		private FormRequest FillHelpInfo(FormRequest requestor)
		{
			requestor.KnownHeaderValues.Clear();
			requestor.KnownHeaderValues.Clear();
			requestor.KnownUrls.Clear();
			foreach (var item in Config.Profiles.SelectMany(p => p.Ops))
			{
				requestor.KnownUrls.AddIfNotExist(item.Request.Url);
				foreach (var h in item.Request.Headers)
				{
					requestor.KnownHeaderNames.AddIfNotExist(h.Key);
					requestor.KnownHeaderValues.AddIfNotExist(h.Value);
				}
			}
			return requestor;
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!autoSaveOnExit) return;
			formDdos.SaveState(Config.States);
			if(executor.IsValueCreated)executor.Value.Save(Config);
		}

	}
}
