using System;
using System.IO;
using System.Linq;
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
            return (from dir in new[] { "Libraries", "Localization" } select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
		}
	}
}
