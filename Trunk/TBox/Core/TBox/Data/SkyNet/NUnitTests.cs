using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Tools.SkyNet.Common;
using ServiceStack.Text;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public class NUnitTests : ISkyScript
    {
        private const string NunitAgentPath = "NUnitAgent.exe";
        private const string RunAsx86Path = "RunAsx86.exe";
        [Directory]
        public string DataFolderPath { get; set; }
        [File]
        public string TestDllPath { get; set; }
        [StringList("*.dll", "*.config", CanBeEmpty = false)]
        public string[] PathMasksToInclude { get; set; }
        [String]
        public string CommandBeforeTestsRun { get; set; }
        [Bool(true)]
        public bool RunAsx86 { get; set; }
        [String]
        public string Framework { get; set; }
        [Bool]
        public bool IncludeCategories { get; set; }
        [StringList(CanBeEmpty=true)]
        public string[] Categories { get; set; }

        private class ScriptView : IUnitTestsView
        {
            public void SetItems(IList<Result> items, TestsMetricsCalculator metrics){}
            public void Clear(){}
        }

        public IList<SkyAgentWork> ServerBuildAgentsData(IList<ServerAgent> agents, ISkyContext context)
        {
            using (var p = CreatePackage(DataFolderPath, context))
            {
                var i = 0;
                return p.PrepareToRun(agents.Count, Categories, Categories.Length>0 ? (bool?)IncludeCategories : null, false)
                    .Select(x => new SkyAgentWork
                    {
                        Agent = agents[i++],
                        Config = JsonSerializer.SerializeToString(x)
                    })
                    .ToArray();
            }
        }

        private ProcessPackage CreatePackage(string folder, ISkyContext context)
        {
            var p = new ProcessPackage(Path.Combine(folder, GetTestDllRelativePath()), 
                NunitAgentPath, RunAsx86, false,
                Path.GetTempPath(),
                CommandBeforeTestsRun, new ScriptView(), RunAsx86Path, Framework);
            if (!p.EnsurePathIsValid())
            {
                throw new ArgumentException("Incorrect path: " + context);
            }
            p.DoRefresh(x => { }, x => { throw new ArgumentException("Can't receive tests list"); });
            return p;
        }

        private string GetTestDllRelativePath()
        {
            var fullDataPath = Path.GetFullPath(DataFolderPath);
            var fullTestDllPath = Path.GetFullPath(TestDllPath);
            return fullTestDllPath.Substring(fullDataPath.Length);
        }

        public string ServerBuildResultByAgentResults(IList<SkyAgentWork> results)
        {
            return JsonSerializer.SerializeToString(results.Select(x=>x.Report));
        }

        public string AgentExecute(string workingDirectory, string agentData, ISkyContext context)
        {
            var packages = JsonSerializer.DeserializeFromString<IList<Result>>(agentData);
            using (var p = CreatePackage(workingDirectory, context))
            {
                var synchronizer = new Synchronizer(1);
                p.DoRun(o => { }, p.Items, new[] {packages}, false, new string[0], false, 0, synchronizer,
                    new SimpleUpdater(context, synchronizer), true);
                return JsonSerializer.SerializeToString(p.Items);
            }
        }

    }
}