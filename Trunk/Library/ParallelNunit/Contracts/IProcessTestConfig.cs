namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IProcessTestConfig : ITestsConfig
    {
        string NunitAgentPath { get; }
        string RunAsx86Path { get; }
        bool RunAsx86 { get; }
        bool RunAsAdmin { get; }
    }
}
