using System;
using System.Collections.Generic;
using Common.UI.ModelsContainers;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace NUnitRunner.Code.Settings
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public CheckableDataCollection<TestConfig> DllPathes { get; set; }
		public IDictionary<string, DialogState> States { get; set; }
		public bool ShowOnlyFailed { get; set; }
		public bool NeedSyncForBatch { get; set; }

		public Config()
		{
			DllPathes = new CheckableDataCollection<TestConfig>();
			States = new Dictionary<string, DialogState>();
			ShowOnlyFailed = true;
			NeedSyncForBatch = true;
		}

	}
}
