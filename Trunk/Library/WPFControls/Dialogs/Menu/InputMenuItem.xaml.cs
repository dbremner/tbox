using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Mnk.Library.Localization.WPFControls;

namespace Mnk.Library.WpfControls.Dialogs.Menu
{
	/// <summary>
	/// Interaction logic for MenuDialog.xaml
	/// </summary>
	public sealed partial class InputMenuItem 
	{
		private bool isSuccess = false;
		private Func<string, bool> validator;
		public InputMenuItem()
		{
			InitializeComponent();
		}

		public KeyValuePair<bool, string> ShowDialog(string question, string caption, string value, Func<string, bool> validator, IEnumerable<MenuDialogItem> values, Window owner, bool showInTaskBar = false)
		{
		    btnOk.ToolTip = string.Empty;
			ShowInTaskbar = showInTaskBar;
			this.validator = validator;
			Owner = owner;
			Icon = owner == null ? null : owner.Icon;
			Title = caption;
			Question.Text = question;
			isSuccess = false;
			Items.ItemsSource = values;
			Selector_OnSelectionChanged(null, null);
			ShowDialog();
			return new KeyValuePair<bool, string>(isSuccess,
				(Items.SelectedItem != null) ? FormatPath((MenuDialogItem)Items.SelectedValue) : string.Empty);
		}

		private string FormatPath(MenuDialogItem item)
		{
			var path = item.Name;
			while (item.Parent != null)
			{
				item = item.Parent;
				path = item.Name + Environment.NewLine + path;
			}
			return path;
		}

		private void Selector_OnSelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> routedPropertyChangedEventArgs)
		{
			btnOk.IsEnabled = CanChoose();
            btnOk.ToolTip = btnOk.IsEnabled ? string.Empty : WPFControlsLang.ValueAlreadyExistOrIncorrect;
		}

		private bool CanChoose()
		{
			var item = Items.SelectedValue as MenuDialogItem;
			return item != null && item.Children.Count == 0 && validator(FormatPath(item));
		}

		private void BtnOk_OnClick(object sender, RoutedEventArgs e)
		{
			isSuccess = true;
			Hide();
		}

		private void BtnCancelClick(object sender, RoutedEventArgs e)
		{
			Hide();
		}

		private void OnMouseLeftButton(object sender, MouseButtonEventArgs e)
		{
			if (e.ClickCount != 2) return;
			if (CanChoose()) BtnOk_OnClick(sender, e);
		}
	}
}
