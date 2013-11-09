using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.TeamManager;
using PluginsShared.ScriptEngine;
using TeamManager.Code;
using TeamManager.Code.Scripts;
using TeamManager.Code.Settings;
using TeamManager.Forms;
using WPFControls.Code;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;

namespace TeamManager
{
	[PluginInfo(typeof(TeamManagerLang), 160, PluginGroup.Development)]
	public class TeamManager : ConfigurablePlugin<Settings, Config>, IDisposable
	{
	    private ReportReceiver receiver;
        private readonly ReportScriptRunner reportScriptRunner = new ReportScriptRunner();
        private readonly LazyDialog<EditorDialog> editorDialog;
	    private readonly LazyDialog<TimeReportDialog> timeReportDialog;

	    public TeamManager()
	    {
            timeReportDialog = new LazyDialog<TimeReportDialog>(()=> new TimeReportDialog(receiver) { Icon = ImageSource }, "timeReportDialog");
            editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
	    }

        public void Dispose()
        {
            editorDialog.Dispose();
            timeReportDialog.Dispose();
        }

        public override void Save(bool autoSaveOnExit)
        {
            base.Save(autoSaveOnExit);
            if (!autoSaveOnExit) return;
            timeReportDialog.SaveState(Config.States);
            editorDialog.SaveState(Config.States);
        }


	    private EditorDialog CreateEditor()
	    {
            return new EditorDialog { Context = Context, Icon = ImageSource };
	    }

	    public override void Init(IPluginContext context)
        {
            base.Init(context);
            receiver = new ReportReceiver(context.DataProvider.ReadOnlyDataPath);
        }

		public override void OnRebuildMenu()
		{
			Menu = Config.Profiles.Select(
                x=>new UMenuItem
				{
					Header = x.Key,
					OnClick = o=>GetTimeTable(o, x)
				})
                .Concat(
                    new []
			        {
                        new USeparator(), 
				        new UMenuItem
				        {
					        Header = TeamManagerLang.ScriptEditor,
					        OnClick = OpenEditor
				        }
			        })
                .ToArray();
		}
        
	    private void GetTimeTable(object obj, Profile p)
		{
            timeReportDialog.LoadState(Config.States);
            timeReportDialog.Value.ShowReportDialog(p, Context, Config.SpecialDays);
		}

        private void OpenEditor(object obj)
        {
            editorDialog.LoadState(Config.States);
            editorDialog.Value.ShowDialog(GetPathes(), reportScriptRunner);
        }

        protected override Settings CreateSettings()
        {
            var s = base.CreateSettings();
            s.FilePathesGetter = GetPathes;
            s.ReportScriptRunner = reportScriptRunner;
            s.OperationsDialog = new LazyDialog<OperationDialog>(
                () => new OperationDialog{Icon = ImageSource}, "report operation");
            s.ScriptsConfigurator = new LazyDialog<ScriptsConfigurator>(
                () => new ScriptsConfigurator{Context = Context, Icon = ImageSource}, "scripts configurator");
            return s;
        }

        private IList<string> GetPathes()
        {
            var dir = new DirectoryInfo(Context.DataProvider.ReadOnlyDataPath);
            if (!dir.Exists) return new string[0];
            var length = dir.FullName.Length + 1;
            return dir
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(x => x.FullName.Substring(length))
                .ToArray();
        }

	}
}
