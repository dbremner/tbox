using System.Windows;
using System.Windows.Controls;
using WPFControls.Code.EditPanel;
using WPFControls.Tools;


namespace WPFControls.Components.EditButtons
{
	public class EditButtonsPanel : UserControl
	{
		public StackPanel SpPanel { get; private set; }
		private readonly Button btnAdd;
		private readonly Button btnClone;
		private readonly Button btnEdit;
		private readonly Button btnDel;
		private readonly Button btnClear;

		public EditButtonsPanel()
		{
			SpPanel = new StackPanel();
			SpPanel.Children.Add(btnAdd = CreatButton("Add", btnAdd_Click));
			SpPanel.Children.Add(btnClone = CreatButton("Clone", btnClone_Click));
			SpPanel.Children.Add(btnEdit = CreatButton("Edit", btnEdit_Click));
			SpPanel.Children.Add(btnDel = CreatButton("Del", btnDel_Click));
			SpPanel.Children.Add(btnClear = CreatButton("Clear", btnClear_Click));
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
			btnEdit.IsEnabled = setting;
			if (setting) UpdateButtons();
		}

		private void UpdateButtons()
		{
			var itemsCount = config.Items.Count;
			btnClear.IsEnabled = itemsCount > 0;
			btnClone.IsEnabled =
			btnDel.IsEnabled =
			btnEdit.IsEnabled = btnClear.IsEnabled && config.SelectedIndex > -1;
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

	}
}
