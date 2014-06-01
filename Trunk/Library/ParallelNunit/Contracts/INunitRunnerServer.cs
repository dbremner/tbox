using System.ServiceModel;

namespace Mnk.Library.ParallelNUnit.Contracts
{
	[ServiceContract]
	public interface INunitRunnerServer
	{
		[OperationContract]
		void CanClose();
	}
}
