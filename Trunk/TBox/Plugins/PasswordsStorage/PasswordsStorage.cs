using System;
using System.Linq;
using System.Windows;
using Mnk.Library.Common.Log;
using Mnk.Library.WpfControls;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.Library.WpfWinForms;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Core.Contracts;
using Mnk.TBox.Locales.Localization.Plugins.PasswordsStorage;
using Mnk.TBox.Plugins.PasswordsStorage.Code;
using Mnk.TBox.Plugins.PasswordsStorage.Code.Settings;
using Mnk.TBox.Plugins.PasswordsStorage.Components;

namespace Mnk.TBox.Plugins.PasswordsStorage
{
    [PluginInfo(typeof(PasswordsStorageLang), 104, PluginGroup.Desktop)]
    public class PasswordsStorage : ConfigurablePlugin<Settings, Config>
    {
        private readonly ILog log = LogManager.GetLogger<PasswordsStorage>();
        private readonly LazyDialog<Dialog> dialog;
        private readonly IPasswordGenerator passwordGenerator;

        public PasswordsStorage()
        {
            passwordGenerator = new PasswordGenerator();
            dialog = new LazyDialog<Dialog>(CreateDialog);
        }

        private Dialog CreateDialog()
        {
            return new Dialog(passwordGenerator){Icon = Icon.ToImageSource()};
        }

        public override void OnRebuildMenu()
        {
            base.OnRebuildMenu();
            Menu = Config.Profiles
                .Select(p => new UMenuItem
                {
                    Header = p.Key,
                    OnClick = o=>ShowDialog(p,null)
                })
                .Concat(new []
                {
                    new USeparator(), 
                    new UMenuItem
                    {
                        Header = PasswordsStorageLang.NewPassword,
                        OnClick = o=>NewPassword()
                    }, 
                })
                .ToArray();
        }

        private void NewPassword()
        {
            try
            {
                passwordGenerator.Generate(Config.PasswordLength, Config.PasswordNonAlphaCharacters);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Unexpected issue");
            }
        }

        private void ShowDialog(Profile p, Window owner)
        {
            dialog.Do(Context.DoSync,
                d => d.ShowDialog(ConfigManager, p, Context, owner), 
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
