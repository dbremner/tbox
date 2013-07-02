using System;
using System.Windows;
using System.Windows.Controls;
using Common.UI.ModelsContainers;

namespace WPFControls.Components
{
	public class PanelToICheckable : UserControl
	{
		private readonly StackPanel spPanel;
		private readonly Button btnNone;
		private readonly Button btnAll;
		private ICheckableItemsView checkableItemsView;

		public PanelToICheckable()
		{
			spPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
			spPanel.Children.Add(btnAll = CreatButton("All", btnAll_Click));
			spPanel.Children.Add(btnNone = CreatButton("None", btnNone_Click));
			Content = spPanel;
		}

		private static Button CreatButton(string text, RoutedEventHandler click)
		{
			var btn = new Button { Content = text, Margin = new Thickness(5, 2, 5, 2), Width = 75 };
			btn.Click += click;
			return btn;
		}

		public HorizontalAlignment PanelHorizontalAlignment
		{
			get { return spPanel.HorizontalAlignment; }
			set { spPanel.HorizontalAlignment = value; }
		}

		public ICheckableItemsView View
		{
			get { return checkableItemsView; }
			set
			{
				if (checkableItemsView != null) checkableItemsView.OnCheckChanged -= OnCheckChanged;
				checkableItemsView = value;
				if (checkableItemsView != null) checkableItemsView.OnCheckChanged += OnCheckChanged;
				OnCheckChanged(null, null);
			}
		}

		public bool IsAnyChecked
		{
			get { return btnNone.IsEnabled; }
		}

		private ICheckableList CheckableList
		{
			get
			{
				return (ICheckableList)
					((checkableItemsView == null) ? null : checkableItemsView.ItemsSource);
			}
		}

		private void OnCheckChanged(object sender, RoutedEventArgs e)
		{
			var checkableList = CheckableList;
			var count = (checkableList == null) ? 0 : checkableList.ValuesCount;
			var state = (checkableList == null) ? null : checkableList.IsChecked;
			btnNone.IsEnabled = state != false && count > 0;
			btnAll.IsEnabled = state != true && count > 0;
		}

		private void btnAll_Click(object sender, RoutedEventArgs e)
		{
			CheckableList.SetCheck(true);
			View.Refresh();
			OnCheckChangedByPanel();
		}

		private void btnNone_Click(object sender, RoutedEventArgs e)
		{
			CheckableList.SetCheck(false);
			View.Refresh();
			OnCheckChangedByPanel();
		}


		private void OnCheckChangedByPanel()
		{
			var ch = CheckChangedByPanel;
			if (ch != null) ch(this, null);
		}

		public event EventHandler<RoutedEventArgs> CheckChangedByPanel;
	}
}
