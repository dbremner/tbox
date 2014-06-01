using System.ServiceModel;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.Library.ParallelNUnit.Core
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
	class NUnitRunnerServer : INunitRunnerServer
	{
		public bool ShouldWait { get; private set; }
		public NUnitRunnerServer()
		{
			ShouldWait = true;
		}
		public void CanClose()
		{
			ShouldWait = false;
		}
	} 
}
