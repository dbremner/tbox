namespace WFControls.Components.DataManagers
{
	partial class CheckedList
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.clbItems = new System.Windows.Forms.CheckedListBox();
			this.btnAll = new System.Windows.Forms.Button();
			this.btnNone = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// clbItems
			// 
			this.clbItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.clbItems.FormattingEnabled = true;
			this.clbItems.IntegralHeight = false;
			this.clbItems.Location = new System.Drawing.Point(0, 0);
			this.clbItems.Name = "clbItems";
			this.clbItems.Size = new System.Drawing.Size(164, 120);
			this.clbItems.TabIndex = 0;
			this.clbItems.SelectedIndexChanged += new System.EventHandler(this.clbItems_SelectedIndexChanged);
			// 
			// btnAll
			// 
			this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAll.Enabled = false;
			this.btnAll.Location = new System.Drawing.Point(8, 126);
			this.btnAll.Name = "btnAll";
			this.btnAll.Size = new System.Drawing.Size(75, 23);
			this.btnAll.TabIndex = 1;
			this.btnAll.Text = "All";
			this.btnAll.UseVisualStyleBackColor = true;
			this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
			// 
			// btnNone
			// 
			this.btnNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNone.Enabled = false;
			this.btnNone.Location = new System.Drawing.Point(89, 126);
			this.btnNone.Name = "btnNone";
			this.btnNone.Size = new System.Drawing.Size(75, 23);
			this.btnNone.TabIndex = 2;
			this.btnNone.Text = "None";
			this.btnNone.UseVisualStyleBackColor = true;
			this.btnNone.Click += new System.EventHandler(this.btNone_Click);
			// 
			// CheckedList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnNone);
			this.Controls.Add(this.btnAll);
			this.Controls.Add(this.clbItems);
			this.Name = "CheckedList";
			this.Size = new System.Drawing.Size(167, 152);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox clbItems;
		private System.Windows.Forms.Button btnAll;
		private System.Windows.Forms.Button btnNone;
	}
}
