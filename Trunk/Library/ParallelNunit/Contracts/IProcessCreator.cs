namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IProcessCreator
    {
        IRunnerContext Create(IProcessTestConfig config, string handle, string command);
    }

}
