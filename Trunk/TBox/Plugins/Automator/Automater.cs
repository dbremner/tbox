using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Automator.Code;
using Automator.Code.Settings;
using Automator.Components;
using Common.Base.Log;
using Common.MT;
using Interface;
using Interface.Atrributes;
using ScriptEngine;
using ScriptEngine.Core;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFControls.Tools;
using WPFSyntaxHighlighter;
using WPFWinForms;

namespace Automator
{
	[PluginName("Automater")]
	[PluginDescription("Simple tool to automate everything what is possible. :)")]
	public sealed class Automater : ConfigurablePlugin<Settings, Config>, IDisposable
	{
		private readonly LazyDialog<EditorDialog> editor;
		private readonly LazyDialog<ScriptsRunner> runner;
		private static readonly ILog Log = LogManager.GetLogger<Automater>();
		public Automater()
		{
			editor = new LazyDialog<EditorDialog>(CreateEditor, "editor");
			runner = new LazyDialog<ScriptsRunner>(CreateRunner, "runner");
		}

		public void Dispose()
		{
			runner.Dispose();
			editor.Dispose();
		}

		private EditorDialog CreateEditor()
		{
			var d = new EditorDialog { Context = Context };
			d.SetIcon(Icon);
			return d;
		}

		private ScriptsRunner CreateRunner()
		{
			var d = new ScriptsRunner { Context = Context };
			d.SetIcon(Icon);
			return d;
		}

		public override void Init(IPluginContext context)
		{
			base.Init(context);
			context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
			context.AddTypeToWarmingUp(typeof(ScriptCompiler));
			Icon = Context.GetSystemIcon(12);
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
											Header = "Run all",
											OnClick = x=>RunAll(p)
										}
								})
							.ToArray()
					})
					.Concat(new[]
						{
							new USeparator(), 
							new UMenuItem{Header = "Editor", OnClick = ShowIde}
						})
					.ToArray();
		}

		private void RunAll(Profile profile)
		{
			DialogsCache.ShowProgress(u=>RunAll(u, profile.Operations), "Run script: " + profile.Key, topmost:false, showInTaskBar:true);
		}
		 
		private void RunAll(IUpdater u, IList<Operation> operations)
		{
			var i = 0;
			var count = operations.Sum(x => x.Pathes.CheckedItems.Count());
			var folder = Context.DataProvider.ReadOnlyDataPath;
			foreach (var op in operations)
			{
				using (var r = new Runner(folder, op.Parameters))
				{
					foreach (var path in op.Pathes.CheckedItems)
					{
						if (u.UserPressClose) return;
						u.Update(path.Key, i++/(float) count);
						var result = false;
						r.Run(Path.Combine(folder, path.Key), a => Context.DoSync(() => Execute(a, out result)));
						if(!result)return;
					}
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
			editor.Do(Context.DoSync, x=>x.ShowDialog(GetPathes()), Config.States);
		}

		private void DoWork(Operation operation, object context)
		{
			if (context is NonUserRunContext)
			{
				RunAll(new NullUpdater(), new[] {operation});
			}
			else
			{
				runner.LoadState(Config.States);
				runner.Value.ShowDialog(operation);
			}
		}

		public override void Save(bool autoSaveOnExit)
		{
			base.Save(autoSaveOnExit);
			if (!autoSaveOnExit) return;
			runner.SaveState(Config.States);
			editor.SaveState(Config.States);
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
