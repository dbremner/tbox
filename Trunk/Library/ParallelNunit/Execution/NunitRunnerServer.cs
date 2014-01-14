using System.ServiceModel;
using Mnk.Library.ParallelNUnit.Interfaces;

namespace Mnk.Library.ParallelNUnit.Execution
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
