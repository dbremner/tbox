using System.ServiceModel;

namespace ParallelNUnit.Interfaces
{
	[ServiceContract]
	public interface INunitRunnerServer
	{
		[OperationContract]
		void CanClose();
	}
}
