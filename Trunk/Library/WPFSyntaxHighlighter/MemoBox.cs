using System;
using System.Windows;
using System.Windows.Controls;
using Mnk.Library.Localization.WPFSyntaxHighlighter;
using Mnk.Library.WpfControls.Code.Log;
using Mnk.Library.WpfControls.Dialogs;

namespace Mnk.Library.WpfSyntaxHighlighter
{
	/// <summary>
	/// Interaction logic for MemoBox.xaml
	/// </summary>
	public class MemoBox : DialogWindow, ICaptionedLog
	{
		public int EntriesCount { get; set; }
		public event EventHandler OnClear;
		private readonly SyntaxHighlighter shText = new SyntaxHighlighter();
		private readonly Button btnClear = new Button { Content = WPFSyntaxHighlighterLang.Clear };
		private readonly Button btnToClipboard = new Button { Content = WPFSyntaxHighlighterLang.ToClipboard, IsDefault = true };
		private readonly Button btnClose = new Button { Content = WPFSyntaxHighlighterLang.Close, IsCancel = true };
		public MemoBox()
		{
			ShowInTaskbar = true;
			Width = MinWidth = 640;
			Height = MinHeight = 480;
			var dock = new DockPanel();
			var stack = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
			dock.Children.Add(stack);
			AddButton(stack, btnClear, btnClear_Click);
			AddButton(stack, btnToClipboard, btnToClipboard_Click);
			AddButton(stack, btnClose, btnClose_Click);
			DockPanel.SetDock(stack, Dock.Bottom);
			dock.Children.Add(shText);
			Content = dock;
			Clear();
		}

		private static void AddButton(StackPanel stack, Button btn, RoutedEventHandler callback)
		{
			stack.Children.Add(btn);
			btn.Margin = new Thickness(5);
			btn.Width = 100;
			btn.Click += callback;
		}

		public string Format
		{
			get { return shText.Format; }
			set { shText.Format = value; }
		}

	    public bool IsReadOnly
	    {
	        get { return shText.IsReadOnly; }
            set { shText.IsReadOnly = value; }
	    }

	    private void btnClear_Click(object sender, RoutedEventArgs e)
		{
			Clear();
		}

		private void btnToClipboard_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(shText.Text);
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			Hide();
		}

		public void ShowDialog(string caption, string text, string format = "", Window owner = null)
		{
			Owner = owner ?? Application.Current.MainWindow;
			Title = caption;
			shText.Text = text;
			if (!string.IsNullOrEmpty(format))
			{
				shText.Format = format;
			}
			ShowAndActivate();
		}

		public void Write(string caption, string value)
		{
			++EntriesCount;
			shText.AppendText(caption + Environment.NewLine + value + Environment.NewLine + Environment.NewLine); ;
		}

		public void Clear()
		{
			EntriesCount = 0;
			shText.Clear();
			if (OnClear != null) { OnClear(this, null); }
		}
	}
}
