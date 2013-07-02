namespace WFControls.Components.DataManagers {
    partial class ListBoxMan {
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
			this.btnClear = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnDel = new System.Windows.Forms.Button();
			this.lbParams = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnClear
			// 
			this.btnClear.Location = new System.Drawing.Point(3, 90);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(41, 23);
			this.btnClear.TabIndex = 4;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(3, 32);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(41, 23);
			this.btnEdit.TabIndex = 2;
			this.btnEdit.Text = "Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(3, 3);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(41, 23);
			this.btnAdd.TabIndex = 1;
			this.btnAdd.Text = "Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnDel
			// 
			this.btnDel.Location = new System.Drawing.Point(3, 61);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(41, 23);
			this.btnDel.TabIndex = 3;
			this.btnDel.Text = "Del";
			this.btnDel.UseVisualStyleBackColor = true;
			// 
			// lbParams
			// 
			this.lbParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbParams.FormattingEnabled = true;
			this.lbParams.IntegralHeight = false;
			this.lbParams.Location = new System.Drawing.Point(50, 3);
			this.lbParams.Name = "lbParams";
			this.lbParams.Size = new System.Drawing.Size(131, 111);
			this.lbParams.TabIndex = 0;
			// 
			// ListBoxMan
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lbParams);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnDel);
			this.Name = "ListBoxMan";
			this.Size = new System.Drawing.Size(184, 117);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.ListBox lbParams;
    }
}
