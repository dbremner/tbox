using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit
{
    public class ProcessTestConfig : IProcessTestConfig
    {
        public string TestDllPath { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public string RuntimeFramework { get; set; }
        public bool CopyToSeparateFolders { get; set; }
        public string[] CopyMasks { get; set; }
        public bool NeedSynchronizationForTests { get; set; }
        public int StartDelay { get; set; }
        public bool NeedOutput { get; set; }
        public bool UsePrefetch { get; set; }
        public int ProcessCount { get; set; }
        public string[] Categories { get; set; }
        public bool? IncludeCategories { get; set; }
        public string NunitAgentPath { get; set; }
        public string RunAsx86Path { get; set; }
        public bool RunAsx86 { get; set; }
        public bool RunAsAdmin { get; set; }
    }
}
