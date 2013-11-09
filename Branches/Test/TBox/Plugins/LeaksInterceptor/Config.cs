using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using Interface;
using WPFControls.Dialogs.PerfomanceCounters;
using WPFControls.Dialogs.StateSaver;

namespace LeaksInterceptor
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public int RefreshViewInterval { get; set; }
		public int RefreshDataInterval { get; set; }
		public string LastProcessName { get; set; }
		public CheckableDataCollection<CheckableData> Counters { get; set; }
		public ObservableCollection<SelectedEntity> PerfomanceCounters { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			RefreshViewInterval = 1000;
			RefreshDataInterval = 1000;
			Counters = new CheckableDataCollection<CheckableData>();
			PerfomanceCounters = new ObservableCollection<SelectedEntity>();
			States = new Dictionary<string, DialogState>();
		}

	}
}
