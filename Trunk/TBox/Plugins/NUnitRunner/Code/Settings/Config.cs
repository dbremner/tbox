using System;
using System.Collections.Generic;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.TBox.Core.Contracts;
using Mnk.Library.WpfControls.Dialogs.StateSaver;

namespace Mnk.TBox.Plugins.NUnitRunner.Code.Settings
{
    [Serializable]
    public class Config : IConfigWithDialogStates
    {
        public CheckableDataCollection<TestConfig> DllPaths { get; set; }
        public IDictionary<string, DialogState> States { get; set; }

        public Config()
		{
			(DllPaths = new CheckableDataCollection<TestConfig>()).
				FillCollection("c:\\projects\\sample.lib.tests.dll");
			States = new Dictionary<string, DialogState>();
		}

	}
}
