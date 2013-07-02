using System;
using System.Windows.Forms;

namespace WFControls.Forms
{
    public partial class InputComboBox : Form
    {
        public InputComboBox()
        {
            InitializeComponent();
        }

        public string Value
        {
            get { return cbText.Text; }
            set { cbText.Text = value; }
        }

        public string[] Values
        {
            get
            {
                if (cbText.Items.Count <= 0)
                {
                    return null;
                }
                else
                {
                    var ret = new string[cbText.Items.Count];
                    for (int i = 0; i < cbText.Items.Count; ++i)
                    {
                        ret[i] = cbText.Items[i].ToString();
                    }
                    return ret;
                }
            }
            set
            {
                cbText.Items.Clear();
                cbText.Items.AddRange(value);
            }
        }

        Func<string, bool> m_validator;
        public DialogResult ShowDialog(string question, string caption, Func<string, bool> validator)
        {
            m_validator = validator;
            Text = caption;
            lbQuestion.Text = question;
            cbText_TextChanged(null, null);
            cbText.Focus();
            DialogResult result = this.ShowDialog();
            cbText.Text = cbText.Text.Trim();
            return result;
        }

        public DialogResult ShowDialog(string question, string caption, string value, Func<string, bool> validator)
        {
            Add(value);
            cbText.Text = value;
            return this.ShowDialog(question, caption, validator);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Add(cbText.Text);
            Close();
        }

        private void Add(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                int id = cbText.Items.IndexOf(value);
                if (id != 0)
                {
                    if (id != -1)
                    {
                        cbText.Items.RemoveAt(id);
                    }
                    cbText.Items.Insert(0, value);
                    cbText.SelectedIndex = 0;
                }
            }
        }

        public ComboBoxStyle DropDownStyle
        {
            get { return cbText.DropDownStyle; }
            set { cbText.DropDownStyle = value; }
        }

        private void InputComboBox_Shown(object sender, EventArgs e)
        {
            cbText.Focus();
        }

        private void cbText_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = (cbText.Text.Length > 0) && (m_validator != null && m_validator(cbText.Text));
        }
    }
}
