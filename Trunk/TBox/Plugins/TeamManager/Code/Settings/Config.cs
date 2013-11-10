using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.UI.Model;
using Interface;
using ScriptEngine;
using WPFControls.Dialogs.StateSaver;

namespace TeamManager.Code.Settings
{
	public class Config : IConfigWithDialogStates
	{
        public IDictionary<string, DialogState> States { get; set; }
        public string SelectedProfile { get; set; }
        public ObservableCollection<Profile> Profiles { get; set; }
        public ObservableCollection<SpecialDay> SpecialDays { get; set; }

		public Config()
		{
		    SelectedProfile = string.Empty;
			Profiles = new ObservableCollection<Profile>();
            States = new Dictionary<string, DialogState>();
            SpecialDays = new ObservableCollection<SpecialDay>();
		}
	}
}
