﻿using System;
using System.Collections.Generic;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace BookletPagesGenerator.Code
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public int PagesOffset { get; set; }
		public int TotalPages { get; set; }
		public int PagesToPrint { get; set; }
		public int PrintPageId { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			PagesOffset = 0;
			TotalPages = 800;
			PagesToPrint = 40;
			PrintPageId = 0;
			States = new Dictionary<string, DialogState>();
		}

	}
}
