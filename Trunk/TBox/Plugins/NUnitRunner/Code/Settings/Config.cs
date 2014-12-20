using System;
using System.Collections.Generic;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.TBox.Core.Contracts;

namespace Mnk.TBox.Plugins.NUnitRunner.Code.Settings
{
    [Serializable]
    public class Config : IConfigWithDialogStates
    {
        public CheckableDataCollection<TestSuiteConfig> TestSuites { get; set; }
        public IDictionary<string, DialogState> States { get; set; }

        public Config()
        {
            TestSuites = new CheckableDataCollection<TestSuiteConfig>
            {
                new TestSuiteConfig
                {
                    Key = "Default",
                    FilePathes = new CheckableDataCollection<CheckableData>
                    {
                        new CheckableData
                        {
                            Key = "c:\\projects\\sample.lib.tests.dll"
                        }
                    }
                }
            };
            States = new Dictionary<string, DialogState>();
        }
    }
}
