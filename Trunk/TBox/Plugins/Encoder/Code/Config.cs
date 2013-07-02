using System;
using System.Collections.Generic;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace Encoder.Code
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public string SourceText { get; set; }
		public int SelectedEncoder { get; set; }
		public bool ConvertOnSourceChanged { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			ConvertOnSourceChanged = true;
			States = new Dictionary<string, DialogState>();
		}

	}
}
