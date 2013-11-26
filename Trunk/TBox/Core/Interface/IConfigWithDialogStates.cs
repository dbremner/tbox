using System.Collections.Generic;
using WPFControls.Dialogs.StateSaver;

namespace Interface
{
	public interface IConfigWithDialogStates
	{
		IDictionary<string, DialogState> States { get; set; }
	}
}
