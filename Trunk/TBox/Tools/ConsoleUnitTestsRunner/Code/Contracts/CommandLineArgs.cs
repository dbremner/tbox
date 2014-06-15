using System;
using System.Collections.Generic;
using System.Globalization;
using Mnk.Library.ParallelNUnit.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts
{
    public class CommandLineArgs 
    {
        public IList<string> Paths { get; set; }
        public int TestsInParallel { get; set; }
        public int AssembliesInParallel { get; set; }
        public bool Prefetch { get; set; }
        public bool Clone { get; set; }
        public string[] CopyMasks { get; set; }
        public bool Sync { get; set; }
        public bool Teamcity { get; set; }
        public string DirToCloneTests { get; set; }
        public string CommandBeforeTestsRun { get; set; }
        public int StartDelay { get; set; }
        public string Mode { get; set; }

        //NUnit
        public string[] Include { get; set; }
        public string[] Exclude { get; set; }
        public bool Logo { get; set; }
        public bool Labels { get; set; }
        public string XmlReport { get; set; }
        public string OutputReport { get; set; }
        public bool Wait { get; set; }
        public string RuntimeFramework { get; set; }

        public CommandLineArgs()
        {
            TestsInParallel = Environment.ProcessorCount;
            AssembliesInParallel = 1;
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
            Paths = new List<string>();
            Mode = TestsRunnerType.Internal;
        }

        public CommandLineArgs(IEnumerable<string> args) : this()
        {
            foreach (var s in args)
            {
                ParseArgument(s);
            }
        }

        private void ParseArgument(string arg)
        {
            if (Starts(arg, CommandLineConstants.TestsInParallel))
            {
                TestsInParallel = int.Parse(arg.Substring(CommandLineConstants.TestsInParallel.Length), CultureInfo.InvariantCulture);
                if (TestsInParallel <= 0) TestsInParallel = Environment.ProcessorCount;
            }
            else if (Starts(arg, CommandLineConstants.AssembliesInParallel))
            {
                AssembliesInParallel = int.Parse(arg.Substring(CommandLineConstants.AssembliesInParallel.Length), CultureInfo.InvariantCulture);
                if (AssembliesInParallel <= 0) AssembliesInParallel = Environment.ProcessorCount;
            }
            else if (Starts(arg, CommandLineConstants.StartDelay))
            {
                StartDelay = int.Parse(arg.Substring(CommandLineConstants.StartDelay.Length), CultureInfo.InvariantCulture);
            }
            else if (Equals(arg, CommandLineConstants.Clone))
            {
                Clone = true;
            }
            else if (Starts(arg, CommandLineConstants.CopyMasks))
            {
                CopyMasks = arg.Substring(CommandLineConstants.CopyMasks.Length).Split(';');
            }
            else if (Starts(arg, CommandLineConstants.DirToCloneTests))
            {
                DirToCloneTests = arg.Substring(CommandLineConstants.DirToCloneTests.Length);
            }
            else if (Starts(arg, CommandLineConstants.CommandBeforeTestsRun))
            {
                CommandBeforeTestsRun = arg.Substring(CommandLineConstants.CommandBeforeTestsRun.Length);
            }
            else if (Equals(arg, CommandLineConstants.Sync))
            {
                Sync = true;
            }
            else if (Equals(arg, CommandLineConstants.Prefetch))
            {
                Prefetch = true;
            }
            else if (Equals(arg, CommandLineConstants.Teamcity))
            {
                Teamcity = true;
            }
            else if (Starts(arg, CommandLineConstants.Mode))
            {
                Mode = arg.Substring(CommandLineConstants.Mode.Length);
            }
            //NUnit arguments
            else if (Starts(arg, CommandLineConstants.Include))
            {
                if (Exclude != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Include = arg.Substring(CommandLineConstants.Include.Length).Split(';');
            }
            else if (Starts(arg, CommandLineConstants.Exclude))
            {
                if (Include != null) throw new ArgumentException("You can't specify include and exclude at the same time.");
                Exclude = arg.Substring(CommandLineConstants.Exclude.Length).Split(';');
            }
            else if (Starts(arg, CommandLineConstants.Output))
            {
                OutputReport = arg.Substring(CommandLineConstants.Output.Length);
            }
            else if (Starts(arg, CommandLineConstants.Xml))
            {
                XmlReport = arg.Substring(CommandLineConstants.Xml.Length);
            }
            else if (Starts(arg, CommandLineConstants.Framework))
            {
                RuntimeFramework = arg.Substring(CommandLineConstants.Framework.Length);
            }
            else if (Equals(arg, CommandLineConstants.Nologo))
            {
                Logo = false;
            }
            else if (Equals(arg, CommandLineConstants.Labels))
            {
                Labels = true;
            }
            else if (Equals(arg, CommandLineConstants.Noshadow))
            {
            }
            else if (Equals(arg, CommandLineConstants.Wait))
            {
                Wait = true;
            }
            else
            {
                Paths.Add(arg);
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
