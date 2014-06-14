﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Packages.Common;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Plugins.BookletPagesGenerator.Code;
using Mnk.TBox.Tools.SkyNet.Common;
using ServiceStack.Text;
using Result = Mnk.Library.ParallelNUnit.Core.Result;

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

    public IList<SkyAgentWork> ServerBuildAgentsData(string workingDirectory, IList<ServerAgent> agents)
    {
        var config = CreateConfig(agents.Count);
        using (var container = ServicesRegistrar.Register())
        {
            var p = container.GetInstance<ITestsFixture>();
            var results = CollectInfo(workingDirectory, config, p);
            var i = 0;
            return container.GetInstance<ITestsDivider>()
                .Divide(config, results.Metrics, null)
                .Select(x => new SkyAgentWork
                {
                    Agent = agents[i++],
                    Config = JsonSerializer.SerializeToString(x)
                })
                .ToArray();
        }
    }

    private TestsResults CollectInfo(string folder, ITestsConfig config, ITestsFixture p)
    {
        Console.WriteLine("Folder: " + folder);
        var path = Path.Combine(folder, GetTestDllRelativePath());
        if (!p.EnsurePathIsValid(config))
        {
            throw new ArgumentException("Incorrect path: " + path);
        }
        var results = p.Refresh(config);
        if (results.IsFailed)
            throw new ArgumentException("Can't receive tests list from:" + path);
        return results;
    }

    private ITestsConfig CreateConfig(int agentsCount)
    {
        return new TestsConfig
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
            Type = TestsRunnerType.Thread,
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
            sb.AppendLine();
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
        var config = CreateConfig(1);
        using (var container = ServicesRegistrar.Register())
        {
            var p = container.GetInstance<ITestsFixture>();
            var results = CollectInfo(workingDirectory, config, p);
            results = p.Run(config, results, new SimpleUpdater(context), packages);
            return JsonSerializer.SerializeToString(results.Items);
        }
    }

}
