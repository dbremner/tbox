using System;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Components
{
	public class PanelToICheckable : UserControl
	{
		private readonly StackPanel spPanel;
		private readonly CheckBox ctrlNone;
        private readonly CheckBox ctrlAll;
		private ICheckableItemsView checkableItemsView;

		public PanelToICheckable()
		{
            Margin = new Thickness(0);
            Padding = new Thickness(0);
			spPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
			spPanel.Children.Add(ctrlAll = CreateControl(WPFControlsLang.All, ctrlAll_Click, true));
			spPanel.Children.Add(ctrlNone = CreateControl(WPFControlsLang.None, ctrlNone_Click, false));
			Content = spPanel;
		}

        private static CheckBox CreateControl(string text, RoutedEventHandler click, bool isChecked)
		{
            var ctrl = new CheckBox { Content = text, Margin = new Thickness(5,0,5,0), IsChecked = isChecked };
            if (isChecked) ctrl.Unchecked += click;
            else           ctrl.Checked += click;
			return ctrl;
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
			get { return ctrlNone.IsEnabled; }
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
			ctrlNone.IsEnabled = state != false && count > 0;
			ctrlAll.IsEnabled = state != true && count > 0;
		}

		private void ctrlAll_Click(object sender, RoutedEventArgs e)
		{
		    e.Handled = true;
		    ctrlAll.IsChecked = true;
			CheckableList.SetCheck(true);
			View.Refresh();
			OnCheckChangedByPanel();
		}

		private void ctrlNone_Click(object sender, RoutedEventArgs e)
		{
            e.Handled = true;
            ctrlNone.IsChecked = false;
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
