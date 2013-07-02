﻿namespace WFControls.Forms {
	public partial class Progress
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progress));
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// progressBar
			// 
			resources.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			// 
			// btnCancel
			// 
			resources.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label
			// 
			resources.ApplyResources(this.label, "label");
			this.label.Name = "label";
			// 
			// Progress
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.Controls.Add(this.label);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.progressBar);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Progress";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProgress_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label;
	}
}