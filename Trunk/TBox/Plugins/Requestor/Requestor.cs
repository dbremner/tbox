﻿using System;
using System.Linq;
using System.Windows;
using Common.Tools;
using Interface;
using Interface.Atrributes;
using PluginsShared.Ddos.Components;
using Requestor.Code;
using Requestor.Code.Settings;
using Requestor.Components;
using WPFControls.Code;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;
using WPFWinForms;

namespace Requestor
{
	[PluginName("Request maker")]
	[PluginDescription("Ability to build and send any requests. Also you can do DDos\nto find perfomance issues or memory leaks.")]
	public sealed class Requestor : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly LazyDialog<FormDdos> formDdos;
		private readonly Lazy<Executor> executor;

		public Requestor()
		{
			formDdos = new LazyDialog<FormDdos>(CreateForm, "ddos");
			executor = new Lazy<Executor>(() => new Executor());
		}

		private FormDdos CreateForm()
		{
			var dialog = new FormDdos();
			dialog.Init(Icon, new Ddoser());
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
							OnClick = o => executor.Value.Execute(Application.Current.MainWindow, x, Config)
						})
						.Concat(
						new[]
							{
								new USeparator(), 
								new UMenuItem{
									IsEnabled = p.Ops.Count>0,
									Header = "Ddos...", 
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
			Icon = context.GetSystemIcon(13);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		protected override Settings CreateSettings()
		{
			var s = base.CreateSettings();
			s.Requestor = new Lazy<FormRequest>(()=>FillHelpInfo(new FormRequest()));
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
