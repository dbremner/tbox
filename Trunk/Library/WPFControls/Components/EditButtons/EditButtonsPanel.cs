using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Localization.WPFControls;
using Mnk.Library.WpfControls.Code;
using Mnk.Library.WpfControls.Tools;


namespace Mnk.Library.WpfControls.Components.EditButtons
{
	public class EditButtonsPanel : UserControl
	{
		public StackPanel SpPanel { get; private set; }
		private readonly Button btnAdd;
		private readonly Button btnClone;
		private readonly Button btnEdit;
		private readonly Button btnDel;
		private readonly Button btnClear;
		private readonly Button btnSort;

		public EditButtonsPanel()
		{
			SpPanel = new StackPanel();
			SpPanel.Children.Add(btnAdd = CreatButton(WPFControlsLang.Add, btnAdd_Click));
			SpPanel.Children.Add(btnClone = CreatButton(WPFControlsLang.Clone, btnClone_Click));
			SpPanel.Children.Add(btnEdit = CreatButton(WPFControlsLang.Edit, btnEdit_Click));
			SpPanel.Children.Add(btnDel = CreatButton(WPFControlsLang.Del, btnDel_Click));
			SpPanel.Children.Add(btnClear = CreatButton(WPFControlsLang.Clear, btnClear_Click));
			SpPanel.Children.Add(btnSort = CreatButton(WPFControlsLang.Sort, btnSort_Click));
			Content = SpPanel;

			EnableButtons(false);
		}

		private static Button CreatButton(string text, RoutedEventHandler click)
		{
			var btn = new Button { Content = text, Margin = new Thickness(2), MinWidth = 40 };
			btn.Click += click;
			return btn;
		}

		public Orientation Orientation
		{
			get { return SpPanel.Orientation; }
			set { SpPanel.Orientation = value; }
		}

		private Configuration config;
		internal void SetConfig(Configuration cfg)
		{
			config = cfg;
			EnableButtons(cfg != null);

			btnAdd.SetVisibility(config.Dialog.IsAddVisible);
			btnClear.SetVisibility(config.Dialog.IsClearVisible);
			btnClone.SetVisibility(config.Dialog.IsCloneVisible);
			btnDel.SetVisibility(config.Dialog.IsDelVisible);
			btnEdit.SetVisibility(config.Dialog.IsEditVisible);
			btnSort.SetVisibility(true);
		}

		public void OnSelectionChanged()
		{
			UpdateButtons();
		}

		private void EnableButtons(bool setting)
		{
			btnAdd.IsEnabled =
			btnClear.IsEnabled =
			btnClone.IsEnabled =
			btnDel.IsEnabled =
			btnEdit.IsEnabled = 
			btnSort.IsEnabled = 
			setting;
			if (setting) UpdateButtons();
		}

		private void UpdateButtons()
		{
			var itemsCount = config.Items.Count;
			btnClear.IsEnabled = itemsCount > 0;
            btnDel.IsEnabled = btnClear.IsEnabled && config.SelectedIndexes.Length > 0;
            btnClone.IsEnabled =
			btnEdit.IsEnabled = btnClear.IsEnabled && config.SelectedIndexes.Length == 1;
			btnSort.IsEnabled = itemsCount > 1;
		}

		private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Add()) UpdateButtons();
		}

		private void btnClone_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Clone()) UpdateButtons();
		}

		private void btnEdit_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Edit()) UpdateButtons();
		}

		private void btnDel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Del()) UpdateButtons();
		}

		private void btnClear_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Clear()) UpdateButtons();
		}

		private void btnSort_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (config.Controller.Sort()) UpdateButtons();
		}

	}
}
