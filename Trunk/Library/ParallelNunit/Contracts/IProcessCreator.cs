namespace Mnk.Library.ParallelNUnit.Contracts
{
    public interface IProcessCreator
    {
        IRunnerContext Create(string handle, string command);
    }

}
