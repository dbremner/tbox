using System;
using System.Linq;
using System.Windows.Forms;
using Common.Communications;
using Common.Communications.Interprocess;
using extended.nunit.Interfaces;

namespace NUnitAgent.Code
{
	class Worker
	{
		public int Run(string[] args)
		{
			if (args.Length != 3)
			{
				Log("You should specify 3 parameters: mainwindow handle, path to test dll, method [collect, test]");
				return -1;
			}
			var handle = args[0];
			var path = args[1];
			switch (args[2])
			{
				case "collect":
					return CollectTests(handle, path);
				case "fasttest":
					return RunTests(handle, path, true);
				case "test":
					return RunTests(handle, path, false);
				default:
					Log("Unknown command: " + args[2]);
					return -1;
			}
		}

		private static void Log(string message)
		{
			MessageBox.Show(message, "Unexpected exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private static int CollectTests(string handle, string path)
		{
			return ProcessMessage(handle, r=>r.CollectTests(path));
		}

		private static int RunTests(string handle, string path, bool fast)
		{
		    var results = new InterprocessClient<INunitRunnerClient>(handle).Instance.GiveMeTestIds();
            var ids = string.IsNullOrWhiteSpace(results) ? 
                new int[0] : results.Split(',').Select(int.Parse).ToArray();
			return ProcessMessage(handle, r => r.Run(path, ids, fast));
		}

		private static int ProcessMessage(string handle, Func<Runner, int> action)
		{
			try
			{
				using (var r = new Runner(handle))
				{
					return action(r);
				}
			}
			catch (Exception ex)
			{
				Log(ex.ToString());
			}
			return -1;
		}

	}
}
