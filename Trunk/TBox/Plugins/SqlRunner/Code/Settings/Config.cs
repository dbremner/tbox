using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace SqlRunner.Code.Settings
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public string SelectedProfile { get; set; }
		public string ConnectionString { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }
		public CheckableDataCollection<CheckableData> ConnectionStrings { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			Profiles = new ObservableCollection<Profile>();
			ConnectionStrings = new CheckableDataCollection<CheckableData>();
			States = new Dictionary<string, DialogState>();
		}

	}
}
