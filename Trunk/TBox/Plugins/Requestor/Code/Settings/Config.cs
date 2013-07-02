using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace Requestor.Code.Settings
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public string SelectedProfile { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			Profiles = new ObservableCollection<Profile>();
			States = new Dictionary<string, DialogState>();
		}

	}
}
