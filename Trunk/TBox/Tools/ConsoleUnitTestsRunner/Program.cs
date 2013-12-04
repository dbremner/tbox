using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common.Base.Log;
using ConsoleUnitTestsRunner.ConsoleRunner;
using ParallelNUnit.Core;

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

            if (args.Length <= 0 || string.Equals(args[0], "/?") || string.Equals(args[0], "/help", StringComparison.OrdinalIgnoreCase))
            {
                ShowHelp();
                return -1;
            }
            int ret = -1;
            var wait = false;
            try
            {
                var cmdArgs = new CommandLineArgs();
                cmdArgs.Parse(args);
                wait = cmdArgs.Wait;
                if (cmdArgs.Logo)
                {
                    ShowLogo();
                    ShowArgs(cmdArgs);
                }
                Console.WriteLine("ProcessModel: Default\tDomainUseage: Single");
                Console.WriteLine("Execution Runtime: Default\tCPUCount: " + Environment.ProcessorCount);
                if (!File.Exists(args[0]))
                {
                    log.Write("Can't find tests in library: " + args[0]);
                    return -2;
                }
                var testsRunner = new TestsRunner();
                ret = testsRunner.Run(cmdArgs);
            }
            catch (Exception ex)
            {
                log.Write(ex, "Internal error");
                ShowHelp();
            }
            finally
            {
                NUnitBase.Dispose();
            }
            if (wait) Console.ReadKey();
            return ret;
        }

        private static void ShowArgs(CommandLineArgs cmd)
        {
            Console.WriteLine("Start with arguments: {0} -p={1}{2}{3}{4}{5}{6} -dirToCloneTests='{7}' -commandBeforeTestsRun='{8}'{9}{10} -startDelay={11}{12}{13}{14}{15}{16}",
                cmd.Path,
                cmd.ProcessCount, AddIfTrue(" /wait ", cmd.Wait),
                AddIfTrue(" -clone ", cmd.Clone), Format(" -copyMasks=", cmd.CopyMasks),
                AddIfTrue(" -sync ", cmd.Sync), Format(" /xml=",cmd.XmlReport),
                cmd.DirToCloneTests, cmd.CommandBeforeTestsRun,
                Format(" /include=", cmd.Include), Format(" /exclude=", cmd.Exclude),
                cmd.StartDelay, 
                AddIfTrue("/nologo", !cmd.Logo), AddIfTrue("/labels", cmd.Labels),
                Format(" /output:", cmd.OutputReport), AddIfTrue(" -prefetch ", cmd.Prefetch), AddIfTrue(" -teamcity ", cmd.Teamcity));
        }

        private static string AddIfTrue(string str, bool value)
        {
            return value ? str : string.Empty;
        }

        private static string Format(string prefix, params string[] items)
        {
            if (items == null) return string.Empty;
            return prefix + string.Join(";", items);
        }

        private static void ShowLogo()
        {
            Console.WriteLine("TBox.ConsoleUnitTestsRunner version " + Assembly.GetCallingAssembly().GetName().Version);
            Console.WriteLine("Source: http://tbox.codeplex.com");
            Console.WriteLine("Copyright (C) 2010-2013 Aliaksandr Hrynko (Mnk).");
            Console.WriteLine("All Rights Reserved");
            Console.WriteLine();
            Console.WriteLine("Runtime Environment - ");
            Console.WriteLine("   OS Version: " + Environment.OSVersion);
            Console.WriteLine("  CLR Version: " + Environment.Version);
            Console.WriteLine();
        }

        private static void ShowHelp()
        {
            Console.WriteLine("You should specify at least 1 parameter: path to unit tests");
            Console.WriteLine("Other parameters:");
            Console.WriteLine("-p=N             - specify process count, by default N = cpu cores count");
            Console.WriteLine("-clone           - clone unit tests folder, by default false");
            Console.WriteLine("-copyMasks=a[;b] - file masks to copy, by default =*.dll;*.config");
            Console.WriteLine("-sync            - enable sync for unit tests, by default false");
            Console.WriteLine("-dirToCloneTests=path - folder to clone tests, by default %temp%");
            Console.WriteLine("-commandBeforeTestsRun=exePath - command to run before execute tests, but after it clone , by default empty");
            Console.WriteLine("-startDelay=N    - delay before test starts, by default 0");
            Console.WriteLine("-prefetch        - use test run time statistic to optimize tests separation, by default false");
            Console.WriteLine("-teamcity        - teamcity inteagration: will show test progress, by default false");
            Console.WriteLine("NUnit compatible arguments:");
            Console.WriteLine("/include=a1[;a2] - include only tests with specified categories");
            Console.WriteLine("/exclude=a1[;a2] - exclude all tests with specified categories");
            Console.WriteLine("/xml=[path]        path for xml report, by default empty");
            Console.WriteLine("/output:[path]     path for tests output, by default empty");
            Console.WriteLine("/labels            Label each test in stdOut, by default false");
            Console.WriteLine("/noshadow          Do nothing (for now)");
            Console.WriteLine("/nologo            Do not display the logo, by default false");
            Console.WriteLine("/help              Display this page");
            Console.WriteLine("/wait              wait for input key after at the end, by default false");
        }

        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
        }

        public static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            return (from dir in new[] { "Libraries", "Localization" } select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
        }
    }
}
