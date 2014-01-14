﻿using System;
using System.Collections.Generic;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.TBox.Core.Interface;
using Mnk.TBox.Core.PluginsShared.Watcher;
using Mnk.Library.WPFControls.Dialogs.StateSaver;

namespace Mnk.TBox.Plugins.SqlWatcher.Code
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public bool Started { get; set; }
		public int ToolTipsTimeOut { get; set; }
		public int RescanLogsInterval { get; set; }
		public bool ToolTipsEnabled { get; set; }
		public bool RemoveTypeInfo { get; set; }
		public IDictionary<string, DialogState> States { get; set; }
		public WatchSettings Watches { get; set; }

		public Config()
		{
			Started = false;
			ToolTipsTimeOut = 5000;
			RescanLogsInterval = 1000;
			ToolTipsEnabled = false;
		    RemoveTypeInfo = true;
			Watches = new WatchSettings
				{
					Files = new CheckableDataCollection<DirInfo>
						{
							new DirInfo
								{
									Key = "Sample",
									Path = "c:\\www\\logs",
									Mask = "nhibernate.log"
								}
						}
				};
			States = new Dictionary<string, DialogState>();
		}

	}
}
