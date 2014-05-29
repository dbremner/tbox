using System.Linq;
using System.Windows;
using Mnk.Library.WpfControls;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.PasswordsStorage;
using Mnk.TBox.Plugins.PasswordsStorage.Code.Settings;
using Mnk.TBox.Plugins.PasswordsStorage.Components;

namespace Mnk.TBox.Plugins.PasswordsStorage
{
    [PluginInfo(typeof(PasswordsStorageLang), 104, PluginGroup.Desktop)]
    public class PasswordsStorage : ConfigurablePlugin<Settings, Config>
    {
        private readonly LazyDialog<Dialog> dialog;

        public PasswordsStorage()
        {
            dialog = new LazyDialog<Dialog>(CreateDialog, "passwords-dialog");
        }

        private Dialog CreateDialog()
        {
            return new Dialog{Icon = Icon.ToImageSource()};
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
            dialog.Do(Context.DoSync, 
                d => d.ShowDialog(ConfigManager, p, owner), 
                ConfigManager.Config.States);
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
