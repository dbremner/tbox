using System;
using System.IO;
using Common.UI.Model;

namespace NUnitRunner.Code.Settings
{
	[Serializable]
	public class TestConfig : CheckableData
	{
		public bool OnlyFailed { get; set; }
		public bool Multithreaded { get; set; }
		public bool CopyToSeparateFolders { get; set; }
		public int ProcessCount { get; set; }
		public int CopyDeep { get; set; }
        public bool RunAsx86 { get; set; }
        public bool RunAsAdmin { get; set; }
		public bool NeedSynchronizationForTests { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public bool UseCategories { get; set; }
        public bool IncludeCategories { get; set; }

		public TestConfig()
		{
			OnlyFailed = false;
			CopyToSeparateFolders = false;
			ProcessCount = 1;
			CopyDeep = 1;
            RunAsx86 = true;
			RunAsAdmin = false;
			NeedSynchronizationForTests = false;
		    DirToCloneTests = Path.GetTempPath();
		    CommandBeforeTestsRun = string.Empty;
		    UseCategories = false;
		    IncludeCategories = true;
		}

		public override object Clone()
		{
			return new TestConfig
				{
					IsChecked = IsChecked,
					Key = Key,
					OnlyFailed = OnlyFailed,
					CopyToSeparateFolders = CopyToSeparateFolders,
					ProcessCount = ProcessCount,
					CopyDeep = CopyDeep,
                    RunAsx86 = RunAsx86,
                    RunAsAdmin = RunAsAdmin,
					NeedSynchronizationForTests = NeedSynchronizationForTests,
                    Multithreaded = Multithreaded,
                    DirToCloneTests = DirToCloneTests,
                    CommandBeforeTestsRun = CommandBeforeTestsRun,
                    UseCategories = UseCategories,
                    IncludeCategories = IncludeCategories,
				};
		}
	}
}
