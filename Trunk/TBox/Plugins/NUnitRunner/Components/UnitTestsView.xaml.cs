using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using NUnit.Core;
using NUnitRunner.Code.Settings;

namespace NUnitRunner.Components
{
	/// <summary>
	/// Interaction logic for UnitTestsView.xaml
	/// </summary>
	public partial class UnitTestsView
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
				Description.Text = string.Empty;
				return;
			}
			Description.Text =
				selected.Message + Environment.NewLine +
				selected.StackTrace;
		}

		private void FilterChanged(object sender, RoutedEventArgs e)
		{
			if (Results.ItemsSource == null) return;
			var view = CollectionViewSource.GetDefaultView(Results.ItemsSource);
			if (view == null) return;
			if (OnlyFailed)
			{
				view.Filter = x => IsFailed((Result)x);
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

	}
}
