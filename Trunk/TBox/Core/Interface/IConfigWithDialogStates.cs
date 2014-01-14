using System.Collections.Generic;
using Mnk.Library.WPFControls.Dialogs.StateSaver;

namespace Mnk.TBox.Core.Interface
{
	public interface IConfigWithDialogStates
	{
		IDictionary<string, DialogState> States { get; set; }
	}
}
