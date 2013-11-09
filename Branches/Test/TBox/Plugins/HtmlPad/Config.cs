using System;
using System.Collections.Generic;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace HtmlPad
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public string Text { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			States = new Dictionary<string, DialogState>();
		}
	}
}
