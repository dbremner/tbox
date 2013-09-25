using System;
using System.IO;
using System.Reflection;
using NUnitAgent.Code;

namespace NUnitAgent
{
	class Program
	{
		[STAThread]
		static int Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
            Console.WriteLine("Execute NUnitAgent with arguments: " + string.Join("; ", args));
			var worker = new Worker();
			return worker.Run(args);
		}

		static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
		{
			var assemblyPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\Libraries\\", new AssemblyName(args.Name).Name + ".dll"));
			return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
		}
	}
}
