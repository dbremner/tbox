using System;
using System.Configuration;
using System.ServiceProcess;
using Common.Base.Log;
using Common.Communications.Network;

namespace FeedbackService
{
	public partial class FeedbackService : ServiceBase
	{
		static void Main(string[] args)
		{
			LogManager.Init(new MultiLog(new IBaseLog[]{new ConsoleLog(), new FileLog("service.log") }));
			var service = new FeedbackService();
			if (Environment.UserInteractive)
			{
				service.OnStart(args);
				Console.WriteLine("Press any key to stop program");
				Console.Read();
				service.OnStop();
			}
			else
			{
				Run(new ServiceBase[] { service });
			}
		}

		private Server<IFeedbackServer> server;

		public FeedbackService()
		{
			InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			server = new Server<IFeedbackServer>(
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
	}
}
