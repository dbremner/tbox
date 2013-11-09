using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Base.Log;
using ConsoleUnitTestsRunner.ConsoleRunner;

namespace ConsoleUnitTestsRunner
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
			    ShowHelp();
			    return -1;
			}
		    try
			{
				var cmdArgs = new CommandLineArgs();
				cmdArgs.Parse(args);
				var testsRunner = new TestsRunner();
				return testsRunner.Run(cmdArgs.Path, cmdArgs.ProcessCount, cmdArgs.X86, cmdArgs.Clone, cmdArgs.CloneDeep, cmdArgs.Sync, cmdArgs.Report, cmdArgs.DirToCloneTests, cmdArgs.CommandBeforeTestsRun);
			}
			catch (Exception ex)
			{
				log.Write(ex, "Internal error");
                ShowHelp();
				return -1;
			}
		}

	    private static void ShowHelp()
	    {
	        log.Write("You should specify at least 1 parameter: path to unit tests");
	        Console.WriteLine("Other parameters:");
	        Console.WriteLine("-p=N         - specify process count, by default N = cpu cores count");
	        Console.WriteLine("-x86         - force x86 mode, by default false");
	        Console.WriteLine("-clone       - clone unit tests folder, by default false");
	        Console.WriteLine("-cloneDeep=N - clone deep for unit tests folder, by default deep = 1");
	        Console.WriteLine("-sync        - enable sync for unit tests, by default false");
	        Console.WriteLine("-report      - save xml report, by default false");
	        Console.WriteLine("-report      - save xml report, by default false");
	        Console.WriteLine("-dirToCloneTests=path - folder to clone tests, by default %temp%");
	        Console.WriteLine("-commandBeforeTestsRun=exePath - command to run before execute tests, but after it clone , by default empty");
	    }

		static Program()
		{
			AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
		}

		static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
		{
            return (from dir in new[] { "Libraries", "Localization" } select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
		}
	}
}
