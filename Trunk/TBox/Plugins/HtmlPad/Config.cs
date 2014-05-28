using System;
using System.Collections.Generic;
using Mnk.TBox.Core.Interface;
using Mnk.Library.WpfControls.Dialogs.StateSaver;

namespace Mnk.TBox.Plugins.HtmlPad
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
