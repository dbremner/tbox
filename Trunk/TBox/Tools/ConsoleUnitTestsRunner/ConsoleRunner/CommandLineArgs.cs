using System;
using System.Linq;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
	class CommandLineArgs
	{
		public string Path { get; set; }
		public int ProcessCount { get; set; }
		public bool X86 { get; set; }
		public bool Clone { get; set; }
		public int CloneDeep { get; set; }
		public bool Sync { get; set; }
		public bool Report { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }

		public CommandLineArgs()
		{
			ProcessCount = Environment.ProcessorCount;
			X86 = false;
			Clone = false;
			CloneDeep = 1;
			Sync = false;
			Report = false;
		    DirToCloneTests = System.IO.Path.GetTempPath();
		    CommandBeforeTestsRun = string.Empty;
		}

		public void Parse(string[] args)
		{
			Path = args[0];
			foreach (var s in args.Skip(1))
			{
				ParseArgument(s);
			}
            Console.WriteLine("Start with arguments: -p={0}, -x86={1}, -clone={2}, -cloneDeep={3}, -sync={4}, -report={5}, path='{6}', -dirToCloneTests='{7}', -commandBeforeTestsRun='{8}' ",
				ProcessCount, X86, Clone, CloneDeep, Sync, Report, Path, DirToCloneTests, CommandBeforeTestsRun);
		}

		private void ParseArgument(string arg)
		{
			if (Starts(arg, "-p="))
			{
				ProcessCount = int.Parse(arg.Substring(3));
			}
			else if (Equals(arg, "-x86"))
			{
				X86 = true;
			}
			else if (Equals(arg, "-clone"))
			{
				Clone = true;
			}
			else if (Starts(arg, "-cloneDeep="))
			{
				CloneDeep = int.Parse(arg.Substring(11));
			}
            else if (Starts(arg, "-dirToCloneTests="))
            {
                DirToCloneTests = arg.Substring(17);
            }
            else if (Starts(arg, "-commandBeforeTestsRun="))
            {
                CommandBeforeTestsRun = arg.Substring(23);
            }
			else if (Equals(arg, "-sync"))
			{
				Sync = true;
			}
			else if (Equals(arg, "-report"))
			{
				Report = true;
			}
			else
			{
				throw new ArgumentException("Unknown argument: " + arg);
			}
		}

		private static bool Equals(string arg, string str)
		{
			return string.Equals(arg, str, StringComparison.OrdinalIgnoreCase);
		}

		private static bool Starts(string arg, string str)
		{
			return arg.StartsWith(str, StringComparison.OrdinalIgnoreCase);
		}
	}
}
