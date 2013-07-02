namespace WFControls.Components.Controls {
	partial class Combo {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lCaption = new System.Windows.Forms.Label();
			this.cbData = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// lCaption
			// 
			this.lCaption.AutoSize = true;
			this.lCaption.Location = new System.Drawing.Point(3, 0);
			this.lCaption.Name = "lCaption";
			this.lCaption.Size = new System.Drawing.Size(35, 13);
			this.lCaption.TabIndex = 0;
			this.lCaption.Text = "label1";
			// 
			// cbData
			// 
			this.cbData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cbData.FormattingEnabled = true;
			this.cbData.Location = new System.Drawing.Point(6, 16);
			this.cbData.Name = "cbData";
			this.cbData.Size = new System.Drawing.Size(141, 21);
			this.cbData.TabIndex = 1;
			this.cbData.SelectedIndexChanged += new System.EventHandler(this.cbData_SelectedIndexChanged);
			this.cbData.TextChanged += new System.EventHandler(this.cbData_TextChanged);
			// 
			// Combo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.cbData);
			this.Controls.Add(this.lCaption);
			this.Name = "Combo";
			this.Size = new System.Drawing.Size(150, 40);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lCaption;
		public System.Windows.Forms.ComboBox cbData;
	}
}
