namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IProcessCalculator
    {
        void CollectTests(IProcessTestConfig config, string path, string handle);
    }
}