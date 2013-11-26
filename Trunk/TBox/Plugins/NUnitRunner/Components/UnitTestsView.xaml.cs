using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Common.UI.ModelsContainers;
using NUnit.Core;
using PluginsShared.UnitTests.Interfaces;
using PluginsShared.UnitTests.Settings;

namespace NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for UnitTestsView.xaml
    /// </summary>
    public partial class UnitTestsView : IUnitTestsView
    {
        public UnitTestsView()
        {
            InitializeComponent();
            Panel.View = Results;
        }

        public IEnumerable ItemsSource
        {
            get { return Results.ItemsSource; }
            set
            {
                Results.ItemsSource = value;
                FilterChanged(null, null);
                SelectedTestChanged(null, null);
            }
        }

        private bool onlyFailed;
        public bool OnlyFailed
        {
            get { return onlyFailed; }
            set
            {
                onlyFailed = value;
                FilterChanged(null, null);
            }
        }

        public void Refresh()
        {
            Results.Items.Refresh();
            FilterChanged(null, null);
        }

        private void OnTestChecked(object sender, RoutedEventArgs e)
        {
            Results.OnCheckChangedEvent(sender, e);
        }

        private void SelectedTestChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = Results.SelectedValue as Result;
            if (selected == null)
            {
                Description.Value = string.Empty;
                return;
            }
            Description.Value =
                selected.Message + Environment.NewLine +
                selected.StackTrace;
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            if (Results.ItemsSource == null) return;
            var view = CollectionViewSource.GetDefaultView(Results.ItemsSource);
            if (view == null) return;
            Predicate<Result> failedFilter = null;
            Predicate<Result> filter = null;
            if (OnlyFailed) filter = failedFilter = IsFailed;
            var text = Filter.Text.Trim();
            if (!string.IsNullOrEmpty(text))
            {
                Predicate<Result> textFilter = x => x.Key.IndexOf(text, StringComparison.InvariantCultureIgnoreCase) >= 0;
                filter = filter != null ?
                    x => failedFilter(x) && textFilter(x) :
                    textFilter;
            }
            if (filter != null)
            {
                view.Filter = x => filter((Result)x);
            }
            else
            {
                view.Filter = null;
            }
        }

        private static bool IsFailed(Result i)
        {
            return i.State == ResultState.Failure || i.State == ResultState.Error;
        }

        public void UpdateFilter(bool onlyFail)
        {
            if (ItemsSource == null) return;
            OnlyFailed = onlyFail;
        }

        public void SetItems(CheckableDataCollection<Result> items)
        {
            ItemsSource = items;
        }

        public void Clear()
        {
            ItemsSource = null;
        }
    }
}
