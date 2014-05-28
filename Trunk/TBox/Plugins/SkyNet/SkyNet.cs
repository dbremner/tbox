using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LightInject;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.SkyNet;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Plugins.SkyNet.Code.Settings;
using Mnk.TBox.Tools.SkyNet.Common.Scripts;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;

namespace Mnk.TBox.Plugins.SkyNet
{
    [PluginInfo(typeof(SkyNetLang), 18, PluginGroup.Development)]
    public class SkyNet : ConfigurablePlugin<Settings, Config>, IDisposable
    {
        private readonly IServiceContainer container;
        private readonly SkyNetScriptConfigurator scriptConfigurator = new SkyNetScriptConfigurator();
        private readonly LazyDialog<EditorDialog> editorDialog;
        private readonly ITaskExecutor taskExecutor;

        public SkyNet()
        {
            container = new ServicesRegistrator().Register();
            editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
            taskExecutor = container.GetInstance<ITaskExecutor>();
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
            editorDialog.Do(Context.DoSync, x=>x.ShowDialog(GetPaths(), scriptConfigurator),Config.States);
        }

        protected override Settings CreateSettings()
        {
            var s = base.CreateSettings();
            s.FilePaths = GetPaths();
            s.ScriptConfigurator = scriptConfigurator;
            s.ScriptConfiguratorDialog = new LazyDialog<ScriptsConfigurator>(
                ()=>new SingleFileScriptConfigurator{Context = Context, Icon = Icon.ToImageSource()}, "script configurator" );
            s.ServicesBuilder = container.GetInstance<IServicesBuilder>();
            s.ConfigsFacade = container.GetInstance<IConfigsFacade>();
            s.Init(Context);
            return s;
        }

        private IList<string> GetPaths()
        {
            var dir = new DirectoryInfo(Context.DataProvider.ReadOnlyDataPath);
            if (!dir.Exists) return new string[0];
            var length = dir.FullName.Length + 1;
            return dir
                .EnumerateFiles("*.cs", SearchOption.AllDirectories)
                .Select(x => x.FullName.Substring(length))
                .ToArray();
        }

        public void Dispose()
        {
            container.Dispose();
        }
    }

}
