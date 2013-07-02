using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Encoder.Code;
using WPFControls.Tools;

namespace Encoder.Components
{
	/// <summary>
	/// Interaction logic for Dialog.xaml
	/// </summary>
	partial class Dialog
	{
		private Operation last = null;
		public ObservableCollection<Operation> KnownEncoders { get; set; }

		public void Init(Operation[] operations)
		{
			KnownEncoders = new ObservableCollection<Operation>(operations);
			InitializeComponent();
			BtnClearClick(null, null);
		}

		private void BtnClearClick(object sender, RoutedEventArgs e)
		{
			Source.Value = string.Empty;
			Result.Value = string.Empty;
		}

		private void BtnToClipboardClick(object sender, RoutedEventArgs e)
		{
			if (ConvertOnSourceChanged.IsChecked != true)
			{
				RunLastCommand();
			}
			Clipboard.SetText(Result.Value);
		}

		protected override void OnShow()
		{
			base.OnShow();
			Dispatcher.BeginInvoke(new Func<bool>(Source.Focus));
		}

		public void ShowDialog(string caption, string source, string title, string format)
		{
			Title = caption;
			if(!string.IsNullOrWhiteSpace(format))
			{
				Result.Format = format;
			}
			ShowAndActivate();
			var i = -1;
			last = null;
			foreach (var item in Encoders.Items)
			{
				++i;
				if (!string.Equals(item.ToString(), title)) continue;
				Encoders.SelectedIndex = i;
				last = KnownEncoders[i];
				break;
			}
			Source.Value = source;
			RunLastCommand();
		}

		private void ButtonSwapClick(object sender, RoutedEventArgs e)
		{
			var tmp = Source.Value;
			Source.Value = Result.Value;
			Result.Value = tmp;
			Source_OnTextChanged();
		}

		private void RunLastCommand()
		{
			if (last != null) Selector_OnSelectionChanged(null, null);
		}

		private void Source_OnTextChanged()
		{
			if (ConvertOnSourceChanged.IsChecked == true)
			{
				RunLastCommand();
			}
		}

		private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				last = (Operation)Encoders.SelectedValue;
				Title = "Encoder - [" + last.Header + "]";
				if (!string.IsNullOrWhiteSpace(last.Format))
				{
					Result.Format = last.Format;
				}
				Result.Value = last.Work(Source.Value);
			}
			catch (Exception ex)
			{
				if (e != null) e.Handled = true;
				Result.Value = "Encoder. Unexpected exception!" +
					Environment.NewLine + ex.Message;
			}
		}

		private void CancelClick(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
