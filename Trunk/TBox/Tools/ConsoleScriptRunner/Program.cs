using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Base.Log;
using ScriptEngine.Core;
using ScriptEngine.Core.Assemblies;

namespace ConsoleScriptRunner
{
	class Program
	{
		private static ILog log;

		static Program()
		{
			AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
		}

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

                var rootPath = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
				Console.WriteLine("Load libraries");
				var loader = new Loader();
				loader.Load(rootPath);
                var worker = new Worker(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox"), 
                    rootPath, 
                    collector.Collect());
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

		static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
		{
		    return (from dir in new[] {"Libraries", "Localization"} select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
		}
	}
}
