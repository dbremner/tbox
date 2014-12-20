using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using LightInject;
using Mnk.Library.Common.MT;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.WpfControls;
using Mnk.Library.WpfControls.Dialogs;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Locales.Localization.Plugins.NUnitRunner;
using Mnk.TBox.Plugins.NUnitRunner.Code;
using Mnk.TBox.Plugins.NUnitRunner.Code.Settings;
using NUnit.Core;
using ExecutionContext = Mnk.Library.ParallelNUnit.Packages.ExecutionContext;

namespace Mnk.TBox.Plugins.NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    sealed partial class Dialog
    {
        private readonly ITestsConfigurator testsConfigurator;
        private IServiceContainer container;
        private IList<TestsConfig> testsConfigs;
        private IMultiTestsFixture testsFixture;
        private IList<ExecutionContext> results;
        private TestSuiteConfig suiteConfig;
        public Dialog(ITestsConfigurator testsConfigurator)
        {
            this.testsConfigurator = testsConfigurator;
            InitializeComponent();
            Framework.ItemsSource = new[] { "", "net-2.0", "net-4.0", "net-4.5" };
            Mode.ItemsSource = new[] { TestsRunnerMode.Process, TestsRunnerMode.MultiProcess };
            Progress.OnStartClick += StartClick;
            //load icons
            foreach (DictionaryEntry res in Properties.Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true))
            {
                var icon = res.Value as Icon;
                CachedIcons.Icons[res.Key.ToString()] = icon.ToImageSource();
            }
        }

        public void ShowDialog(TestSuiteConfig cfg)
        {
            if (IsVisible)
            {
                ShowAndActivate();
                return;
            }
            Tabs.SelectedIndex = 1;
            suiteConfig = cfg;
            DataContext = suiteConfig;
            Title = suiteConfig.Key;
            ShowAndActivate();
            Dispatcher.BeginInvoke(new Action(() => RefreshClick(this, null)));
        }

        private void RecreatePackage()
        {
            DisposePackage();
            testsConfigs = suiteConfig.FilePathes
                .Select(x => testsConfigurator.CreateConfig(x.Key, suiteConfig))
                .ToArray(); 
            container = ServicesRegistrar.Register();
            testsFixture = container.GetInstance<IMultiTestsFixture>();
        }

        private void DisposePackage()
        {
            if (container == null) return;
            container.Dispose();
            container = null;
            if (results != null)
            {
                foreach (var result in results)
                {
                    result.Dispose();
                }
                results = null;
                testsFixture = null;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            var exists = (results == null) ? null : 
                results.ToDictionary(x=>x.Config.TestDllPath, x=>x.Results.Items);
            results = null;
            RecreatePackage();
            View.Clear();
            Statistics.Clear();
            if (!EnsureTestsExists())
            {
                return;
            }
            DialogsCache.ShowProgress(u => DoRefresh(Environment.TickCount, exists), suiteConfig.Key, this, false);
        }

        private bool EnsureTestsExists()
        {
            if (!suiteConfig.FilePathes.CheckedItems.Any())
            {
                MessageBox.Show(NUnitRunnerLang.CantRefreshUnitTests, Title, MessageBoxButton.OK, MessageBoxImage.Stop);
                return false;
            }
            return true;
        }

        private void DoRefresh(int time, IDictionary<string, IList<Result>> exists)
        {
            results = testsFixture.Refresh(testsConfigs, suiteConfig.AssembliesCount);

            if (exists != null)
            {
                foreach (var item in exists)
                {
                    var exist = results.FirstOrDefault(
                        x => string.Equals(x.Config.TestDllPath, item.Key, StringComparison.OrdinalIgnoreCase));
                    if (exist != null)
                    {
                        exist.Results = new TestsResults(item.Value);
                    }
                }
            }

            Mt.Do(this, () =>
            {
                if (!results.Any(x=>x.Results==null || x.Results.IsFailed))
                {
                    var items = new TestsResults(results.SelectMany(x => x.Results.Items).ToArray());
                    View.SetItems(items);
                    Statistics.SetItems(items);
                    Categories.ItemsSource = GetCategories();
                    RefreshView(time);
                }
            });
        }

        public IEnumerable<CheckableData> GetCategories()
        {
            return new CheckableDataCollection<CheckableData>(
                results.SelectMany(x => x.Results.Metrics.Tests)
                .SelectMany(x => x.Categories)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new CheckableData { Key = x })
                );
        }


        private void StartClick(object sender, RoutedEventArgs e)
        {
            var time = Environment.TickCount;
            PrepareUiToRun(false);
            Progress.Start(
                u => DoStart(time, u)
                );
        }

        private void DoStart(int time, IUpdater updater)
        {
            IList<Result> items = null;
            Mt.Do(this, ()=>
            {
                items = View.GetTests();
                var categories = ((CheckableDataCollection<CheckableData>) Categories.ItemsSource)
                    .CheckedItems.Select(x => x.Key)
                    .ToArray();
                foreach (var testsConfig in testsConfigs)
                {
                    testsConfigurator.UpdateConfig(testsConfig, suiteConfig);
                    testsConfig.Categories = categories;
                }
                foreach (var result in items)
                {
                    result.State = ResultState.NotRunnable;
                    result.Refresh();
                }
            });
            var checkedTests = items.Where(x => !x.Children.Any()).ToArray();
            testsFixture.Run(testsConfigs, 
                suiteConfig.AssembliesCount,
                results.Select(x=>CopyExecutionContext(x, items)).ToArray(), 
                new GroupUpdater(updater, checkedTests.Length),
                checkedTests: checkedTests);
            Mt.Do(this, () =>
                {
                    var result = new TestsResults(checkedTests, true);
                    View.SetItems(result);
                    Statistics.SetItems(result);
                    RefreshView(time);
                    PrepareUiToRun(true);
                }
            );
        }

        private static ExecutionContext CopyExecutionContext(ExecutionContext source, IList<Result> exists)
        {
            return new ExecutionContext
            {
                Config = source.Config,
                Container = source.Container,
                Path = source.Path,
                RetValue = source.RetValue,
                StartTime = source.StartTime,
                TestsFixture = source.TestsFixture,
                Results = new TestsResults(source.Results.Items)
            };
        }

        private void PrepareUiToRun(bool enable)
        {
            Tabs.SelectedIndex = 1;
            SettingsTab.IsEnabled = FilePathesTab.IsEnabled = enable;
            btnCancel.IsEnabled = enable;
            btnRefresh.IsEnabled = enable;
        }

        private void RefreshView(int time)
        {
            View.Refresh((Environment.TickCount - time) / 1000);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!btnRefresh.IsEnabled) e.Cancel = true;
            else base.OnClosing(e);
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposePackage();
        }
    }
}
