using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Common.Data;
using Common.Tools;
using Localization.Plugins.NUnitRunner;
using ParallelNUnit.Core;
using ParallelNUnit.Infrastructure;
using ParallelNUnit.Infrastructure.Interfaces;
using WPFSyntaxHighlighter;

namespace NUnitRunner.Components
{
    /// <summary>
    /// Interaction logic for UnitTestsView.xaml
    /// </summary>
    public partial class UnitTestsView : IUnitTestsView
    {
        private TestsMetricsCalculator tmc;
        private readonly CheckableTreeView results = new CheckableTreeView();
        public UnitTestsView()
        {
            InitializeComponent();
            results.SelectedItemChanged += SelectedTestChanged;
            Panel.Children.Add(results);
        }

        public void Refresh(int time, params string[] output)
        {
            results.Refresh();
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
            TextOutput.Items.Clear();
            for(var i=0;i<output.Length;++i)
            {
                if (string.IsNullOrEmpty(output[i])) continue;
                TextOutput.Items.Add(
                    new TabItem
                        {
                            Header = String.Format("{0} {1}", NUnitRunnerLang.Agent, i+1), 
                            Content = new SyntaxHighlighter{IsReadOnly = true, Value = output[i]}
                        });
            }
            if (TextOutput.Items.Count > 0) TextOutput.SelectedIndex = 0;
        }

        private void SelectedTestChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
        {
            var selected = results.SelectedValue as Result;
            if (selected == null)
            {
                Description.Value = string.Empty;
                return;
            }
            Description.Value =
                selected.Time + Environment.NewLine +
                selected.Description + Environment.NewLine +
                selected.Message + Environment.NewLine +
                selected.StackTrace;
        }

        public void SetItems(IList<Result> items, TestsMetricsCalculator metrics)
        {
            if (results.IsEmpty)
            {
                results.SetItems(items);
            }
            SelectedTestChanged(null, null);
            tmc = metrics;
        }

        public void Clear()
        {
            results.SetItems(null);
        }

        public IList<Result> GetCheckedTests()
        {
            return new TestsMetricsCalculator(results.GetChecked().Cast<Result>()).Tests;
        }
    }
}
