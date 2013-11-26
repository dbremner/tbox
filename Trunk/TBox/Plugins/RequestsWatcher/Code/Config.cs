﻿using System;
using System.Collections.Generic;
using Common.UI.ModelsContainers;
using Interface;
using PluginsShared.Watcher;
using WPFControls.Dialogs.StateSaver;

namespace RequestsWatcher.Code
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public bool Started { get; set; }
		public int ToolTipsTimeOut { get; set; }
		public int RescanLogsInterval { get; set; }
		public bool ToolTipsEnabled { get; set; }
		public int MaxEntriesInLog { get; set; }
		public IDictionary<string, DialogState> States { get; set; }
		public WatchSettings Watches { get; set; }

		public Config()
		{
			Started = false;
			ToolTipsTimeOut = 5000;
			RescanLogsInterval = 100;
			ToolTipsEnabled = false;
			MaxEntriesInLog = 16;
			Watches = new WatchSettings
				{
					Files = new CheckableDataCollection<DirInfo>
						{
							new DirInfo
								{
									Key = "Sample",
									Path = "c:\\www\\trace",
									Mask = "*.trace"
								}
						}
				};
			States = new Dictionary<string, DialogState>();
		}

	}

}
