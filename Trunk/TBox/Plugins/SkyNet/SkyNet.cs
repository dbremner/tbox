using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using LightInject;
using Mnk.Library.Common.Log;
using Mnk.Library.ScriptEngine;
using Mnk.Library.WpfControls;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.SkyNet;
using Mnk.TBox.Core.PluginsShared.ScriptEngine;
using Mnk.TBox.Plugins.SkyNet.Code;
using Mnk.TBox.Plugins.SkyNet.Code.Interfaces;
using Mnk.TBox.Plugins.SkyNet.Code.Settings;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Tools.SkyNet.Common;

namespace Mnk.TBox.Plugins.SkyNet
{
    [PluginInfo(typeof(SkyNetLang), 18, PluginGroup.Development)]
    public class SkyNet : ConfigurablePlugin<Settings, Config>, IDisposable
    {
        private readonly ILog log = LogManager.GetLogger<SkyNet>();
        private readonly SkyNetScriptConfigurator scriptConfigurator = new SkyNetScriptConfigurator();
        private readonly LazyDialog<EditorDialog> editorDialog;
        private ITaskExecutor taskExecutor;
        private IServiceContainer container;

        public SkyNet()
        {
            editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
        }

        public override void Init(IPluginContext context)
        {
            base.Init(context);
            container = ServicesRegistrator.Register(context);
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
                    OnClick = o => DoExecute(x)
                })
                .Concat(
                    new[]
			        {
                        new USeparator(), 
				        new UMenuItem
				        {
					        Header = SkyNetLang.ScriptEditor,
					        OnClick = OpenEditor
				        }
			        })
                .ToArray();
        }

        private void DoExecute(SingleFileOperation x)
        {
            try
            {
                var id = taskExecutor.Execute(x);
                MessageBox.Show(id);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected error");
            }
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
            s.SettingsLogic = container.GetInstance<ISettingsLogic>();
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
