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
		[File( "c:/")]
		public string PathToDevServer { get; set; }

		[IntegerDictionary("pathtoserver", 49163, Min=0, Max=65536)]
		public IDictionary<string,int> DevProjects { get; set; }

		[IntegerList(401,403)]
		public IList<int> AllowedHttpStatusCodes { get; set; }

		[Integer(240)]
		public int TimeOut { get; set; }


		public void Run()
		{
			Parallel.ForEach(DevProjects, Work);
		}

		private void Work(KeyValuePair<string, int> project)
		{
			Console.WriteLine("Start dev server: " + project.Key);
			new CassiniRunner("").Run(project.Key, project.Value, "/", false, PathToDevServer, "", false);
			Console.WriteLine("Open page: " + project.Key);
			var r = new Request().GetResult("http://localhost:" + project.Value, timeOut: TimeOut*1000);
			Console.WriteLine("Page '{0}' time: {1}", project.Key, r.Time);
			if (r.HttpStatusCode == HttpStatusCode.OK || AllowedHttpStatusCodes.Contains((int)r.HttpStatusCode)) return;
			Console.WriteLine("Body: {0}", r.Body);
			throw new ArgumentException("Can't open: " + project.Key);
		}
	}
}
