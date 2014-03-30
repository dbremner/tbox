using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.SkyNet;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code;
using Mnk.TBox.Plugins.SkyNet.Code.Settings;
using Mnk.TBox.Tools.SkyNet.Common.Scripts;
using Mnk.Library.WPFControls.Code;
using Mnk.Library.WPFControls.Dialogs.StateSaver;
using Mnk.Library.WPFWinForms;
using Mnk.Library.WPFWinForms.Icons;

namespace Mnk.TBox.Plugins.SkyNet
{
    [PluginInfo(typeof(SkyNetLang), 18, PluginGroup.Development)]
    public class SkyNet : ConfigurablePlugin<Settings, Config>
    {
        private readonly SkyNetScriptConfigurator scriptConfigurator = new SkyNetScriptConfigurator();
        private readonly LazyDialog<EditorDialog> editorDialog;
        private readonly TaskExecutor taskExecutor;
        private readonly ServicesFacade servicesFacade;

        public SkyNet()
        {
            servicesFacade = new ServicesFacade();
            editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
            taskExecutor = new TaskExecutor(servicesFacade);
        }

        private EditorDialog CreateEditor()
        {
            return new EditorDialog { Context = Context, Icon = Icon.ToImageSource() };
        }

        public override void OnRebuildMenu()
        {
            Menu = Config.Operations.CheckedItems.Select(
                x => new UMenuItem
                {
                    Header = x.Key,
                    OnClick = o => taskExecutor.Execute(x)
                })
                .Concat(
                    new[]
			        {
                        new USeparator(), 
				        new UMenuItem
				        {
					        Header = "{ScriptEditor}",
					        OnClick = OpenEditor
				        }
			        })
                .ToArray();
        }

        private void OpenEditor(object o)
        {
            editorDialog.LoadState(Config.States);
            editorDialog.Value.ShowDialog(GetPathes(), scriptConfigurator);
        }

        protected override Settings CreateSettings()
        {
            var s = base.CreateSettings();
            s.FilePathes = GetPathes();
            s.ScriptConfigurator = scriptConfigurator;
            s.ScriptConfiguratorDialog = new LazyDialog<ScriptsConfigurator>(
                ()=>new SingleFileScriptConfigurator{Context = Context, Icon = Icon.ToImageSource()}, "script configurator" );
            s.ServicesFacade = servicesFacade;
            s.Init(Context);
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
