using System;
using Common.Base.Log;
using ScriptEngine.Core;
using ScriptEngine.Core.Assemblies;

namespace ConsoleScriptRunner
{
	class Program
	{
		private static ILog log;
		[STAThread]
		static int Main(string[] args)
		{
			LogManager.Init(new ConsoleLog());
			log = LogManager.GetLogger<Program>();

			if (args.Length <= 0)
			{
				log.Write("You should specify at least 1 parameter: profile name to run");
				return -1;
			}
			try
			{
				var collector = new AssembliesCollector();

				var rootPath = AppDomain.CurrentDomain.BaseDirectory;
				Console.WriteLine("Load libraries");
				var loader = new Loader();
				loader.Load(rootPath);
				var worker = new Worker(Folders.Application, collector.Collect());
				foreach (var s in args)
				{
					Console.WriteLine("Execute: " + s);
					worker.Run(s);
				}
				return 0;

			}
			catch (CompilerExceptions cex)
			{
				log.Write("Compiler error: " + cex);
				return -1;
			}
			catch (Exception ex)
			{
				log.Write(ex, "Internal error");
				return -1;
			}
		}
	}
}
