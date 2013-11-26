using System.ServiceModel;

namespace extended.nunit.Interfaces
{
	[ServiceContract]
	public interface INunitRunnerClient
	{
		[OperationContract]
		void SetCollectedTests(string results);

		[OperationContract]
		string GiveMeTestIds();
		[OperationContract]
		void SendTestsResults(string items);
		[OperationContract]
		void CanFinish(string handle);
	}
}
