using System;
using System.Collections.ObjectModel;

namespace Mnk.TBox.Plugins.PasswordsStorage.Code.Settings
{
    [Serializable]
    public class Config
    {
        public ObservableCollection<Profile> Profiles { get; set; }
        public string SelectedProfile { get; set; }

        public Config()
        {
            Profiles = new ObservableCollection<Profile>();
        }
    }
}
