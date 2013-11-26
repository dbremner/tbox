using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Automator.Code;
using Automator.Code.Settings;
using Common.Base.Log;
using Common.MT;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.Automator;
using PluginsShared.Automator;
using PluginsShared.ScriptEngine;
using ScriptEngine;
using ScriptEngine.Core;
using WPFControls.Code;
using WPFControls.Code.OS;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFSyntaxHighlighter;
using WPFWinForms;
using WPFWinForms.Icons;

namespace Automator
{
	[PluginInfo(typeof(AutomatorLang), 12, PluginGroup.Development)]
	public sealed class Automater : ConfigurablePlugin<Settings, Config>, IDisposable
	{
	    private readonly ScriptRunner scriptRunner = new ScriptRunner();
		private readonly LazyDialog<EditorDialog> editorDialog;
		private readonly LazyDialog<ScriptsRunner> runnerDialog;
		private static readonly ILog Log = LogManager.GetLogger<Automater>();
		public Automater()
		{
			editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
            runnerDialog = new LazyDialog<ScriptsRunner>(CreateRunner, "runnerDialog");
		}

		public void Dispose()
		{
			runnerDialog.Dispose();
			editorDialog.Dispose();
		}

		private EditorDialog CreateEditor()
		{
			return new EditorDialog { Context = Context, Icon = Icon.ToImageSource() };
		}

        private ScriptsRunner CreateRunner()
		{
            return new ScriptsRunner { Context = Context, Icon = Icon.ToImageSource() };
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
		}

		public override void OnRebuildMenu()
		{
			base.OnRebuildMenu();
			Menu = Config.Profiles.Where(x => x.Operations.Count > 0)
				.Select(p => new UMenuItem
					{
						Header = p.Key,
						Items = p.Operations.Select(o => new UMenuItem
							{
								Header = o.Key,
								OnClick = x=>DoWork(o, x)
							})
							.Concat(new[]
								{
									new USeparator(), 
									new UMenuItem
										{
											Header = AutomatorLang.RunAll,
											OnClick = x=>RunAll(p)
										}
								})
							.ToArray()
					})
					.Concat(new[]
						{
							new USeparator(), 
							new UMenuItem{Header = AutomatorLang.Editor, OnClick = ShowIde}
						})
					.ToArray();
		}

		private void RunAll(Profile profile)
		{
            DialogsCache.ShowProgress(u => RunAll(u, profile.Operations), AutomatorLang.RunScript + ": " + profile.Key, null, false, true, Icon.ToImageSource());
		}
		 
		private void RunAll(IUpdater u, IList<MultiFileOperation> operations)
		{
			var i = 0;
			var count = operations.Sum(x => x.Pathes.CheckedItems.Count());
			var folder = Context.DataProvider.ReadOnlyDataPath;
			foreach (var op in operations)
			{
			    var r = new ScriptRunner();
				foreach (var path in op.Pathes.CheckedItems)
				{
					if (u.UserPressClose) return;
					u.Update(path.Key, i++/(float) count);
					var result = false;
				    var key = path.Key;
				    var p = op.Parameters;
                    Execute(() => r.Run(Path.Combine(folder, key), p, a=>Context.DoSync(a), new NullUpdater()), out result);
					if(!result)return;
				}
			}
		}

		private static void Execute(Action action, out bool result)
		{
			result = false;
			try
			{
				action();
				result = true;
			}
			catch (CompilerExceptions cex)
			{
				Log.Write(cex.ToString());
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Error running script");
			}
		}

		private void ShowIde(object o)
		{
            editorDialog.Do(Context.DoSync, x => x.ShowDialog(GetPathes(), scriptRunner), Config.States);
		}

		private void DoWork(MultiFileOperation operation, object context)
		{
			if (context is NonUserRunContext)
			{
				RunAll(new NullUpdater(), new[] {operation});
			}
			else
			{
				runnerDialog.LoadState(Config.States);
                runnerDialog.Value.ShowDialog(operation, scriptRunner, null);
			}
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!autoSaveOnExit) return;
			runnerDialog.SaveState(Config.States);
			editorDialog.SaveState(Config.States);
		}

		protected override Settings CreateSettings()
		{
			var s = base.CreateSettings();
			s.FilePathesGetter = GetPathes;
			return s;
		}

		private IList<string> GetPathes()
		{
			var dir = new DirectoryInfo(Context.DataProvider.ReadOnlyDataPath);
			if(!dir.Exists)return new string[0];
			var length = dir.FullName.Length + 1;
			return dir
				.EnumerateFiles("*.cs", SearchOption.AllDirectories)
				.Select(x => x.FullName.Substring(length))
				.ToArray();
		}
	}
}
