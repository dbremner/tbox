﻿using System;
using System.Collections.Generic;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace Searcher.Code.Settings
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public IndexSettings Index { get; set; }
		public SearchConfig Search { get; set; }
		public ResultConfig Result { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			Index = new IndexSettings();
			Search = new SearchConfig();
			Result = new ResultConfig();
			States = new Dictionary<string, DialogState>();
		}
	}
}