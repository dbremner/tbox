using System;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.TBox.Plugins.PasswordsStorage.Code.Settings
{
    [Serializable]
    public class Profile : Data
    {
        public CheckableDataCollection<CheckableData<string>> Passwords { get; set; }

        public Profile()
        {
            Passwords = new CheckableDataCollection<CheckableData<string>>();
        }

        public override object Clone()
        {
            return new Profile
            {
                Key = Key,
                Passwords = Passwords.Clone()
            };
        }
    }
}
