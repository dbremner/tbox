using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common.Base;
using Common.Base.Log;
using Common.Tools;

namespace PluginsShared.Tools
{
	public class CassiniRunner
	{
		private static readonly ILog Log = LogManager.GetLogger<CassiniRunner>();
		private readonly string dataRootPath;
		public CassiniRunner(string dataRootPath)
		{
			this.dataRootPath = dataRootPath;
		}

		public void Run( string path, int port, string vpath, bool asAdmin, string pathToDevServer, string pathToBrowser, bool runBrowser)
		{
			if(!ExceptionsHelper.HandleException(
				() => RunServer(path, port, vpath, asAdmin, pathToDevServer),
				() => "Can't run dev server: " + pathToDevServer,
				Log
				))
			{
				if (runBrowser)
				{
					ExceptionsHelper.HandleException(
					() => RunBrowser(port, vpath, pathToBrowser),
					() => "Can't run browser: " + pathToBrowser,
					Log
					);
				}
			}
		}

		private static void RunBrowser(int port, string vpath, string path)
		{
			using (Process.Start(path, "http://localhost:" + port + vpath)){}
		}

		private void RunServer(string path, int port, string vpath, bool asAdmin, string pathToDevServer)
		{
			var args = string.Format("/path:\"{0}\" /port:{1} /vpath:{2}", path, port, vpath);
		    var pi = new ProcessStartInfo
		        {
		            Arguments = args,
		            FileName = pathToDevServer,
		            UseShellExecute = true,
		            CreateNoWindow = true
		        };
		    if (asAdmin) pi.Verb = "runas";
            try
            {
                using (Process.Start(pi)){}
            }
            catch (Exception)
            {
                pi.Verb = null;
                using (Process.Start(pi)) { }
            }
		}

		public void StopAll(string pathToDevServer)
		{
			ProcessProcesses(pathToDevServer, p=>p.Kill());
		}

		private static void ProcessProcesses(string pathToDevServer, Action<Process> worker )
		{
			var name = Path.GetFileNameWithoutExtension(pathToDevServer);
			foreach (var p in Process.GetProcesses().Where(x => x.ProcessName.EqualsIgnoreCase(name)))
			{
				try
				{
					worker(p);
				}
				catch (Exception ex)
				{
					Log.Write(ex, "Can't process: " + p.ProcessName);
					return;
				}
			}
		}
	}
}
