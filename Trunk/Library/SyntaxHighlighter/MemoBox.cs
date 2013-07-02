using System;
using System.Windows.Forms;
using Common;
using Common.Base.Log;
using WFControls.OS;

namespace SyntaxHighlighter
{
	public partial class MemoBox : Form
	{
		public MemoBox()
		{
			InitializeComponent();
		}

		public string Caption
		{
			set { Text = value; }
			get { return Text; }
		}

		public string Value
		{
			set { shBox.Value = value; }
			get { return shBox.Value; }
		}

		public ContextMenu EditContextMenu
		{
			set { shBox.tbText.ContextMenu = value; }
			get { return shBox.tbText.ContextMenu; }
		}

		public void Clear()
		{
			shBox.tbText.ResetText();
		}

		public void ShowDialog(string caption, string value)
		{
			Caption = caption;
			Value = value;
			if(!Visible)
			{
				base.ShowDialog();
			}
		}

		private void btnToClipboard_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(Value);
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void BigMessageBox_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			Clear();
		}
	}
}
