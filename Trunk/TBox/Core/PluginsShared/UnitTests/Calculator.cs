using System.Diagnostics;

namespace PluginsShared.UnitTests
{
	class Calculator
	{
		private readonly AgentProcessCreator processCreator;

	    public Calculator(string nunitAgentPath)
		{
			processCreator = new AgentProcessCreator(nunitAgentPath);
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
