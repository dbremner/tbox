using System.ServiceModel;

namespace Mnk.Library.ParallelNUnit.Interfaces
{
	[ServiceContract]
	public interface INunitRunnerServer
	{
		[OperationContract]
		void CanClose();
	}
}
