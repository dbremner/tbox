using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mnk.Library.Common.Log;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code;
using Mnk.TBox.Tools.ConsoleUnitTestsRunner.Code.Contracts;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            Init();
            using (var container = ServicesRegistrar.Register())
            {
                return container.GetInstance<IExecutor>().Execute(args);
            }
        }

        private static void Init()
        {
            var logsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TBox",
                "Logs");
            if (!Directory.Exists(logsFolder)) Directory.CreateDirectory(logsFolder);
            LogManager.Init(new MultiplyLog(new ConsoleLog(),
                new FileLog(Path.Combine(logsFolder, "ConsoleUnitTestsRunner.log"))));
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
