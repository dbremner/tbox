using System.Collections.Generic;
using Common.UI.ModelsContainers;
using Interface;
using ScriptEngine;
using WPFControls.Dialogs.StateSaver;

namespace SkyNet.Code.Settings
{
    public class Config: IConfigWithDialogStates
	{
        public IDictionary<string, DialogState> States { get; set; }
        public CheckableDataCollection<Operation> Operations { get; set; }

        public Config()
        {
            States = new Dictionary<string, DialogState>();
            Operations = new CheckableDataCollection<Operation>();
        }
    }
}
