namespace Mnk.Library.ParallelNUnit.Contracts
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
        bool OptimizeOrder { get; }
        int ProcessCount { get; }
        string[] Categories { get; }
        bool? IncludeCategories { get; }
        string Type { get; }

        string NunitAgentPath { get; }
        string RunAsx86Path { get; }
        bool RunAsx86 { get; }
        bool RunAsAdmin { get; }

    }
}
