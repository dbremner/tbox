using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mnk.Library.WPFControls.Dialogs.StateSaver;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Plugins.Notes.Code.Settings
{
    [Serializable]
    public class Config : IConfigWithDialogStates
    {
        public IDictionary<string, DialogState> States { get; set; }
        public ObservableCollection<Profile> Profiles { get; set; }
        public string SelectedProfile { get; set; }

        public Config()
        {
            Profiles = new ObservableCollection<Profile>();
            States = new Dictionary<string, DialogState>();
        }

    }
}
