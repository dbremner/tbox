using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Common.MT;
using ConsoleUnitTestsRunner.Code;
using ConsoleUnitTestsRunner.Code.Interfaces;
using ConsoleUnitTestsRunner.Code.Settings;
using ConsoleUnitTestsRunner.Code.Updater;
using NUnitRunner.Code.Settings;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace NUnitRunner.Components
{
	/// <summary>
	/// Interaction logic for BatchRunDialog.xaml
	/// </summary>
	sealed partial class BatchRunDialog
	{
		private Config config;
		private string nunitAgentPath;
		private readonly List<TestsPackage> packages = new List<TestsPackage>();
		private readonly string originalCaption;
		public BatchRunDialog()
		{
			InitializeComponent();
			originalCaption = Title;
			DllsPanel.View = Dlls;
		}

		public void ShowDialog(Config cfg, string nunitPath)
		{
			if (IsVisible)
			{
                ShowAndActivate();
			    return;
			}
			nunitAgentPath = nunitPath;
			DisposePackages();
			config = cfg;
			DataContext = config;
			ShowAndActivate();
		}

		private void DisposePackages()
		{
			foreach (var p in packages)
			{
				p.Dispose();
			}
			packages.Clear();
			Results.Items.Clear();
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			if (packages.Any(p => !p.EnsurePathIsValid()))
			{
				return;
			}
			var testPackages = packages.ToDictionary(x => x, x => x.PrepareToRun(1));
			var time = Environment.TickCount;
			DialogsCache.ShowProgress(
				u => DoRun(new GroupUpdater(u, testPackages.Sum(x=>x.Value.Sum(y=>y.Count))), time, testPackages), 
				originalCaption, this);
		}

		private void DoRun(IProgressStatus u, int time, IDictionary<TestsPackage, IList<IList<Result>>> tests)
		{
			var count = 0;
			var synchronizer = new Synchronizer(tests.Count);
			Parallel.ForEach(packages,
				p => p.DoRun(
					o => DoRun(o, time, ref count),
					tests[p], false, 1, config.NeedSyncForBatch && tests.Count > 1,
					synchronizer,
					u));
		}

		private void DoRun(TestsPackage o, int time, ref int count)
		{
			if (++count != packages.Count) return;
			Mt.Do(this,
				  () =>
				  {
					  Title = string.Format("{0} - tests: [ {1} ], failed = [ {2} ], time: {3:0.0}",
											originalCaption,
											packages.Sum(x => x.Count), packages.Sum(x => x.FailedCount),
											(Environment.TickCount - time) / 1000.0);
					  foreach (var p in packages)
					  {
						  ((UnitTestsView)p.Results).Refresh();
					  }
				  }
				);
		}

		private void FilterChanged(object sender, RoutedEventArgs e)
		{
			foreach (var p in packages)
			{
				p.UpdateFilter(Filter.IsChecked == true);
			}
		}

		private void RefreshClick(object sender, RoutedEventArgs e)
		{
			DisposePackages();
			foreach (var item in config.DllPathes.CheckedItems)
			{
                var p = new TestsPackage(item.Key, nunitAgentPath, item.RunAsx86, item.RunAsAdmin, item.DirToCloneTests, item.CommandBeforeTestsRun, new UnitTestsView());
				if (!p.EnsurePathIsValid())
				{
					return;
				}
				packages.Add(p);
				Results.Items.Add(
					new TabItem {Header = Path.GetFileName(item.Key), Content = p.Results});
			}
			DialogsCache.ShowProgress(DoRefresh, originalCaption, this);
		}

		private void DoRefresh(IUpdater u)
		{
			var count = 0;
			var addedPackages = 0;
			Parallel.ForEach(packages, p => 
				p.DoRefresh(
					o => OnRefreshFinish(o, ref count, ref addedPackages),
					o => Mt.Do(this, Close)
				));
		}

		private void OnRefreshFinish(TestsPackage package, ref int count, ref int addedPackages)
		{
			count += package.Count;
			if (++addedPackages != packages.Count) return;
			var added = count;
			Mt.Do(this, () => {
				foreach (var p in packages)
				{
					p.ApplyResults();
				}
				Title = string.Format("{0} - [ {1} ]", originalCaption, added);
				FilterChanged(null,null);
				btnStart.IsEnabled = true;
			});
		}

		private void DllsCheckChanged(object sender, RoutedEventArgs e)
		{
			btnStart.IsEnabled = false;
			btnRefresh.IsEnabled = config.DllPathes.CheckedItems.Any();
		}

		public override void Dispose()
		{
			base.Dispose();
			DisposePackages();
		}
	}
}
