using System.Collections.Generic;
using System.Linq;
using Common.Communications;
using ConsoleUnitTestsRunner.Code.Communication;
using ConsoleUnitTestsRunner.Code.Interfaces;
using ConsoleUnitTestsRunner.Code.Settings;
using extended.nunit.Interfaces;

namespace ConsoleUnitTestsRunner.Code
{
	class Runner
	{
		private readonly AgentProcessCreator processCreator;
		private readonly DirectoriesManipulator dirMan = new DirectoriesManipulator();

		public Runner(string nunitAgentPath)
		{
			processCreator = new AgentProcessCreator(nunitAgentPath);
		}

		public IEnumerable<IList<Result>> DivideTestsToRun(IList<Result> files, int threadCount)
		{
			var result = new List<IList<Result>>();
			for (var j = 0; j < threadCount; ++j)
			{
				result.Add(new List<Result>(files.Count/threadCount));
			}
			for (var i = 0; i < files.Count;)
			{
				for (var j = 0; j < threadCount && i < files.Count; ++j)
				{
					result[j].Add(files[i++]);
				}
			}
			return result;
		}

		public void Run(string path, IList<IList<Result>> packages, Server<INunitRunnerClient> server, bool copyToLocalFolders, int copyDeep, bool needSynchronizationForTests, bool runAsx86, bool runAsAdmin, Synchronizer synchronizer, IProgressStatus u)
		{
			var dllPathes = dirMan.GenerateFolders(path, packages, copyToLocalFolders, copyDeep, u);
			var s = (NunitRunnerClient)server.Owner;
			try
			{
				var handle = server.Handle;
				s.PrepareToRun(synchronizer, u);
				foreach (var items in packages)
				{
					foreach (var item in items)
					{
						s.AllTestsResults[item.Id] = item;
					}
					s.TestsToRun.Add(items.Select(x => x.Id));
				}
				s.Processes.AddRange(packages
					.AsParallel()
					.Select((items, i) => processCreator.Create(dllPathes[i], handle, needSynchronizationForTests ? "test" : "fasttest", runAsx86, runAsAdmin)));
			}
			finally
			{
				foreach (var p in s.Processes)
				{
					p.WaitForExit();
					p.Dispose();
				}
				dirMan.ClearFolders(dllPathes, copyToLocalFolders, copyDeep);
			}
		}

	}
}
