using PluginsShared.Tools;
using ScriptEngine;

namespace Solution.Scripts
{
	public class StopDevServers : IScript
	{
		[File(true, "c:/")]
		public string PathToDevServer { get; set; }

		public void Run()
		{
			new CassiniRunner("").StopAll(PathToDevServer);
		}
	}
}
