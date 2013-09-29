using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Integration;
using Common.Tools;
using ScintillaNET;
using WPFControls.Controls;
using WPFControls.Tools;

namespace WPFSyntaxHighlighter
{
	public sealed class SyntaxHighlighter : UserControl
	{
        private bool holded = false; 
		private static readonly IDictionary<string, string[]> KnownTypes = new Dictionary<string, string[]>
			{
				{"xml", new[]{"config", "csproj", "build", "props", "xaml", "resx"}},
				{"html", new[]{"aspx", "ascx", "cshtml", "htm"}},
				{"mssql", new[]{"sql"}},
				{"vbscript", new[]{"vb"}},
				{"cpp", new[]{"c", "cs", "h", "hpp"}},
			};
		private readonly StatusBar statusBar = new StatusBar();
		private readonly ComboBox cbFormats = new ComboBox{
			Width = 70,
			IsReadOnly = true,
			ItemsSource = new[] { "asm", "cpp", "html", "js", "css", "mssql", "python", "text", "vbscript", "xml" },
			SelectedIndex = 7,
		};
		private readonly ExtCheckBox sbWordWrap = new ExtCheckBox { Content = "Word wrap" };
		private readonly ExtCheckBox sbWhitespaces = new ExtCheckBox { Content = "Whitespaces" };
		private readonly StatusBarItem sbSize = new StatusBarItem();
		private readonly WindowsFormsHost wfh;
		private readonly Scintilla editor = new Scintilla
			{
				AcceptsReturn = true,
				AcceptsTab = true,
				IsBraceMatching = true,
				MatchBraces = true,
			};
		public event EventHandler TextChanged;
		public SyntaxHighlighter()
		{
			var panel = new DockPanel();
			panel.Children.Add(statusBar);
			DockPanel.SetDock(statusBar, Dock.Bottom);
			statusBar.Items.Add(new StatusBarItem {Content = "Hightlight:"});
			cbFormats.SelectionChanged += CbFormatsSelectionChanged;
			statusBar.Items.Add(new StatusBarItem { Content = cbFormats });
			sbWordWrap.ValueChanged += sbWordWrap_Checked;
			statusBar.Items.Add(new StatusBarItem { Content = sbWordWrap });
			sbWhitespaces.ValueChanged += sbWhitespaces_Checked;
			statusBar.Items.Add(new StatusBarItem { Content = sbWhitespaces });
			statusBar.Items.Add(new StatusBarItem { Content = "Size:", Margin = new Thickness(10, 0, 0, 0) });
			statusBar.Items.Add(sbSize);

			editor.Margins.Margin0.Width = 32;
			editor.Indentation.TabWidth = 4;
			editor.FindReplace.Indicator.Color = Color.Black;
			editor.TextChanged += EditorTextChanged;
			editor.LostFocus += EditorLostFocus;
			panel.Children.Add(wfh = new WindowsFormsHost { Child = editor });
			sbWhitespaces.ValueChanged += (o, e) => SetValue(IsWhitespacesProperty, sbWhitespaces.IsChecked);
			sbWordWrap.ValueChanged += (o, e) => SetValue(IsWordWrapProperty, sbWordWrap.IsChecked);
			CbFormatsSelectionChanged(null, null);
			Loaded += OnLoaded;
			Content = panel;
		}

        bool CanEdit { get { return !IsReadOnly && !holded; } }

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			var p = Parent.GetParentWindow();
			if (p != null)
			{
				p.Deactivated +=
					(o, e) =>
					{
						wfh.IsEnabled = false;
						wfh.IsEnabled = true;
					};
			}
		}

		private void EditorLostFocus(object sender, EventArgs e)
		{
            if (!CanEdit) return;
			SetValue(ValueProperty, editor.Text);
		}

		private void EditorTextChanged(object sender, EventArgs e)
		{
			sbSize.Content = editor.TextLength;
            if (!CanEdit) return;
			if (TextChanged != null)
				TextChanged(this, e);
		}

		public static readonly DependencyProperty FormatProperty =
			DpHelper.Create<SyntaxHighlighter, string>("Format", (s, v) => s.Format = v);
		public string Format
		{
			get { return editor.ConfigurationManager.Language; }
			set
			{
				if (value == null) value = string.Empty;
				var known = KnownTypes.FirstOrDefault(x => x.Value.Any(y => y.EqualsIgnoreCase(value)));
				if (!string.IsNullOrEmpty(known.Key)) value = known.Key;
				SetValue(FormatProperty, value);
				editor.ConfigurationManager.Language = value;
				if (cbFormats.Text.EqualsIgnoreCase(value)) return;
				for (var i = 0; i < cbFormats.Items.Count; ++i)
				{
					var item = (string)cbFormats.Items[i];
					if (!item.EqualsIgnoreCase(value)) continue;
					cbFormats.SelectedIndex = i;
					return;
				}
			}
		}

		public new bool Focus()
		{
			return editor.Focus();
		}

		public static readonly DependencyProperty ValueProperty =
			DpHelper.Create<SyntaxHighlighter, string>("Value", (s, v) => s.SetValue(v));
		public string Value
		{
			get { return editor.Text; }
			set
			{
                if (CanEdit && string.Equals(editor.Text, value)) return;
				SetValue(value);
			}
		}

		public int Length
		{
			get { return editor.TextLength; }
		}

		private void SetValue(string value)
		{
            ApplyValue(() =>
                {
                    editor.Text = value;
                });

		}

        public void Read(string path)
        {
            ApplyValue(()=>
            {
                editor.ResetText();
	            if (!File.Exists(path)) throw new FileNotFoundException(path);
				var content = NativeBoost.FileSystem.ReadFileContent(path);
	            try
	            {
		            editor.NativeInterface.SendMessageDirect((int) Constants.SCI_SETTEXT, (IntPtr) 0, (IntPtr)content);
	            }
	            finally
	            {
					NativeBoost.Tools.ClearArray(content);
	            }
            });
        }

	    private void ApplyValue(Action setter)
	    {
	        holded = true;
	        editor.SuspendLayout();
	        try
	        {
                HandleReadOnly(setter);
	            editor.UndoRedo.EmptyUndoBuffer();
	            editor.Modified = false;
	            editor.Caret.Position = 0;
                editor.Scrolling.ScrollToLine(0);
	        }
	        finally
	        {
	            editor.ResumeLayout(true);
	            holded = false;
	        }
	    }

        private void HandleReadOnly(Action action)
        {
            var readOnly = IsReadOnly;
            try
            {
                if (readOnly) IsReadOnly = false;
                action();
            }
            finally
            {
                if (readOnly) IsReadOnly = true;
            }
        }

	    public void Select(int start, int end)
		{
			editor.Selection.Range.Start = start;
			editor.Selection.Range.End = end;
			editor.Select();
		}

		public void ScrollToCaret()
		{
			editor.Scrolling.ScrollToCaret();
		}

		private void CbFormatsSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (editor != null)
			{
				Format = (string)cbFormats.SelectedItem;
			}
		}

		private void sbWordWrap_Checked(object sender, RoutedEventArgs e)
		{
			editor.LineWrapping.Mode = sbWordWrap.IsChecked == true ?
				LineWrappingMode.Word : LineWrappingMode.None;
		}

		private void sbWhitespaces_Checked(object sender, RoutedEventArgs e)
		{
			if (sbWhitespaces.IsChecked == true)
			{
				editor.Indentation.ShowGuides = true;
				editor.Whitespace.Mode = WhitespaceMode.VisibleAlways;
			}
			else
			{
				editor.Indentation.ShowGuides = false;
				editor.Whitespace.Mode = WhitespaceMode.Invisible;
			}
		}
		public int MarkAll(string text, bool mathCase)
		{
			var finded = editor.FindReplace.FindAll(text, mathCase ? SearchFlags.MatchCase : SearchFlags.Empty);
			editor.FindReplace.HighlightAll(finded);
			editor.FindReplace.MarkAll(finded);
			return finded.Count;
		}

		public static readonly DependencyProperty IsReadOnlyProperty =
			DpHelper.Create<SyntaxHighlighter, bool>("IsReadOnly", (s, v) => s.IsReadOnly = v);
		public bool IsReadOnly
		{
			get { return editor.IsReadOnly; }
			set
			{
				SetValue(IsReadOnlyProperty, value);
				editor.IsReadOnly = value;
			}
		}

		public static readonly DependencyProperty IsWhitespacesProperty =
			DpHelper.Create<SyntaxHighlighter, bool>("IsWhitespaces", (s, v) => s.IsWhitespaces = v);
		public bool IsWhitespaces
		{
			get { return sbWhitespaces.IsChecked == true; }
			set
			{
				SetValue(IsWhitespacesProperty, value);
				sbWhitespaces.IsChecked = value;
			}
		}

		public static readonly DependencyProperty IsWordWrapProperty =
			DpHelper.Create<SyntaxHighlighter, bool>("IsWordWrap", (s, v) => s.IsWordWrap = v);
		public bool IsWordWrap
		{
			get { return sbWordWrap.IsChecked == true; }
			set
			{
				SetValue(IsWordWrapProperty, value);
				sbWordWrap.IsChecked = value;
			}
		}

		public bool IsStatusBarVisible
		{
			get { return statusBar.IsVisible; }
			set { statusBar.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
		}

		public void AppendText(string s)
		{
            HandleReadOnly(()=>editor.AppendText(s));
		}

		public void Clear()
		{
			ApplyValue(()=>editor.ResetText());
		}
	}
}
