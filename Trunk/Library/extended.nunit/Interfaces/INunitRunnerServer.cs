using System.ServiceModel;

namespace extended.nunit.Interfaces
{
	[ServiceContract]
	public interface INunitRunnerServer
	{
		[OperationContract]
		void CanClose();
	}
}
