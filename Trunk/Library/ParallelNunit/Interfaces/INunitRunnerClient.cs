using System.ServiceModel;

namespace ParallelNUnit.Interfaces
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
        [OperationContract]
        void SendOutput(string text);
    }
}
