using System.ServiceModel;

namespace Mnk.Library.ParallelNUnit.Interfaces
{
    [ServiceContract]
    public interface INunitRunnerClient
    {
        [OperationContract]
        void SetCollectedTests(string items);
        [OperationContract]
        string GiveMeConfig();
        [OperationContract]
        bool SendTestsResults(string items);
        [OperationContract]
        void CanFinish(string handle);
    }
}
