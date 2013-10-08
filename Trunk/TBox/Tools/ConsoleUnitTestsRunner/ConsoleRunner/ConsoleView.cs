using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Common.Data;
using Common.UI.ModelsContainers;
using NUnit.Core;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;

namespace ConsoleUnitTestsRunner.ConsoleRunner
{
	internal class ConsoleView : IUnitTestsView
	{
		protected int Skipped { get; set; }
		protected int NotRunnable { get; set; }
		protected int Inconclusive { get; set; }
		protected int Ignored { get; set; }
		protected int Total { get; set; }
		protected Result[] Failed { get; set; }
		private IList<Result> testsResults;

		public void UpdateFilter(bool onlyFailed)
		{
		}

		public void SetItems(CheckableDataCollection<Result> items)
		{
			testsResults = items;
			Failed = items.Where(x => x.State == ResultState.Error || x.State == ResultState.Failure).ToArray();
			Ignored = items.Count(x => x.State == ResultState.Ignored);
			Inconclusive = items.Count(x => x.State == ResultState.Inconclusive);
			Skipped = items.Count(x => x.State == ResultState.Skipped);
			NotRunnable = items.Count(x => x.State == ResultState.NotRunnable);
			Total = items.Count - Failed.Length - Ignored - Inconclusive - Skipped - NotRunnable;
			Console.WriteLine(
				"Tests: {0}, Passed: {1}, Failed: {2}, Ignored: {3}, Inconclusive: {4}, Skipped: {5}, NotRunnable: {6}",
				items.Count,
				Total,
				Failed.Length,
				Ignored,
				Inconclusive,
				Skipped,
				NotRunnable
				);
			if (Failed.Length == 0) return;
			Console.WriteLine("Failed tests:");
			var i = 0;
			foreach (var r in Failed)
			{
				Console.WriteLine("[{0}]: {1}\n{2}", ++i, r.Message, r.StackTrace);
			}
		}

		public void Clear()
		{
		}

		public void GenerateReport(int time, string path)
		{
			var doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", "no"));
			var root = doc.AppendChild(CreateElement(doc, "test-results", 
				Pair.Create("name", path), 
				Pair.Create("total", Total.ToString()), 
				Pair.Create("errors", Failed.Count(x=>x.State == ResultState.Error).ToString()),
				Pair.Create("failures", Failed.Count(x => x.State == ResultState.Failure).ToString()),  
				Pair.Create("not-run", NotRunnable.ToString()),  
				Pair.Create("inconclusive", Inconclusive.ToString()),  
				Pair.Create("ignored", Ignored.ToString()),  
				Pair.Create("skipped", Skipped.ToString()),  
				Pair.Create("date", DateTime.Now.ToShortDateString()),  
				Pair.Create("time", DateTime.Now.ToShortTimeString())
				));
			root.AppendChild(CreateElement(doc, "environment", 
				Pair.Create("nunit-version", "2.5.10.11092"),
				Pair.Create("clr-version", "2.0.50727.5466"),
				Pair.Create("os-version", Environment.OSVersion.ToString()),
				Pair.Create("machine-name", Environment.MachineName),
				Pair.Create("user", Environment.UserName),
				Pair.Create("user-domain", Environment.UserDomainName)
				));
			root.AppendChild(CreateElement(doc, "culture-info", 
				Pair.Create("current-culture", Thread.CurrentThread.CurrentCulture.Name),
				Pair.Create("current-uiculture", Thread.CurrentThread.CurrentUICulture.Name)
				));
			var success = Failed.Length > 0;
			var suite = AppendSuite(path, "Assembly", root, doc, success);
			suite = AppendSuite(Path.GetFileNameWithoutExtension(path),"Namespace", suite, doc, success);
			suite = AppendSuite(Path.GetFileNameWithoutExtension(path),"TestFixture", suite, doc, success);
			foreach (var r in testsResults)
			{
				suite.AppendChild(CreateElement(doc, "test-case", 
					Pair.Create("name", r.Key),
					Pair.Create("executed", "True"),
					Pair.Create("result", r.State.ToString()),
					Pair.Create("success", (r.State == ResultState.Success).ToString()),
					Pair.Create("time", "0"),
					Pair.Create("asserts", "1")
					));
			}
			var reportPath = Path.Combine(Environment.CurrentDirectory, "TestResult.xml");
			doc.Save(reportPath);
			Console.WriteLine("Report saved to: " + reportPath);
		}

		private static XmlNode AppendSuite(string path, string type, XmlNode root, XmlDocument doc, bool success)
		{
			var suite = root.AppendChild(CreateElement(doc, "test-suite",
			                                           Pair.Create("type", type),
			                                           Pair.Create("name", path),
			                                           Pair.Create("executed", "True"),
			                                           Pair.Create("result", success ? "Success" : "Failed"),
			                                           Pair.Create("success", success.ToString()),
			                                           Pair.Create("time", "0"),
			                                           Pair.Create("asserts", "0")
				                             ));
			return suite.AppendChild(CreateElement(doc, "results"));
		}

		private static XmlElement CreateElement(XmlDocument doc, string name, params Pair<string,string>[] attributes )
		{
			var el = doc.CreateElement(name);
			foreach (var a in attributes)
			{
				var atr = doc.CreateAttribute(a.Key);
				atr.Value = a.Value;
				el.Attributes.Append(atr);
			}
			return el;
		}
	}
}
