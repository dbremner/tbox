﻿using System;
using System.IO;
using Mnk.Library.Common.Tools;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;

namespace Mnk.TBox.Plugins.NUnitRunner.Code.Settings
{
    [Serializable]
    public class TestConfig : CheckableData
    {
        public bool Multithreaded { get; set; }
        public bool CopyToSeparateFolders { get; set; }
        public int ProcessCount { get; set; }
        public CheckableDataCollection<CheckableData> CopyMasks { get; set; }
        public bool RunAsx86 { get; set; }
        public bool RunAsAdmin { get; set; }
        public bool NeedSynchronizationForTests { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public bool UseCategories { get; set; }
        public bool IncludeCategories { get; set; }
        public bool UsePrefetch { get; set; }
        public int StartDelay { get; set; }
        public string RuntimeFramework { get; set; }

        public TestConfig()
        {
            CopyToSeparateFolders = false;
            ProcessCount = 1;
            CopyMasks = new CheckableDataCollection<CheckableData>
            {
                new CheckableData { Key = "*.dll" }, 
                new CheckableData { Key = "*.config"}
            };
            RunAsx86 = true;
            RunAsAdmin = false;
            NeedSynchronizationForTests = false;
            DirToCloneTests = Path.GetTempPath();
            CommandBeforeTestsRun = string.Empty;
            UseCategories = false;
            IncludeCategories = true;
            StartDelay = 0;
            UsePrefetch = false;
            RuntimeFramework = "net-4.0";
        }

        public override object Clone()
        {
            return new TestConfig
            {
                IsChecked = IsChecked,
                Key = Key,
                CopyToSeparateFolders = CopyToSeparateFolders,
                ProcessCount = ProcessCount,
                CopyMasks = CopyMasks.Clone(),
                RunAsx86 = RunAsx86,
                RunAsAdmin = RunAsAdmin,
                NeedSynchronizationForTests = NeedSynchronizationForTests,
                Multithreaded = Multithreaded,
                DirToCloneTests = DirToCloneTests,
                CommandBeforeTestsRun = CommandBeforeTestsRun,
                UseCategories = UseCategories,
                IncludeCategories = IncludeCategories,
                StartDelay = StartDelay,
                UsePrefetch = UsePrefetch,
                RuntimeFramework = RuntimeFramework
            };
        }
    }
}
