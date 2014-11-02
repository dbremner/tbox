using System;
using System.Collections.Generic;
using Mnk.Library.WpfControls.Dialogs.StateSaver;
using Mnk.TBox.Core.Contracts;

namespace Mnk.TBox.Plugins.XsltTester
{
    [Serializable]
    public class Config : IConfigWithDialogStates
    {
        public bool TestManual { get; set; }
        public bool AutoFormatResult { get; set; }
        public string Xslt { get; set; }
        public string Xml { get; set; }
        public IDictionary<string, DialogState> States { get; set; }

        public Config()
        {
            TestManual = false;
            AutoFormatResult = true;
            States = new Dictionary<string, DialogState>();
        }
    }
}
