using System;
using System.Windows.Forms;

namespace WFControls.Forms
{
    public partial class InputBox : Form
    {
        public InputBox()
        {
            InitializeComponent();
        }

        public string Value
        {
            get { return edText.Text; }
            set { edText.Text = value; }
        }

        Func<string, bool> m_validator;
        public DialogResult ShowDialog(string question, string caption, Func<string, bool> validator)
        {
            this.m_validator = validator;
            Text = caption;
            lbQuestion.Text = question;
            edText_TextChanged(null, null);
            edText.Focus();
            DialogResult result = this.ShowDialog();
            edText.Text = edText.Text.Trim();
            return result;
        }

        public DialogResult ShowDialog(string question, string caption, string value, Func<string, bool> validator)
        {
            Value = value;
            return this.ShowDialog(question, caption, validator);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void edText_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (edText.Text.Length > 0) && (m_validator != null && m_validator(edText.Text));
        }

        private void InputText_Shown(object sender, EventArgs e)
        {
            edText.Focus();
        }

    }
}
