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
        protected readonly LazyDialog<NotesDialog> Dialog;

        public Notes()
        {
            Dialog = new LazyDialog<NotesDialog>(CreateDialog, "notes-dialog");
        }

        private NotesDialog CreateDialog()
        {
            return new NotesDialog{Icon = Icon.ToImageSource()};
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
            Dialog.LoadState(ConfigManager.Config.States);
            Dialog.Do(Context.DoSync, d=>d.ShowDialog(p,owner));
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
            if (autoSaveOnExit) Dialog.SaveState(ConfigManager.Config.States);
        }

        public virtual void Dispose()
        {
            Dialog.Dispose();
        }
    }
}
