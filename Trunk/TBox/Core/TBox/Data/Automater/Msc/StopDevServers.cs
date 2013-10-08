using PluginsShared.Tools;
using ScriptEngine;

namespace Solution.Msc
{
	public class StopDevServers : IScript
	{
		[File("c:/")]
		public string PathToDevServer { get; set; }

		public void Run()
		{
			new CassiniRunner("").StopAll(PathToDevServer);
		}
	}
}
