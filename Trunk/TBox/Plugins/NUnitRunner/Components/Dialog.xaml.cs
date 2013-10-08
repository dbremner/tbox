using System;
using System.IO;
using System.Windows;
using Localization.Plugins.NUnitRunner;
using NUnitRunner.Code.Settings;
using PluginsShared.UnitTests;
using PluginsShared.UnitTests.Updater;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace NUnitRunner.Components
{
	/// <summary>
	/// Interaction logic for Dialog.xaml
	/// </summary>
	sealed partial class Dialog
	{
		private TestsPackage package;
		private TestConfig config;
		public Dialog()
		{
			InitializeComponent();
		}

		public void ShowDialog(TestConfig cfg, string nunitAgentPath)
		{
			if (IsVisible)
			{
                ShowAndActivate();
			    return;
			}
			config = cfg;
			DisposePackage();
			var view = new UnitTestsView();
            package = new TestsPackage(config.Key, nunitAgentPath, config.RunAsx86, config.RunAsAdmin, config.DirToCloneTests, config.CommandBeforeTestsRun, view);
			Panel.Children.Add(view);
			DataContext = config;
			Title = Path.GetFileName(package.Path);
			ShowAndActivate();
			RefreshClick(this, null);
		}

		private void DisposePackage()
		{
			if (package != null)
			{
				package.Dispose();
			}
			Panel.Children.Clear();
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void RefreshClick(object sender, RoutedEventArgs e)
		{
			if (!package.EnsurePathIsValid())
			{
				Close();
				return;
			}
            package.Reset(config.RunAsx86, config.RunAsAdmin, config.DirToCloneTests, config.CommandBeforeTestsRun);
			var caption = Path.GetFileName(config.Key);
			DialogsCache.ShowProgress(
				u => package.DoRefresh(DoRefresh, o => Mt.Do(this, Close)), 
				caption, this);
		}

		private void DoRefresh(TestsPackage o)
		{
			Mt.Do(this, () =>
			{
				Title = Path.GetFileName(o.Path) + " - [ " + package.Count + " ]";
				package.ApplyResults();
				FilterChanged(null, null);
			});
		}

		private void StartClick(object sender, RoutedEventArgs e)
		{
			if (!package.EnsurePathIsValid())
			{
				return;
			}
			var caption = Path.GetFileName(config.Key);
			package.Reset(config.RunAsx86, config.RunAsAdmin, config.DirToCloneTests, config.CommandBeforeTestsRun);
			var packages = package.PrepareToRun(config.ProcessCount);
			var time = Environment.TickCount;
			var synchronizer = new Synchronizer(config.ProcessCount);
			DialogsCache.ShowProgress(
				u => package.DoRun(o => DoRun(time), packages, config.CopyToSeparateFolders, config.CopyDeep, config.NeedSynchronizationForTests && config.ProcessCount > 1, synchronizer, new SimpleUpdater(u, synchronizer)),
				caption, this);
		}

		private void DoRun(int time)
		{
			Mt.Do(this,
				  () =>
				  {
                      Title = string.Format(NUnitRunnerLang.TestsStateTemplate,
											Path.GetFileName(package.Path),
											package.Count, package.FailedCount,
											(Environment.TickCount - time) / 1000.0);
					  package.ApplyResults();
					  ((UnitTestsView)package.Results).Refresh();
				  }
				);
		}

		private void FilterChanged(object sender, RoutedEventArgs e)
		{
			package.UpdateFilter(Filter.IsChecked == true);
		}

		public override void Dispose()
		{
			base.Dispose();
			DisposePackage();
		}
	}
}
