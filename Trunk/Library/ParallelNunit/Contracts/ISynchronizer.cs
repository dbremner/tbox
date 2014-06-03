namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ISynchronizer
    {
        int Finished { get; }
        void ProcessNextAgent(ITestsConfig config, string handle);
    }
}