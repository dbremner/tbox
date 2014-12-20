﻿using System;
using System.IO;
using System.Linq;
using Mnk.Library.WpfControls;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.NUnitRunner;
using Mnk.TBox.Plugins.NUnitRunner.Code.Settings;
using Mnk.TBox.Plugins.NUnitRunner.Components;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Plugins.NUnitRunner.Code;

namespace Mnk.TBox.Plugins.NUnitRunner
{
    [PluginInfo(typeof(NUnitRunnerLang), typeof(Properties.Resources), PluginGroup.Development)]
    public sealed class NUnitRunner : ConfigurablePlugin<Settings, Config>, IDisposable
    {
        private readonly LazyDialog<Dialog> runner;

        public NUnitRunner()
        {
            runner = new LazyDialog<Dialog>(CreateDialog);
        }

        private Dialog CreateDialog()
        {
            return new Dialog(new TestsConfigurator(NUnitAgentPath, RunAsx86Path, Context.PathResolver), Context.PathResolver)
            {
                Icon = Icon.ToImageSource()
            };
        }

        public override void OnRebuildMenu()
        {
            base.OnRebuildMenu();
            Menu = Config.TestSuites.CheckedItems
                         .Select(x => new UMenuItem
                         {
                             Header = Path.GetFileName(Context.PathResolver.Resolve(x.Key)),
                             OnClick = o => Run(x)
                         })
                         .ToArray();
        }

        private void Run(TestSuiteConfig suiteConfig)
        {
            runner.Do(Context.DoSync, x => x.ShowDialog(suiteConfig), Config.States);
        }

        private string NUnitAgentPath
        {
            get { return Path.Combine(Context.DataProvider.ToolsPath, "NUnitAgent.exe"); }
        }

        private string RunAsx86Path
        {
            get { return Path.Combine(Context.DataProvider.ToolsPath, "RunAsx86.exe"); }
        }


        public override void Save(bool autoSaveOnExit)
        {
            base.Save(autoSaveOnExit);
            if (!autoSaveOnExit) return;
            runner.SaveState(Config.States);
        }

        public void Dispose()
        {
            runner.Dispose();
        }
    }
}
