﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.TeamManager;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.TBox.Plugins.TeamManager.Code;
using Mnk.TBox.Plugins.TeamManager.Code.Scripts;
using Mnk.TBox.Plugins.TeamManager.Code.Settings;
using Mnk.TBox.Plugins.TeamManager.Forms;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.TeamManager
{
    [PluginInfo(typeof(TeamManagerLang), 160, PluginGroup.Development)]
    public class TeamManager : ConfigurablePlugin<Settings, Config>, IDisposable
    {
        private const string DataProvidersFolder = "DataProviders";
        private const string ValidatorsFolder = "Validators";
        private ReportReceiver receiver;
        private readonly ValidatorScriptConfigurator validatorScriptConfigurator = new ValidatorScriptConfigurator();
        private readonly ReportScriptRunner reportScriptRunner = new ReportScriptRunner();
        private readonly LazyDialog<EditorDialog> dataProvidersEditorDialog;
        private readonly LazyDialog<TimeReportDialog> timeReportDialog;

        public TeamManager()
        {
            timeReportDialog = new LazyDialog<TimeReportDialog>(() => new TimeReportDialog(receiver) { Icon = Icon.ToImageSource() }, "timeReportDialog");
            dataProvidersEditorDialog = new LazyDialog<EditorDialog>(CreateEditor, "dataProvidersEditorDialog");
        }

        public void Dispose()
        {
            dataProvidersEditorDialog.Dispose();
            timeReportDialog.Dispose();
        }

        public override void Load()
        {
            base.Load();
            timeReportDialog.Hide();
            dataProvidersEditorDialog.Hide();
        }

        public override void Save(bool autoSaveOnExit)
        {
            base.Save(autoSaveOnExit);
            timeReportDialog.Hide();
            dataProvidersEditorDialog.Hide();
            if (!autoSaveOnExit) return;
            timeReportDialog.SaveState(Config.States);
            dataProvidersEditorDialog.SaveState(Config.States);
        }


        private EditorDialog CreateEditor()
        {
            return new EditorDialog { Context = Context, Icon = Icon.ToImageSource() };
        }

        public override void Init(IPluginContext context)
        {
            base.Init(context);
            receiver = new ReportReceiver(context.DataProvider.ReadOnlyDataPath);
        }

        public override void OnRebuildMenu()
        {
            Menu = Config.Profiles.Select(
                x => new UMenuItem
                {
                    Header = x.Key,
                    OnClick = o => GetTimeTable(o, x)
                })
                .Concat(
                    new[]
			        {
                        new USeparator(), 
				        new UMenuItem
				        {
					        Header = TeamManagerLang.ProvidersScriptEditor,
					        OnClick = OpenProvidersEditor
				        },
                        new UMenuItem
				        {
					        Header = TeamManagerLang.ValidatorsScriptEditor,
					        OnClick = OpenValidatorsEditor
				        }
			        })
                .ToArray();
        }

        private void GetTimeTable(object o, Profile p)
        {
            timeReportDialog.LoadState(Config.States);
            timeReportDialog.Value.ShowReportDialog(p, Context, Config.SpecialDays, o is NonUserRunContext);
        }

        private void OpenProvidersEditor(object o)
        {
            dataProvidersEditorDialog.Do(Context.DoSync,
                x=>x.ShowDialog(GetPaths(DataProvidersFolder), reportScriptRunner),
                Config.States);
        }

        private void OpenValidatorsEditor(object o)
        {
            dataProvidersEditorDialog.Do(Context.DoSync,
                x=>x.ShowDialog(GetPaths(ValidatorsFolder), validatorScriptConfigurator),
                Config.States
                );
        }

        protected override Settings CreateSettings()
        {
            var imageSource = Icon.ToImageSource();
            var s = base.CreateSettings();
            s.FilePaths = GetPaths(DataProvidersFolder);
            s.ScriptConfigurator = reportScriptRunner;
            s.ScriptConfiguratorDialog = new LazyDialog<ScriptsConfigurator>(
                () => new SingleFileScriptConfigurator { Context = Context, Icon = imageSource }, "scripts configurator");
            return s;
        }

        private IList<string> GetPaths(string directoryName)
        {
            var dir = new DirectoryInfo(Context.DataProvider.ReadOnlyDataPath);
            if (!dir.Exists) return new string[0];
            var length = dir.FullName.Length + 1;
            return dir
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Where(x => !string.IsNullOrEmpty(x.DirectoryName) && x.DirectoryName.EndsWith(directoryName))
                .Select(x => x.FullName.Substring(length))
                .ToArray();
        }

    }
}
