using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace RegExpTester
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public bool TestManual { get; set; }
		public string RegExp { get; set; }
		public string Text { get; set; }
		public int Options { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			Options = (int)RegexOptions.Compiled;
			States = new Dictionary<string, DialogState>();
		}
	}
}
