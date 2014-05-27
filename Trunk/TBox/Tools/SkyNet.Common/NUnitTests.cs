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
using Mnk.TBox.Tools.SkyNet.Common.Contracts.Server;
using ServiceStack.Text;

namespace Mnk.TBox.Tools.SkyNet.Common
{
    public class NUnitTests : ISkyScript
    {
        private const string NunitAgentPath = "NUnitAgent.exe";
        private const string RunAsx86Path = "RunAsx86.exe";
        [Directory]
        public string DataFolder { get; set; }
        [File]
        public string TestDllPath { get; set; }
        [StringList("*.dll", "*.config", CanBeEmpty = false)]
        public string[] CopyMasks { get; set; }
        [String]
        public string CommandBeforeTestsRun { get; set; }
        [String]
        public string FrameworkVersion { get; set; }
        [Bool(true)]
        public bool RunAsx86 { get; set; }
        [Bool]
        public bool IncludeCategories { get; set; }
        [StringList(CanBeEmpty=true)]
        public string[] Categories { get; set; }

        private class ScriptView : IUnitTestsView
        {
            public void SetItems(IList<Result> items, TestsMetricsCalculator metrics){}
            public void Clear(){}
        }

        public string[] ServerDivideTasks(ServerAgent[] agents, ISkyContext context)
        {
            using (var p = CreatePackage(context))
            {
                return p.PrepareToRun(agents.Length, Categories, Categories.Length>0 ? (bool?)IncludeCategories : null, false)
                    .Select(JsonSerializer.SerializeToString)
                    .ToArray();
            }
        }

        private ProcessPackage CreatePackage(ISkyContext context)
        {
            var p = new ProcessPackage(Path.Combine(context.TargetFolder, TestDllPath), 
                NunitAgentPath, RunAsx86, false,
                Path.GetTempPath(),
                CommandBeforeTestsRun, new ScriptView(), RunAsx86Path, FrameworkVersion);
            if (!p.EnsurePathIsValid())
            {
                throw new ArgumentException("Incorrect path: " + context);
            }
            p.DoRefresh(x => { }, x => { throw new ArgumentException("Can't receive tests list"); });
            return p;
        }

        public string ServerBuildResult(IDictionary<string, string> results)
        {
            return JsonSerializer.SerializeToString(results);
        }

        public string AgentExecute(string data, ISkyContext context)
        {
            var packages = JsonSerializer.DeserializeFromString<IList<Result>>(data);
            using (var p = CreatePackage(context))
            {
                var synchronizer = new Synchronizer(1);
                p.DoRun(o => { }, p.Items, new[] {packages}, false, new string[0], false, 0, synchronizer,
                    new SimpleUpdater(context, synchronizer), true);
                return JsonSerializer.SerializeToString(p.Items);
            }
        }

    }
}
