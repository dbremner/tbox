using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mnk.Library.WPFControls.Tools;
using Mnk.Library.WPFWinForms;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.Interface.Atrributes;
using Mnk.TBox.Locales.Localization.Plugins.PasswordsStorage;
using Mnk.TBox.Plugins.PasswordsStorage.Code.Settings;

namespace Mnk.TBox.Plugins.PasswordsStorage
{
    [PluginInfo(typeof(PasswordsStorageLang), 104, PluginGroup.Desktop)]
    public class PasswordsStorage : ConfigurablePlugin<Settings, Config>
    {
        public override void OnRebuildMenu()
        {
            base.OnRebuildMenu();
            Menu = Config.Profiles
                .Select(p => new UMenuItem
                {
                    Header = p.Key,
                    Items = p.Passwords.Select(x=>new UMenuItem
                    {
                        Header = x.Key,
                        OnClick = o => Clipboard.SetText(x.Value.DecryptPassword())
                    }).ToArray()
                }).ToArray();
        }
    }
}
