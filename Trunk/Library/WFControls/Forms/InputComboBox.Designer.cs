﻿namespace WFControls.Forms
{
    partial class InputComboBox
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
			this.lbQuestion = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbText = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// lbQuestion
			// 
			this.lbQuestion.AutoSize = true;
			this.lbQuestion.Location = new System.Drawing.Point(12, 9);
			this.lbQuestion.Name = "lbQuestion";
			this.lbQuestion.Size = new System.Drawing.Size(35, 13);
			this.lbQuestion.TabIndex = 0;
			this.lbQuestion.Text = "label1";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(141, 50);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(222, 50);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// cbText
			// 
			this.cbText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbText.FormattingEnabled = true;
			this.cbText.Location = new System.Drawing.Point(12, 25);
			this.cbText.Name = "cbText";
			this.cbText.Size = new System.Drawing.Size(285, 21);
			this.cbText.TabIndex = 0;
			this.cbText.TextChanged += new System.EventHandler(this.cbText_TextChanged);
			// 
			// InputComboBox
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(309, 96);
			this.Controls.Add(this.cbText);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.lbQuestion);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(2560, 123);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(195, 123);
			this.Name = "InputComboBox";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "InputComboBox";
			this.Shown += new System.EventHandler(this.InputComboBox_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label lbQuestion;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbText;
	}
}