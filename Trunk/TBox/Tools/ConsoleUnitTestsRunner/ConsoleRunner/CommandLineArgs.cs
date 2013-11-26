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
        public string[] CopyMasks { get; set; }
        public bool Sync { get; set; }
        public bool Report { get; set; }
        public string[] Include { get; set; }
        public string[] Exclude { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public int StartDelay { get; set; }

        public CommandLineArgs()
        {
            ProcessCount = Environment.ProcessorCount;
            X86 = false;
            Clone = false;
            CopyMasks = new[] { "*.dll", "*.config" };
            Sync = false;
            Report = false;
            DirToCloneTests = System.IO.Path.GetTempPath();
            CommandBeforeTestsRun = string.Empty;
            StartDelay = 0;
        }

        public void Parse(string[] args)
        {
            Path = args[0];
            foreach (var s in args.Skip(1))
            {
                ParseArgument(s);
            }
            Console.WriteLine("Start with arguments: {0} -p={1}{2}{3}{4}{5}{6} -dirToCloneTests='{7}' -commandBeforeTestsRun='{8}'{9}{10}, -startDelay={11}",
                Path,
                ProcessCount, AddIfTrue(" -x86 ", X86), 
                AddIfTrue(" -clone ", Clone), Format(" -copyMasks=", CopyMasks), 
                AddIfTrue(" -sync ", Sync), AddIfTrue(" -report ", Report), 
                DirToCloneTests, CommandBeforeTestsRun, 
                Format(" -include=", Include), Format(" -exclude=", Exclude),
                StartDelay);
        }

        private static string AddIfTrue(string str, bool value)
        {
            return value ? str : string.Empty;
        }

        private static string Format(string prefix, string[] items)
        {
            if (items == null) return string.Empty;
            return prefix + string.Join(";", items);
        }

        private void ParseArgument(string arg)
        {
            if (Starts(arg, "-p="))
            {
                ProcessCount = int.Parse(arg.Substring(3));
            }
            else if (Starts(arg, "-startDelay="))
            {
                StartDelay = int.Parse(arg.Substring(12));
            }
            else if (Equals(arg, "-x86"))
            {
                X86 = true;
            }
            else if (Equals(arg, "-clone"))
            {
                Clone = true;
            }
            else if (Starts(arg, "-copyMasks="))
            {
                CopyMasks = arg.Substring(11).Split(';');
            }
            else if (Starts(arg, "-dirToCloneTests="))
            {
                DirToCloneTests = arg.Substring(17);
            }
            else if (Starts(arg, "-commandBeforeTestsRun="))
            {
                CommandBeforeTestsRun = arg.Substring(23);
            }
            else if (Starts(arg, "-include="))
            {
                if (Exclude != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Include = arg.Substring(9).Split(';');
            }
            else if (Starts(arg, "-exclude="))
            {
                if (Include != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Exclude = arg.Substring(9).Split(';');
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
