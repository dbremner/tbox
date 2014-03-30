using System;
using System.Collections.ObjectModel;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;

namespace Mnk.TBox.Plugins.Notes.Code.Settings
{
    [Serializable]
    public class Profile : Data
    {
        public ObservableCollection<Data<string>> Notes { get; set; }

        public Profile()
        {
            Notes = new ObservableCollection<Data<string>>();
        }

        public override object Clone()
        {
            return new Profile
            {
                Key = Key,
                Notes = Notes.Clone()
            };
        }
    }
}
