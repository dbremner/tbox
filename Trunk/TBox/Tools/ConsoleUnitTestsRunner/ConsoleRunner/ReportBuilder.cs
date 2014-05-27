using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using Mnk.Library.Common.Models;
using NUnit.Core;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit.Infrastructure;

namespace Mnk.TBox.Tools.ConsoleUnitTestsRunner.ConsoleRunner
{
    internal static class ReportBuilder
    {
       public static void GenerateReport(string path, string xmlReport, TestsMetricsCalculator tmc, IEnumerable<Result> testResults )
       {
           var doc = new XmlDocument();
           doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));
           var root = doc.AppendChild(CreateElement(doc, "test-results",
               Pair.Create("name", path),
               Pair.Create("total", tmc.Passed.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("errors", tmc.Errors.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("failures", tmc.Failures.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("not-run", tmc.NotRun.Length.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("inconclusive", tmc.Inconclusive.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("ignored", tmc.Ignored.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("skipped", tmc.Skipped.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("invalid", tmc.Invalid.ToString(CultureInfo.InvariantCulture)),
               Pair.Create("date", DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
               Pair.Create("time", DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture))
               ));
           root.AppendChild(CreateElement(doc, "environment",
               Pair.Create("nunit-version", "2.6.3.0"),
               Pair.Create("clr-version", Environment.Version.ToString()),
               Pair.Create("os-version", Environment.OSVersion.ToString()),
               Pair.Create("platform", Environment.OSVersion.Platform.ToString()),
               Pair.Create("cwd", Environment.CurrentDirectory),
               Pair.Create("machine-name", Environment.MachineName),
               Pair.Create("user", Environment.UserName),
               Pair.Create("user-domain", Environment.UserDomainName)
               ));
           root.AppendChild(CreateElement(doc, "culture-info",
               Pair.Create("current-culture", Thread.CurrentThread.CurrentCulture.Name),
               Pair.Create("current-uiculture", Thread.CurrentThread.CurrentUICulture.Name)
               ));
           foreach (var r in testResults)
           {
               AppendNode(r, root, doc);
           }
           doc.Save(xmlReport);
       }

       private static void AppendNode(Result r, XmlNode root, XmlDocument doc)
       {
           var el = CreateElement(doc, r.IsTest ? "test-case" : "test-suite",
                   r.IsTest ? null : Pair.Create("type", r.Type),
                   Pair.Create("name", r.IsTest ? r.FullName : r.Key),
                   Pair.Create("executed", r.Executed.ToString()),
                   Pair.Create("result", r.State.ToString()),
                   Pair.Create("success", (r.State == ResultState.Success).ToString()),
                   Pair.Create("time", string.Format(CultureInfo.InvariantCulture, "{0:0.###}", r.Time)),
                   Pair.Create("asserts", r.AssertCount.ToString(CultureInfo.InvariantCulture))
               );
           if (r.IsTest)
           {
               if (r.State == ResultState.Failure || r.State == ResultState.Error)
               {
                   var f = CreateElement(doc, "failure");
                   if (!string.IsNullOrEmpty(r.Message))
                   {
                       var m = CreateElement(doc, "message");
                       m.AppendChild(doc.CreateCDataSection(r.Message));
                       f.AppendChild(m);
                   }
                   if (!string.IsNullOrEmpty(r.StackTrace))
                   {
                       var m = CreateElement(doc, "stack-trace");
                       m.AppendChild(doc.CreateCDataSection(r.StackTrace));
                       f.AppendChild(m);
                   }
                   el.AppendChild(f);
               }
           }
           else
           {
               var results = CreateElement(doc, "results");
               el.AppendChild(results);
               foreach (var test in r.Children.Cast<Result>())
               {
                   AppendNode(test, results, doc);
               }
           }
           root.AppendChild(el);
       }

       private static XmlElement CreateElement(XmlDocument doc, string name, params Pair<string, string>[] attributes)
       {
           var el = doc.CreateElement(name);
           foreach (var a in attributes.Where(x => x != null))
           {
               var atr = doc.CreateAttribute(a.Key);
               atr.Value = a.Value;
               el.Attributes.Append(atr);
           }
           return el;
       }
    }
}
