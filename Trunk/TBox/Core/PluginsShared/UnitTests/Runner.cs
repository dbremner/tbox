using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Common.Base.Log;
using Common.Communications.Interprocess;
using Common.Console;
using extended.nunit.Interfaces;
using PluginsShared.UnitTests.Communication;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;

namespace PluginsShared.UnitTests
{
	class Runner
	{
	    private static readonly ILog Log = LogManager.GetLogger<Runner>();
	    private readonly AgentProcessCreator processCreator;
		private readonly DirectoriesManipulator dirMan = new DirectoriesManipulator();

        public Runner(string nunitAgentPath, string runAsx86Path)
        {
            processCreator = new AgentProcessCreator(nunitAgentPath, runAsx86Path);
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

        public void Run(string path, IList<IList<Result>> packages, InterprocessServer<INunitRunnerClient> server, bool copyToLocalFolders, string[] copyMasks, bool needSynchronizationForTests, bool runAsx86, bool runAsAdmin, string dirToCloneTests, string commandToExecuteBeforeTests, int startDelay, Synchronizer synchronizer, IProgressStatus u)
		{
			var dllPaths = dirMan.GenerateFolders(path, packages, copyToLocalFolders, copyMasks, dirToCloneTests, u);
			var s = (NunitRunnerClient)server.Owner;
			try
			{
                if (u.UserPressClose) return;
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
                if (!string.IsNullOrEmpty(commandToExecuteBeforeTests))
                {
                    foreach (var folder in dllPaths)
                    {
                        Cmd.Start(commandToExecuteBeforeTests, Log, 
                            directory: Path.GetDirectoryName(folder), 
                            waitEnd: true, 
                            nowindow: true);
                    }
                }
                if(u.UserPressClose)return;
				s.Processes.AddRange(packages.Select(
					(items, i) =>
					    {
                            if(i>0 && startDelay>0)Thread.Sleep(startDelay*1000);
					        return processCreator.Create(dllPaths[i], handle, needSynchronizationForTests ? "test" : "fasttest", runAsx86,
					                              runAsAdmin);
					    }));
			}
			finally
			{
				foreach (var p in s.Processes)
				{
					p.WaitForExit();
					p.Dispose();
				}
				dirMan.ClearFolders(dllPaths, copyToLocalFolders, copyMasks);
			}
		}

	}
}
