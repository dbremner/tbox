using System;
using System.IO;
using System.Linq;
using System.Windows;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using NUnitRunner.Code.Settings;
using ParallelNUnit.Infrastructure;
using ParallelNUnit.Infrastructure.Packages;
using ParallelNUnit.Infrastructure.Updater;
using WPFControls.Code.OS;
using WPFControls.Dialogs;

namespace NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    sealed partial class Dialog
    {
        private readonly string nunitAgentPath;
        private readonly string runAsx86Path;
        private ProcessPackage package;
        private TestConfig config;
        private readonly UnitTestsView view = new UnitTestsView();
        public Dialog(string nunitAgentPath, string runAsx86Path)
        {
            this.nunitAgentPath = nunitAgentPath;
            this.runAsx86Path = runAsx86Path;
            InitializeComponent();
            Panel.Content = view;
        }

        public void ShowDialog(TestConfig cfg)
        {
            if (IsVisible)
            {
                ShowAndActivate();
                return;
            }
            config = cfg;
            RecreatePackage();
            DataContext = config;
            Title = Path.GetFileName(package.FilePath);
            ShowAndActivate();
            RefreshClick(this, null);
        }

        private void RecreatePackage()
        {
            var items = (package == null)?null : package.Items;
            DisposePackage();
            package = new ProcessPackage(config.Key, nunitAgentPath, config.RunAsx86, config.RunAsAdmin,
                                         config.DirToCloneTests,
                                         config.CommandBeforeTestsRun, view, runAsx86Path);
            if (items != null) package.Items = items;
        }

        private void DisposePackage()
        {
            if (package != null)
            {
                package.Dispose();
            }
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
            var time = Environment.TickCount;
            view.Clear();
            RecreatePackage();
            var caption = Path.GetFileName(config.Key);
            DialogsCache.ShowProgress(
                u => package.DoRefresh(o=>DoRefresh(time), o => Mt.Do(this, Close)),
                caption, this, false);
        }

        private void DoRefresh(int time)
        {
            Mt.Do(this, () =>
            {
                package.ApplyResults(false);
                Categories.ItemsSource = package.Categories;
                view.Refresh((Environment.TickCount - time) / 1000, string.Empty);
            });
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!package.EnsurePathIsValid())
            {
                return;
            }
            var caption = Path.GetFileName(config.Key);
            RecreatePackage();
            var categories =
                ((CheckableDataCollection<CheckableData>)Categories.ItemsSource)
                    .CheckedItems.Select(x => x.Key)
                    .ToArray();
            var packages = package.PrepareToRun(config.ProcessCount, categories, config.UseCategories ? (bool?)config.IncludeCategories : null, config.UsePrefetch, view.GetCheckedTests());
            var time = Environment.TickCount;
            var synchronizer = new Synchronizer(config.ProcessCount);
            DialogsCache.ShowProgress(
                u => package.DoRun(o => OnRunEnd(time), package.Items, packages, config.CopyToSeparateFolders, config.CopyMasks.CheckedItems.Select(x => x.Key).ToArray(), config.NeedSynchronizationForTests && config.ProcessCount > 1, config.StartDelay, synchronizer, new SimpleUpdater(u, synchronizer), true),
                caption, this, false);
        }

        private void OnRunEnd(int time)
        {
            Mt.Do(this,
                  () =>
                  {
                      package.ApplyResults(config.UsePrefetch);
                      view.Refresh((Environment.TickCount - time) / 1000, package.Output);
                  }
                );
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposePackage();
        }
    }
}
