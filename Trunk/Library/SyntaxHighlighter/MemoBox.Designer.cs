namespace SyntaxHighlighter
{
	partial class MemoBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnToClipboard = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.shBox = new SyntaxHighlighter.SyntaxHighlighting();
			this.SuspendLayout();
			// 
			// btnToClipboard
			// 
			this.btnToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnToClipboard.Location = new System.Drawing.Point(12, 202);
			this.btnToClipboard.Name = "btnToClipboard";
			this.btnToClipboard.Size = new System.Drawing.Size(75, 23);
			this.btnToClipboard.TabIndex = 1;
			this.btnToClipboard.Text = "To Clipboard";
			this.btnToClipboard.UseVisualStyleBackColor = true;
			this.btnToClipboard.Click += new System.EventHandler(this.btnToClipboard_Click);
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(176, 202);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 3;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnClear
			// 
			this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.btnClear.Location = new System.Drawing.Point(93, 202);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(75, 23);
			this.btnClear.TabIndex = 2;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// shBox
			// 
			this.shBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.shBox.Format = "text";
			this.shBox.Location = new System.Drawing.Point(0, 2);
			this.shBox.Name = "shBox";
			this.shBox.Size = new System.Drawing.Size(262, 194);
			this.shBox.TabIndex = 0;
			this.shBox.Value = "";
			// 
			// MemoBox
			// 
			this.AcceptButton = this.btnToClipboard;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(263, 228);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnToClipboard);
			this.Controls.Add(this.shBox);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(271, 255);
			this.Name = "MemoBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "BigMessageBox";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BigMessageBox_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		protected SyntaxHighlighter.SyntaxHighlighting shBox;
		private System.Windows.Forms.Button btnToClipboard;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnClear;
	}
}