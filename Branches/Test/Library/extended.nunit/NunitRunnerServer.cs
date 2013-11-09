using System.ServiceModel;
using extended.nunit.Interfaces;

namespace extended.nunit
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] 
	class NunitRunnerServer : INunitRunnerServer
	{
		public bool ShouldWait { get; private set; }
		public NunitRunnerServer()
		{
			ShouldWait = true;
		}
		public void CanClose()
		{
			ShouldWait = false;
		}
	} 
}
