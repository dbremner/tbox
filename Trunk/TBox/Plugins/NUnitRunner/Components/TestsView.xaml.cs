﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Mnk.Library.Common.Tools;
using Mnk.Library.ParallelNUnit.Contracts;
using Mnk.Library.ParallelNUnit.Core;
using Mnk.Library.ParallelNUnit;
using Mnk.Library.WpfControls;

namespace Mnk.TBox.Plugins.NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for UnitTestsView.xaml
    /// </summary>
    public partial class TestsView : ITestsView
    {
        private ITestsMetricsCalculator tmc;
        public TestsView()
        {
            InitializeComponent();
        }

        public void Refresh(int time)
        {
            if (results.IsEmpty) return;
            Total.Content = tmc.Total;
            Passed.Content = tmc.Passed;
            Failed.Content = tmc.Failures;
            Errors.Content = tmc.Errors;
            Inconclusive.Content = tmc.Inconclusive;
            Invalid.Content = tmc.Invalid;
            Ignored.Content = tmc.Ignored;
            Skipped.Content = tmc.Skipped;
            Time.Content = time.FormatTimeInSec();
            ErrorsAndFailures.ItemsSource = tmc.Failed;
            TestsNotRun.Items.Clear();
            foreach (var item in tmc.NotRun)
            {
                var ti = new TreeViewItem {Header = item.FullName};
                ti.Items.Add(new TreeViewItem {Header = item.Message});
                TestsNotRun.Items.Add(ti);
            }
            TextOutput.Text = string.Join(string.Empty, tmc.All.Select(x => x.Output));
        }

        private void SelectedTestChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
        {
            var selected = results.SelectedValue as Result;
            if (selected == null)
            {
                Description.Text = string.Empty;
                return;
            }
            Description.Text =
                selected.Time + Environment.NewLine +
                selected.Description + Environment.NewLine +
                selected.Message + Environment.NewLine +
                selected.Output + Environment.NewLine +
                selected.StackTrace;
        }

        public void SetItems(IList<Result> items, ITestsMetricsCalculator metrics)
        {
            if (results.IsEmpty)
            {
                results.SetItems(items);
            }
            Mt.Do(this, () => SelectedTestChanged(null, null));
            tmc = metrics;
        }

        public void Clear()
        {
            results.SetItems(null);
        }

        public IList<Result> GetCheckedTests()
        {
            return results.GetChecked().Cast<Result>()
                .SelectMany(x=>x.Collect())
                .ToList();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            results.OnMouseLeftButtonDown(sender, e);
        }
    }
}