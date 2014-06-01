using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;
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
    [Bool]
    public bool IncludeCategories { get; set; }
    [StringList(CanBeEmpty = true)]
    public string[] Categories { get; set; }

    private class ScriptView : ITestsView
    {
        public void SetItems(IList<Result> items, ITestsMetricsCalculator metrics) { }
        public void Clear() { }
    }

    public IList<SkyAgentWork> ServerBuildAgentsData(string workingDirectory, IList<ServerAgent> agents)
    {
        using (var container = CreateContainer(CreateConfig(agents.Count), new NullUpdater()))
        {
            using (var p = CreatePackage(workingDirectory, container))
            {
                var i = 0;
                return p.DivideTests()
                    .Select(x => new SkyAgentWork
                    {
                        Agent = agents[i++],
                        Config = JsonSerializer.SerializeToString(x)
                    })
                    .ToArray();
            }
        }
    }

    private IPackage<IProcessTestConfig> CreatePackage(string folder, IServiceContainer container)
    {
        Console.WriteLine("Folder: " + folder);
        var path = Path.Combine(folder, GetTestDllRelativePath());
        var p = container.GetInstance<IPackage<IProcessTestConfig>>();
        if (!p.EnsurePathIsValid())
        {
            throw new ArgumentException("Incorrect path: " + path);
        }
        p.RefreshErrorEventHandler += x => { throw new ArgumentException("Can't receive tests list from:" + path); };
        p.Refresh();
        return p;
    }

    private static IServiceContainer CreateContainer(IProcessTestConfig config, IUpdater updater)
    {
        return ServicesRegistrar.Register(config, new ScriptView(), new SimpleUpdater(updater));
    }

    private IProcessTestConfig CreateConfig(int agentsCount)
    {
        return new ProcessTestConfig
        {
            CopyMasks = PathMasksToInclude,
            CommandBeforeTestsRun = CommandBeforeTestsRun,
            DirToCloneTests = Path.GetTempPath(),
            OptimizeOrder = false,
            NeedOutput = false,
            NeedSynchronizationForTests = false,
            ProcessCount = agentsCount,
            RuntimeFramework = Framework,
            TestDllPath = TestDllPath,
            Categories = Categories,
            IncludeCategories = Categories.Length > 0 ? (bool?)IncludeCategories : null,
            RunAsx86 = RunAsx86,
            RunAsx86Path = RunAsx86Path,
            NunitAgentPath = NunitAgentPath,
            RunAsAdmin = false,
        };
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
            var tmc = new TestsMetricsCalculator();
            tmc.Refresh(all);
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
        var failed = results.Where(x => x.IsFailed).ToArray();
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
        using (var container = CreateContainer(CreateConfig(1), context))
        {
            using (var p = CreatePackage(workingDirectory, container))
            {
                p.Run(packages);
                return JsonSerializer.SerializeToString(p.Items);
            }
        }
    }

}
