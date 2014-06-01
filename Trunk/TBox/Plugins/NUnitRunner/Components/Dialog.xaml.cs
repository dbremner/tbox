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
        private ProcessTestConfig packageConfig;
        private IPackage<IProcessTestConfig> package;
        private TestConfig config;
        private readonly TestsView view = new TestsView();
        public Dialog(string nunitAgentPath, string runAsx86Path)
        {
            this.nunitAgentPath = nunitAgentPath;
            this.runAsx86Path = runAsx86Path;
            InitializeComponent();
            Framework.ItemsSource = new[] {"net-2.0", "net-4.0"};
            Panel.Content = view;
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

        private void RecreatePackage(IUpdater u)
        {
            var items = (package == null)?null : package.Items;
            DisposePackage();
            packageConfig = new ProcessTestConfig
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
                UsePrefetch = config.UsePrefetch,
                IncludeCategories = config.UseCategories ? (bool?)config.IncludeCategories : null,
                CopyToSeparateFolders = config.CopyToSeparateFolders,
                CopyMasks = config.CopyMasks.CheckedItems.Select(x => x.Key).ToArray(),
                NeedSynchronizationForTests = config.NeedSynchronizationForTests && config.ProcessCount > 1,
                StartDelay = config.StartDelay,
                NeedOutput = true
            };
            container = ServicesRegistrator.Register(packageConfig, view, new SimpleUpdater(u));
            package = container.GetInstance<IPackage<IProcessTestConfig>>();
            if (items != null) package.Items = items;
        }

        private void DisposePackage()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
                package = null;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            if (package!=null && !package.EnsurePathIsValid())
            {
                Close();
                return;
            }
            var time = Environment.TickCount;
            view.Clear();
            var caption = Path.GetFileName(config.Key);
            DialogsCache.ShowProgress(u => DoRefresh(time, u), caption, this, false);
        }

        private void DoRefresh(int time, IUpdater updater)
        {
            Mt.Do(this, () => RecreatePackage(updater));
            package.RefreshErrorEvent += o => Mt.Do(this, Close);
            package.RefreshSuccessEvent += o => Mt.Do(this, () =>
            {
                package.Tmc.Refresh(package.Items);
                view.SetItems(package.Items, package.Tmc);
                Categories.ItemsSource = GetCategories();
                RefreshView(time);
            });

            package.Refresh();
        }

        public CheckableDataCollection<CheckableData> GetCategories()
        {
            return new CheckableDataCollection<CheckableData>(
                package.Tmc.Tests
                .SelectMany(x => x.Categories)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new CheckableData { Key = x })
                );
        }


        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!package.EnsurePathIsValid())
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
                RecreatePackage(updater);
                items = view.GetCheckedTests();
                packageConfig.Categories =
                    ((CheckableDataCollection<CheckableData>)Categories.ItemsSource)
                        .CheckedItems.Select(x => x.Key)
                        .ToArray();
            });
            package.TestsFinishedEvent += o => Mt.Do(this, () =>
                {
                    RefreshView(time);
                    PrepareUiToRun(true);
                }
            );

            package.Run(items);
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
            view.Refresh((Environment.TickCount - time) / 1000);
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
