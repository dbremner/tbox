namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface ISynchronizer
    {
        int Count { get; }
        int Finished { get; }
        void ProcessNextAgent(string handle);
    }
}