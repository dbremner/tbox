using System.Diagnostics;

namespace PluginsShared.UnitTests
{
	class Calculator
	{
		private readonly AgentProcessCreator processCreator;

	    public Calculator(string nunitAgentPath, string runAsx86Path)
		{
			processCreator = new AgentProcessCreator(nunitAgentPath, runAsx86Path);
		}

		public void CollectTests(string path, bool runAsx86, bool runAsAdmin, string handle)
		{
			Process p = null;
			try
			{
				p = processCreator.Create(path, handle, "collect", runAsx86, runAsAdmin);
			}
			finally
			{
				if (p != null)
				{
					p.WaitForExit();
					p.Dispose();
				}
			}
		}
	}
}
