using System;
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
				log.Write("You should specify at least 1 parameter: path to unit tests");
				Console.WriteLine("Other parameters:");
				Console.WriteLine("-p=N         - specify process count, by default N = cpu cores count");
				Console.WriteLine("-x86         - force x86 mode, by default false");
				Console.WriteLine("-clone       - clone unit tests folder, by default false");
				Console.WriteLine("-cloneDeep=N - clone deep for unit tests folder, by default deep = 1");
				Console.WriteLine("-sync        - enable sync for unit tests, by default false");
				Console.WriteLine("-report      - save xml report, by default false");
				return -1;
			}
			try
			{
				var cmdArgs = new CommandLineArgs();
				cmdArgs.Parse(args);
				var testsRunner = new TestsRunner();
				return testsRunner.Run(cmdArgs.Path, cmdArgs.ProcessCount, cmdArgs.X86, cmdArgs.Clone, cmdArgs.CloneDeep, cmdArgs.Sync, cmdArgs.Report);
			}
			catch (Exception ex)
			{
				log.Write(ex, "Internal error");
				return -1;
			}
		}
	}
}
