using System;
using System.Windows.Forms;

namespace SyntaxHighlighter
{
	public partial class SyntaxHighlighting : UserControl
	{
		public SyntaxHighlighting()
		{
			InitializeComponent();
			tbText.TextChanged += tbResponse_TextChanged;
			foreach (ToolStripItem item in cbFormat.DropDownItems)
			{
				item.Click += cbFormat_SelectedIndexChanged;
			}
		}

		public string Value
		 {
			set
			{
				tbText.Caret.Position = 0;
				tbText.Scrolling.ScrollToCaret();
				tbText.Text = value;
			}
			get { return tbText.Text; }
		}

		public string Format
		{
			get { return cbFormat.Text; }
			set
			{
				tbText.ConfigurationManager.Language = "asm";
				tbText.Lexing.SetKeywords(0, string.Empty);
				tbText.ConfigurationManager.Language = cbFormat.Text = value;
				tbText.Lexing.Colorize();
			}
		}


		private void tbResponse_TextChanged(object sender, EventArgs e)
		{
			lSize.Text = tbText.Text.Length.ToString();
		}

		private void cbFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			Format = sender.ToString();
		}

	}
}
