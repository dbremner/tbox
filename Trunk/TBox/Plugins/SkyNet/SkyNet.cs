using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interface;
using Interface.Atrributes;
using Localization.Plugins.SkyNet;
using PluginsShared.ScriptEngine;
using SkyNet.Code.Settings;
using SkyNet.Common.Scripts;
using WPFControls.Code;
using WPFControls.Dialogs.StateSaver;
using WPFWinForms;
using WPFWinForms.Icons;

namespace SkyNet
{
    [PluginInfo(typeof(SkyNetLang), 18, PluginGroup.Development)]
    public class SkyNet : ConfigurablePlugin<Settings, Config>
    {
        private readonly SkyNetScriptRunner scriptConfigurator = new SkyNetScriptRunner();
        private readonly LazyDialog<EditorDialog> editorDialog;

        public SkyNet()
        {
            editorDialog = new LazyDialog<EditorDialog>(CreateEditor, "editorDialog");
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
                    OnClick = o => { }
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
