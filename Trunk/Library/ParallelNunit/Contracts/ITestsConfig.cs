﻿namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ITestsConfig
    {
        string TestDllPath { get; }
        string DirToCloneTests { get; }
        string CommandBeforeTestsRun { get; }
        string RuntimeFramework { get; }
        bool CopyToSeparateFolders { get; }
        string[] CopyMasks { get; }
        bool NeedSynchronizationForTests { get; }
        int StartDelay { get; }
        bool NeedOutput { get; }
        bool UsePrefetch { get; }
        int ProcessCount { get; }
        string[] Categories { get; }
        bool? IncludeCategories { get; }
    }
}
