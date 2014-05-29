using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using Mnk.Library.Common.UI.Model;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.WpfControls;
using Mnk.Library.WpfWinForms.Icons;
using Mnk.TBox.Plugins.NUnitRunner.Code;
using Mnk.TBox.Plugins.NUnitRunner.Code.Settings;
using Mnk.Library.ParallelNUnit.Infrastructure;
using Mnk.Library.ParallelNUnit.Infrastructure.Packages;
using Mnk.Library.ParallelNUnit.Infrastructure.Updater;
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
        private ProcessPackage package;
        private TestConfig config;
        private readonly UnitTestsView view = new UnitTestsView();
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
                                         config.CommandBeforeTestsRun, view, runAsx86Path, config.Framework);
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
                view.Refresh((Environment.TickCount - time) / 1000);
            });
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!package.EnsurePathIsValid())
            {
                return;
            }
            PrepareUiToRun(false);
            RecreatePackage();
            var categories =
                ((CheckableDataCollection<CheckableData>)Categories.ItemsSource)
                    .CheckedItems.Select(x => x.Key)
                    .ToArray();
            var packages = package.PrepareToRun(config.ProcessCount, categories, config.UseCategories ? (bool?)config.IncludeCategories : null, config.UsePrefetch, view.GetCheckedTests());
            var time = Environment.TickCount;
            var synchronizer = new Synchronizer(config.ProcessCount);
            Progress.Start(
                u => package.DoRun(o => OnRunEnd(time), package.Items, packages, config.CopyToSeparateFolders, config.CopyMasks.CheckedItems.Select(x => x.Key).ToArray(), config.NeedSynchronizationForTests && config.ProcessCount > 1, config.StartDelay, synchronizer, new SimpleUpdater(u, synchronizer), true)
                );
        }

        private void PrepareUiToRun(bool enable)
        {
            Tabs.SelectedIndex = 0;
            Settings.IsEnabled = enable;
            btnCancel.IsEnabled = enable;
            btnRefresh.IsEnabled = enable;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!btnRefresh.IsEnabled) e.Cancel = true;
            else base.OnClosing(e);
        }

        private void OnRunEnd(int time)
        {
            Mt.Do(this,
                  () =>
                  {
                      package.ApplyResults(config.UsePrefetch);
                      view.Refresh((Environment.TickCount - time) / 1000);
                      PrepareUiToRun(true);
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
