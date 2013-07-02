namespace WFControls.Components.DataManagers {
    partial class CheckedListBoxMan {
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
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.lbParams = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point( 3, 90 );
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size( 41, 23 );
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point( 3, 32 );
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size( 41, 23 );
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point( 3, 3 );
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size( 41, 23 );
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point( 3, 61 );
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size( 41, 23 );
            this.btnDel.TabIndex = 3;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnAll.Location = new System.Drawing.Point( 51, 113 );
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size( 47, 23 );
            this.btnAll.TabIndex = 5;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            // 
            // btnNone
            // 
            this.btnNone.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.btnNone.Location = new System.Drawing.Point( 104, 113 );
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size( 47, 23 );
            this.btnNone.TabIndex = 6;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            // 
            // lbParams
            // 
            this.lbParams.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.lbParams.FormattingEnabled = true;
            this.lbParams.IntegralHeight = false;
            this.lbParams.Location = new System.Drawing.Point( 50, 0 );
            this.lbParams.Name = "lbParams";
            this.lbParams.Size = new System.Drawing.Size( 101, 111 );
            this.lbParams.TabIndex = 7;
            // 
            // CheckedListBoxMan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add( this.lbParams );
            this.Controls.Add( this.btnNone );
            this.Controls.Add( this.btnAll );
            this.Controls.Add( this.btnClear );
            this.Controls.Add( this.btnEdit );
            this.Controls.Add( this.btnAdd );
            this.Controls.Add( this.btnDel );
            this.Name = "CheckedListBoxMan";
            this.Size = new System.Drawing.Size( 154, 145 );
            this.ResumeLayout( false );

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.CheckedListBox lbParams;
    }
}
