namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IThreadTestsExecutor
    {
        int RunTests(IThreadTestConfig config, string handle);
    }
}
