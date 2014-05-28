using System.Linq;
using System.Windows;
using Mnk.Library.WPFControls.Code;
using Mnk.Library.WPFControls.Dialogs.StateSaver;
using Mnk.Library.WPFSyntaxHighlighter;
using Mnk.Library.WPFWinForms;
using Mnk.Library.WPFWinForms.Icons;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.Notes;
using Mnk.TBox.Plugins.Notes.Code.Settings;
using Mnk.TBox.Plugins.Notes.Components;

namespace Mnk.TBox.Plugins.Notes
{
    [PluginInfo(typeof(NotesLang), 1, PluginGroup.Desktop)]
    public class Notes : ConfigurablePlugin<Settings, Config>
    {
        private readonly LazyDialog<Dialog> dialog;

        public Notes()
        {
            dialog = new LazyDialog<Dialog>(CreateDialog, "notes-dialog");
        }

        private Dialog CreateDialog()
        {
            return new Dialog{Icon = Icon.ToImageSource()};
        }

        public override void Init(IPluginContext context)
        {
            base.Init(context);
            context.AddTypeToWarmingUp(typeof(SyntaxHighlighter));
        }

        public override void OnRebuildMenu()
        {
            base.OnRebuildMenu();
            Menu = Config.Profiles
                .Select(p => new UMenuItem
                {
                    Header = p.Key,
                    OnClick = o=>ShowDialog(p,null)
                }).ToArray();
        }
         
        private void ShowDialog(Profile p, Window owner)
        {
            dialog.Do(Context.DoSync, d => d.ShowDialog(p, owner), ConfigManager.Config.States);
        }

        protected override Settings CreateSettings()
        {
            var s = base.CreateSettings();
            s.EditHandler += p => ShowDialog(p, Application.Current.MainWindow);
            return s;
        }

        public override void Save(bool autoSaveOnExit)
        {
            base.Save(autoSaveOnExit);
            if (autoSaveOnExit) dialog.SaveState(ConfigManager.Config.States);
        }

        public virtual void Dispose()
        {
            dialog.Dispose();
        }
    }
}
