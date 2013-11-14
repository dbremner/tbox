using System;
using System.Linq;
using System.Windows;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.SqlRunner;
using PluginsShared.Ddos.Components;
using SqlRunner.Code;
using SqlRunner.Code.Settings;
using SqlRunner.Components;
using WPFControls.Code;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;
using WPFWinForms;

namespace SqlRunner
{
	[PluginInfo(typeof(SqlRunnerLang), 8, PluginGroup.Database)]
	public sealed class SqlRunner : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private Ddoser ddoser;
		private readonly LazyDialog<FormDdos> formDdos;
		private readonly LazyDialog<FormBatch> formBatch;
		private readonly Lazy<Executor> executor;

		public SqlRunner()
		{
			formDdos = new LazyDialog<FormDdos>(CreateDdosForm, "ddos");
			formBatch = new LazyDialog<FormBatch>(CreateBatchForm, "batch");
			executor = new Lazy<Executor>(() => new Executor());
		}

		private FormDdos CreateDdosForm()
		{
			var dialog = new FormDdos();
			dialog.Init(ImageSource, Icon, ddoser = new Ddoser());
			ddoser.OnConfigUpdated(Config);
			return dialog;
		}

		private FormBatch CreateBatchForm()
		{
			return new FormBatch{Icon=ImageSource};
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			if (formDdos.IsValueCreated)
			{
				ddoser.OnConfigUpdated(Config);
			}
			Menu = Config.Profiles
				.Select(p => new UMenuItem
				{
					Header = p.Key,
					Items = p.Ops
						.Select(o => new UMenuItem
						{
							Header = o.Key,
							OnClick = x => executor.Value.Execute(Application.Current.MainWindow, o, Config.ConnectionString, Config, null, ImageSource)
						})
						.Concat(
						new[]
							{
								new USeparator(), 
								new UMenuItem{
									IsEnabled = p.Ops.Count>0,
									Header = SqlRunnerLang.Ddos, 
									OnClick = x=>RunDdos(p)
								}, 
								new UMenuItem{
									IsEnabled = p.Ops.Count>0,
									Header = SqlRunnerLang.Batch, 
									OnClick = x=>RunBatch(p)
								}, 
							}
						).ToArray()
				}).ToArray();
		}

		private void RunBatch(Profile profile)
		{
			formBatch.Do(Context.DoSync, d => d.ShowDialog(profile, Config), Config.States);
		}

		private void RunDdos(Profile profile)
		{
			formDdos.Do(Context.DoSync, d => d.ShowDialog(profile), Config.States);
		}

		public void Dispose()
		{
			formDdos.Dispose();
			formBatch.Dispose();
			if(executor.IsValueCreated)executor.Value.Dispose();
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!autoSaveOnExit) return;
			formDdos.SaveState(Config.States);
			formBatch.SaveState(Config.States);
			if (formBatch.IsValueCreated)
			{
				formBatch.Value.Save(Config);
			}
			if (executor.IsValueCreated)
			{
				executor.Value.Save(Config);
			}
		}

		protected override Settings CreateSettings()
		{
			var s = base.CreateSettings();
			s.Requestor = new Lazy<FormRequest>(()=>new FormRequest());
			return s;
		}
	}
}

