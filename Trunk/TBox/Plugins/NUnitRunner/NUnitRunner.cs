using System;
using System.IO;
using System.Linq;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.NUnitRunner;
using NUnitRunner.Code.Settings;
using NUnitRunner.Components;
using WPFControls.Code;
using WPFControls.Dialogs;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using WPFWinForms.Icons;

namespace NUnitRunner
{
    [PluginInfo(typeof(NUnitRunnerLang), typeof(Properties.Resources), PluginGroup.Development)]
    public sealed class NUnitRunner : ConfigurablePlugin<Settings, Config>, IDisposable
    {
        private readonly LazyDialog<Dialog> runner;

        public NUnitRunner()
        {
            runner = new LazyDialog<Dialog>(CreateDialog, "simple");
        }

        private Dialog CreateDialog()
        {
            return new Dialog(NUnitAgentPath, RunAsx86Path)
            {
                Icon = Icon.ToImageSource()
            };
        }

        public override void OnRebuildMenu()
        {
            base.OnRebuildMenu();
            Menu = Config.DllPathes.CheckedItems
                         .Select(x => new UMenuItem
                         {
                             Header = Path.GetFileName(x.Key),
                             OnClick = o => Run(x)
                         })
                         .ToArray();
        }

        private void Run(TestConfig config)
        {
            runner.LoadState(Config.States);
            runner.Value.ShowDialog(config);
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
