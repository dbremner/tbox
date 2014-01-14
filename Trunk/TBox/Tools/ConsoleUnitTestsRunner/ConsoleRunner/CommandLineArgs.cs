using System;
using System.Linq;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    class CommandLineArgs
    {
        public string Path { get; set; }
        public int ProcessCount { get; set; }
        public bool Prefetch { get; set; }
        public bool Clone { get; set; }
        public string[] CopyMasks { get; set; }
        public bool Sync { get; set; }
        public bool Teamcity { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public int StartDelay { get; set; }

        //NUnit
        public string[] Include { get; set; }
        public string[] Exclude { get; set; }
        public bool Logo { get; set; }
        public bool Labels { get; set; }
        public string XmlReport { get; set; }
        public string OutputReport { get; set; }
        public bool Wait { get; set; }

        public CommandLineArgs()
        {
            ProcessCount = Environment.ProcessorCount;
            Prefetch = false;
            Clone = false;
            CopyMasks = new[] { "*.dll", "*.config" };
            Sync = false;
            XmlReport = string.Empty;
            DirToCloneTests = System.IO.Path.GetTempPath();
            CommandBeforeTestsRun = string.Empty;
            StartDelay = 0;
            Logo = true;
            Labels = false;
            Wait = false;
            Teamcity = false;
        }

        public void Parse(string[] args)
        {
            Path = args[0];
            foreach (var s in args.Skip(1))
            {
                ParseArgument(s);
            }
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
            else if (Equals(arg, "-sync"))
            {
                Sync = true;
            }
            else if (Equals(arg, "-prefetch"))
            {
                Prefetch = true;
            }
            else if (Equals(arg, "-teamcity"))
            {
                Teamcity = true;
            }
            //NUnit arguments
            else if (Starts(arg, "/include="))
            {
                if (Exclude != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Include = arg.Substring(9).Split(';');
            }
            else if (Starts(arg, "/exclude="))
            {
                if (Include != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Exclude = arg.Substring(9).Split(';');
            }
            else if (Starts(arg, "/output:"))
            {
                OutputReport = arg.Substring(8);
            }
            else if (Starts(arg, "/xml="))
            {
                XmlReport = arg.Substring(5);
            }
            else if (Equals(arg, "/nologo"))
            {
                Logo = false;
            }
            else if (Equals(arg, "/labels"))
            {
                Labels = true;
            }
            else if (Equals(arg, "/noshadow"))
            {
            }
            else if (Equals(arg, "/wait"))
            {
                Wait = true;
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
