using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Interfaces;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Tools.SkyNet.Common;
using ServiceStack.Text;

public class NUnitTests : ISkyScript
{
    private const string NunitAgentPath = "NUnitAgent.exe";
    private const string RunAsx86Path = "RunAsx86.exe";
    [Directory]
    public string DataFolderPath { get; set; }
    [StringList("*.dll", "*.config", CanBeEmpty = false)]
    public string[] PathMasksToInclude { get; set; }
    [File]
    public string TestDllPath { get; set; }
    [String(CanBeEmpty = true)]
    public string CommandBeforeTestsRun { get; set; }
    [Bool(true)]
    public bool RunAsx86 { get; set; }
    [String(CanBeEmpty = true)]
    public string Framework { get; set; }
    [Bool(false)]
    public bool IncludeCategories { get; set; }
    [StringList(CanBeEmpty = true)]
    public string[] Categories { get; set; }

    private class ScriptView : IUnitTestsView
    {
        public void SetItems(IList<Result> items, TestsMetricsCalculator metrics) { }
        public void Clear() { }
    }

    public IList<SkyAgentWork> ServerBuildAgentsData(string workingDirectory, IList<ServerAgent> agents)
    {
        using (var p = CreatePackage(workingDirectory))
        {
            var i = 0;
            return p.PrepareToRun(agents.Count, Categories, Categories.Length > 0 ? (bool?)IncludeCategories : null, false)
                .Select(x => new SkyAgentWork
                {
                    Agent = agents[i++],
                    Config = JsonSerializer.SerializeToString(x)
                })
                .ToArray();
        }
    }

    private ProcessPackage CreatePackage(string folder)
    {
        Console.WriteLine("Folder: " + folder);
        var path = Path.Combine(folder, GetTestDllRelativePath());
        var p = new ProcessPackage(path,
            NunitAgentPath, RunAsx86, false,
            Path.GetTempPath(),
            CommandBeforeTestsRun, new ScriptView(), RunAsx86Path, Framework);
        if (!p.EnsurePathIsValid())
        {
            throw new ArgumentException("Incorrect path: " + path);
        }
        p.DoRefresh(x => { }, x => { throw new ArgumentException("Can't receive tests list from:" + path); });
        return p;
    }

    private string GetTestDllRelativePath()
    {
        var fullDataPath = Path.GetFullPath(DataFolderPath);
        var fullTestDllPath = Path.GetFullPath(TestDllPath);
        var path = fullTestDllPath.Substring(fullDataPath.Length);
        while (path.StartsWith("\\") || path.StartsWith("/")) path = path.Substring(1);
        return path;
    }

    public string ServerBuildResultByAgentResults(IList<SkyAgentWork> results)
    {
        var all = results.Where(x => !x.IsFailed).SelectMany(x => JsonSerializer.DeserializeFromString<IList<Result>>(x.Report)).ToArray();
        var sb = new StringBuilder();
        if (all.Any())
        {
            var tmc = new TestsMetricsCalculator(all);
            sb.AppendFormat(
                "Tests run: {0}, Errors: {1}, Failures: {2}, Inconclusive: {3}, Not run: {4}, Invalid: {5}, Ignored: {6}, Skipped: {7}",
                tmc.Passed,
                tmc.Errors,
                tmc.Failures,
                tmc.Inconclusive,
                tmc.NotRun.Length,
                tmc.Invalid,
                tmc.Ignored,
                tmc.Skipped
                );
            PrintArray("Errors and Failures:", tmc.Failed, true, sb);
            PrintArray("Tests Not Run:", tmc.NotRun, false, sb);
        }
        var failed = results.Where(x => x.IsFailed);
        if (failed.Any())
        {
            sb.AppendLine("Agents exceptions:");
            sb.AppendLine(string.Join(Environment.NewLine, failed.Select(x => x.Report)));
        }
        return sb.ToString();
    }

    private static void PrintArray(string message, Result[] items, bool stackTrace, StringBuilder sb)
    {
        if (items.Length == 0) return;
        sb.AppendLine();
        sb.AppendLine(message);
        var i = 0;
        foreach (var r in items)
        {
            sb.AppendFormat("{0}) {1} : {2}{3}{4}", ++i, r.State, r.FullName,
            string.IsNullOrEmpty(r.Message) ? string.Empty : ("\n   " + r.Message),
            stackTrace ? ("\n" + r.StackTrace + "\n") : string.Empty
            );
        }
        sb.AppendLine();
    }

    public string AgentExecute(string workingDirectory, string agentData, ISkyContext context)
    {
        var packages = JsonSerializer.DeserializeFromString<IList<Result>>(agentData);
        using (var p = CreatePackage(workingDirectory))
        {
            var synchronizer = new Synchronizer(1);
            p.DoRun(o => { }, p.Items, new[] { packages }, false, new string[0], false, 0, synchronizer,
                new SimpleUpdater(context, synchronizer), true);
            return JsonSerializer.SerializeToString(p.Items);
        }
    }

}
