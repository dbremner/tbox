﻿using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Communications;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Tools.FeedbackService
{
    public partial class FeedbackService : ServiceBase
    {
        static void Main(string[] args)
        {
            LogManager.Init(new MultiplyLog(new IBaseLog[] { new ConsoleLog(), new FileLog(Path.Combine(Folders.UserLogsFolder, "TBox.Feedback.Service.log")) }));
            var service = new FeedbackService();
            if (Environment.UserInteractive)
            {
                service.OnStart(args);
                Console.WriteLine("Press any key to stop program");
                Console.ReadKey();
                service.OnStop();
            }
            else
            {
                Run(new ServiceBase[] { service });
            }
        }

        private NetworkServer<IFeedbackServer> server;

        public FeedbackService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            server = new NetworkServer<IFeedbackServer>(
                new FeedbackServer(
                    ConfigurationManager.AppSettings["smtpServer"],
                    int.Parse(ConfigurationManager.AppSettings["smtpServerPort"]),
                    ConfigurationManager.AppSettings["Login"],
                    ConfigurationManager.AppSettings["Password"],
                    ConfigurationManager.AppSettings["ToAddress"]
                    ), int.Parse(ConfigurationManager.AppSettings["FeedbackPort"]));
        }

        protected override void OnStop()
        {
            if (server == null) return;
            server.Dispose();
            server = null;
        }

        static FeedbackService()
        {
            AppDomain.CurrentDomain.AssemblyResolve += LoadFromSameFolder;
        }

        public static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            return (from dir in new[] { "Libraries", "Localization" } select Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\" + dir + "\\", new AssemblyName(args.Name).Name + ".dll")) into assemblyPath where File.Exists(assemblyPath) select Assembly.LoadFrom(assemblyPath)).FirstOrDefault();
        }
    }
}
