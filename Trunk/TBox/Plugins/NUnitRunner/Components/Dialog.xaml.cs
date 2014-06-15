using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
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
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Plugins.NUnitRunner.Code;
using Mnk.TBox.Plugins.NUnitRunner.Code.Settings;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.TBox.Plugins.NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    sealed partial class Dialog
    {
        private readonly string nunitAgentPath;
        private readonly string runAsx86Path;
        private IServiceContainer container;
        private TestsConfig packageConfig;
        private ITestsFixture testsFixture;
        private TestsResults results;
        private TestConfig config;
        public Dialog(string nunitAgentPath, string runAsx86Path)
        {
            this.nunitAgentPath = nunitAgentPath;
            this.runAsx86Path = runAsx86Path;
            InitializeComponent();
            Framework.ItemsSource = new[] {"net-2.0", "net-4.0"};
            Mode.ItemsSource = new[] { TestsRunnerMode.Process, TestsRunnerMode.MultiProcess };
            Progress.OnStartClick += StartClick;
            //load icons
            foreach (DictionaryEntry res in Properties.Resources.ResourceManager.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true))
            {
                var icon = res.Value as Icon;
                CachedIcons.Icons[res.Key.ToString()] = icon.ToImageSource();
            }
        }

        public void ShowDialog(TestConfig cfg)
        {
            if (IsVisible)
            {
                ShowAndActivate();
                return;
            }
            config = cfg;
            DataContext = config;
            Title = Path.GetFileName(config.Key);
            ShowAndActivate();
            Dispatcher.BeginInvoke(new Action(() => RefreshClick(this, null)));
        }

        private void RecreatePackage()
        {
            var items = (results == null) ? null : results.Items;
            DisposePackage();
            packageConfig = new TestsConfig
            {
                NunitAgentPath = nunitAgentPath,
                RunAsx86Path = runAsx86Path,
                TestDllPath = config.Key,
                RunAsx86 = config.RunAsx86,
                RunAsAdmin = config.RunAsAdmin,
                DirToCloneTests = config.DirToCloneTests,
                CommandBeforeTestsRun = config.CommandBeforeTestsRun,
                RuntimeFramework = config.RuntimeFramework,
                ProcessCount = config.ProcessCount,
                OptimizeOrder = config.UsePrefetch,
                IncludeCategories = config.UseCategories ? (bool?)config.IncludeCategories : null,
                CopyToSeparateFolders = config.CopyToSeparateFolders,
                CopyMasks = config.CopyMasks.CheckedItems.Select(x => x.Key).ToArray(),
                NeedSynchronizationForTests = config.NeedSynchronizationForTests && config.ProcessCount > 1,
                StartDelay = config.StartDelay,
                NeedOutput = true,
                Mode = config.Mode
            };
            container = ServicesRegistrar.Register();
            testsFixture = container.GetInstance<ITestsFixture>();
            if (items != null) results = new TestsResults(items);
        }

        private void DisposePackage()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
                testsFixture = null;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            RecreatePackage();
            if (testsFixture!=null && !testsFixture.EnsurePathIsValid(packageConfig))
            {
                Close();
                return;
            }
            var time = Environment.TickCount;
            View.Clear();
            Statistics.Clear();
            var caption = Path.GetFileName(config.Key);
            DialogsCache.ShowProgress(u => DoRefresh(time), caption, this, false);
        }

        private void DoRefresh(int time)
        {
            results = testsFixture.Refresh(packageConfig);
            
            Mt.Do(this, () =>
            {
                if (results.IsFailed)
                {
                    Close();
                }
                else
                {
                    View.SetItems(results);
                    Statistics.SetItems(results);
                    Categories.ItemsSource = GetCategories();
                    RefreshView(time);
                }
            });
        }

        public CheckableDataCollection<CheckableData> GetCategories()
        {
            return new CheckableDataCollection<CheckableData>(
                results.Metrics.Tests
                .SelectMany(x => x.Categories)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new CheckableData { Key = x })
                );
        }


        private void StartClick(object sender, RoutedEventArgs e)
        {
            RecreatePackage();
            if (!testsFixture.EnsurePathIsValid(packageConfig))
            {
                return;
            }
            PrepareUiToRun(false);
            var time = Environment.TickCount;
            Progress.Start(
                u => DoStart(time, u)
                );
        }

        private void DoStart(int time, IUpdater updater)
        {
            IList<Result> items = null;
            Mt.Do(this, ()=>
            {
                items = View.GetCheckedTests();
                packageConfig.Categories =
                    ((CheckableDataCollection<CheckableData>)Categories.ItemsSource)
                        .CheckedItems.Select(x => x.Key)
                        .ToArray();
            });
            results = testsFixture.Run(packageConfig, results, new SimpleUpdater(updater), items);
            Mt.Do(this, () =>
                {
                    View.SetItems(results);
                    Statistics.SetItems(results);
                    RefreshView(time);
                    PrepareUiToRun(true);
                }
            );
        }

        private void PrepareUiToRun(bool enable)
        {
            Tabs.SelectedIndex = 0;
            Settings.IsEnabled = enable;
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
