using PluginsShared.Automator;
using PluginsShared.Tools;
using ScriptEngine;

namespace Solution.Scripts
{
	public class StopDevServers : IScript
	{
		[File("c:/")]
		public string PathToDevServer { get; set; }

		public void Run(IScriptContext context)
		{
			new CassiniRunner("").StopAll(PathToDevServer);
		}
	}
}
