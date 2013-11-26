using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common.Communications;
using Common.Communications.Interprocess;
using NUnit.Core;
using NUnit.Util;
using ServiceStack.Text;
using extended.nunit;
using extended.nunit.Interfaces;

namespace NUnitAgent.Code
{
	sealed class Runner : IDisposable
	{
		private readonly string handle;
		public Runner(string handle)
		{
			this.handle = handle;
			CoreExtensions.Host.InitializeService();
			var settingsService = new SettingsService(false);
			ServiceManager.Services.AddService(settingsService);
			ServiceManager.Services.AddService(new ProjectService());
			ServiceManager.Services.AddService(new DomainManager());
			ServiceManager.Services.AddService(new TestLoader());
			ServiceManager.Services.AddService(new AddinRegistry());
			ServiceManager.Services.AddService(new AddinManager());
			ServiceManager.Services.InitializeServices();
			CoreExtensions.Host.AddinRegistry = Services.AddinRegistry;
		}

		public int Run(string path, int[] items, bool fast)
		{
			var p = CreatePackage(path);
			using (var runner = new TestDomain())
			{
				if (!runner.Load(p))
				{
					MessageBox.Show("Can't load: " + path);
					return -1;
				}
				try
				{
					runner.Run(new RemoteListener{Handle = handle, Fast = fast},
								new Filter { Items = new HashSet<int>(items) },
								false,
								LoggingThreshold.Off
						);
				    return items.Length;
				}
				finally
				{
					runner.Unload();
				}
			}
		}

		public int CollectTests(string path)
		{
			using (var runner = new TestDomain())
			{
				var p = CreatePackage(path);
				if (!runner.Load(p)) return -1;
				var list = new List<Result>();
				try
				{
					CollectResults(runner.Test, list, new string[0]);
                    return list.Count;
				}
				finally
				{
					new InterprocessClient<INunitRunnerClient>(handle).Instance.SetCollectedTests(JsonSerializer.SerializeToString(list.ToArray()));
					runner.Unload();
				}
			}
		}

		private static TestPackage CreatePackage(string path)
		{
			var p = new TestPackage(path);
			p.Settings["ProcessModel"] = ProcessModel.Single;
			p.Settings["DomainUsage"] = DomainUsage.None;
			p.Settings["ShadowCopyFiles"] = false;
			return p;
		}

		private static void CollectResults(ITest result, ICollection<Result> items, string[] ownerCategories)
		{
            var categories = ownerCategories.Concat(GetCategories(result)).Distinct().ToArray();
			if (string.Equals(result.TestType, "TestMethod"))
			{
				items.Add(new Result
				{
					Id = int.Parse(result.TestName.TestID.ToString()),
					Key = result.TestName.FullName,
                    Categories = categories
				});
			}
			if (result.Tests == null) return;
			foreach (ITest r in result.Tests)
			{
                CollectResults(r, items, categories);
			}
		}

	    private static IList<string> GetCategories(ITest result)
	    {
	        return (result.Categories == null ? 
                new object[0] : result.Categories.Cast<object>())
                .Select(x=>x.ToString())
                .ToArray();
	    }

	    public void Dispose()
		{
			CoreExtensions.Host.UnloadService();
		}
	}
}
