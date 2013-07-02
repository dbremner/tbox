using ScintillaNET;

namespace SyntaxHighlighter {
    partial class SyntaxHighlighting {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyntaxHighlighting));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cbFormat = new System.Windows.Forms.ToolStripDropDownButton();
            this.vbscriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pythonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.psqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mssqlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.htmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.csToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbText = new ScintillaNET.Scintilla();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbText)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.cbFormat,
            this.toolStripStatusLabel2,
            this.lSize});
            this.statusStrip1.Location = new System.Drawing.Point(0, 226);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(358, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(66, 17);
            this.toolStripStatusLabel1.Text = "Highlighting:";
            // 
            // cbFormat
            // 
            this.cbFormat.AutoSize = false;
            this.cbFormat.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cbFormat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.vbscriptToolStripMenuItem,
            this.pythonToolStripMenuItem,
            this.psqlToolStripMenuItem,
            this.mssqlToolStripMenuItem,
            this.htmlToolStripMenuItem,
            this.jsToolStripMenuItem,
            this.csToolStripMenuItem,
            this.xmlToolStripMenuItem,
            this.textToolStripMenuItem});
            this.cbFormat.Image = ((System.Drawing.Image)(resources.GetObject("cbFormat.Image")));
            this.cbFormat.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cbFormat.Name = "cbFormat";
            this.cbFormat.Size = new System.Drawing.Size(100, 20);
            this.cbFormat.Text = "text";
            // 
            // vbscriptToolStripMenuItem
            // 
            this.vbscriptToolStripMenuItem.Name = "vbscriptToolStripMenuItem";
            this.vbscriptToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.vbscriptToolStripMenuItem.Text = "vbscript";
            // 
            // pythonToolStripMenuItem
            // 
            this.pythonToolStripMenuItem.Name = "pythonToolStripMenuItem";
            this.pythonToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.pythonToolStripMenuItem.Text = "python";
            // 
            // psqlToolStripMenuItem
            // 
            this.psqlToolStripMenuItem.Name = "psqlToolStripMenuItem";
            this.psqlToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.psqlToolStripMenuItem.Text = "psql";
            // 
            // mssqlToolStripMenuItem
            // 
            this.mssqlToolStripMenuItem.Name = "mssqlToolStripMenuItem";
            this.mssqlToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.mssqlToolStripMenuItem.Text = "mssql";
            // 
            // htmlToolStripMenuItem
            // 
            this.htmlToolStripMenuItem.Name = "htmlToolStripMenuItem";
            this.htmlToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.htmlToolStripMenuItem.Text = "html";
            // 
            // jsToolStripMenuItem
            // 
            this.jsToolStripMenuItem.Name = "jsToolStripMenuItem";
            this.jsToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.jsToolStripMenuItem.Text = "js";
            // 
            // csToolStripMenuItem
            // 
            this.csToolStripMenuItem.Name = "csToolStripMenuItem";
            this.csToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.csToolStripMenuItem.Text = "cs";
            // 
            // xmlToolStripMenuItem
            // 
            this.xmlToolStripMenuItem.Name = "xmlToolStripMenuItem";
            this.xmlToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.xmlToolStripMenuItem.Text = "xml";
            // 
            // textToolStripMenuItem
            // 
            this.textToolStripMenuItem.Name = "textToolStripMenuItem";
            this.textToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.textToolStripMenuItem.Text = "text";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(30, 17);
            this.toolStripStatusLabel2.Text = "Size:";
            // 
            // lSize
            // 
            this.lSize.Name = "lSize";
            this.lSize.Size = new System.Drawing.Size(13, 17);
            this.lSize.Text = "0";
            // 
            // tbText
            // 
            this.tbText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbText.AutoComplete.CancelAtStart = false;
            this.tbText.AutoComplete.ListString = "";
            this.tbText.Indentation.ShowGuides = true;
            this.tbText.Location = new System.Drawing.Point(3, 3);
            this.tbText.Margins.Margin0.Width = 40;
            this.tbText.Name = "tbText";
            this.tbText.Size = new System.Drawing.Size(352, 220);
            this.tbText.TabIndex = 13;
            // 
            // SyntaxHighlighting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbText);
            this.Controls.Add(this.statusStrip1);
            this.Name = "SyntaxHighlighting";
            this.Size = new System.Drawing.Size(358, 248);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbText)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lSize;
        private System.Windows.Forms.ToolStripDropDownButton cbFormat;
        private System.Windows.Forms.ToolStripMenuItem vbscriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pythonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem psqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mssqlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem htmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem csToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem textToolStripMenuItem;
        public Scintilla tbText;

    }
}
