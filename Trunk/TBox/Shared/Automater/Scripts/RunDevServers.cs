using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Network;
using PluginsShared.Tools;
using ScriptEngine;

namespace Solution.Scripts
{
	public class RunDevServers : IScript
	{
		[File(true, "c:/")]
		public string PathToDevServer { get; set; }

		[IntegerDictionary(0, 65536, "pathtoserver", 49163)]
		public IDictionary<string,int> DevProjects { get; set; }

		public void Run()
		{
			Parallel.ForEach(DevProjects, (project) => Work(project));
		}

		private void Work(KeyValuePair<string, int> project)
		{
			Console.WriteLine("Start dev server: " + project.Key);
			new CassiniRunner("").Run(project.Key, project.Value, "/", false, PathToDevServer, "", false);
			Console.WriteLine("Open page: " + project.Key);
			var r = new Request().GetResult("http://localhost:" + project.Value, timeOut: 240000);
			Console.WriteLine("Page '{0}' time: {1}", project.Key, r.Time);
			if (r.HttpStatusCode == HttpStatusCode.OK) return;
			Console.WriteLine("Body: {0}", r.Body);
			throw new ArgumentException("Can't open: " + project.Key);
		}
	}
}
