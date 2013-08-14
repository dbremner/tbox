﻿using System;
using System.Collections.Generic;
using Common.UI.ModelsContainers;
using Interface;
using PluginsShared.Watcher;
using WPFControls.Dialogs.StateSaver;

namespace FileWatcher.Code
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public bool Started { get; set; }
		public int ToolTipsTimeOut { get; set; }
		public int RescanLogsInterval { get; set; }
		public int MaxEntriesInLog { get; set; }
		public bool ToolTipsEnabled { get; set; }
		public string EntryRegularExpression { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public WatchSettings Watches { get; set; }

		public Config()
		{
			Started = false;
			ToolTipsTimeOut = 5000;
			RescanLogsInterval = 1000;
			MaxEntriesInLog = 99;
			ToolTipsEnabled = true;
			Watches = new WatchSettings
			{
				Files = new CheckableDataCollection<DirInfo>
				{
					new DirInfo
					{
						Key = "sample",
						Path = "c:\\www\\logs",
						Mask = "*.log"
					}
				}
			};
			EntryRegularExpression = "\\d{4}-\\d{2}-\\d{2} \\d{2}:\\d{2}:\\d{2},\\d{3} \\[";
			States = new Dictionary<string, DialogState>();
		}

	}
}
