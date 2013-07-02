using System;
using System.Windows.Forms;
using WFControls.Forms;

namespace WFControls.Components.Dialogs
{
    public sealed class InputTextList : Base
    {
        readonly InputComboBox m_dialog = new InputComboBox();
        readonly Func<string, bool> m_validator;

        public string[] Values
        {
            get { return m_dialog.Values; }
            set { m_dialog.Values = value; }
        }

        public InputTextList(string caption, ComboBoxStyle style, string addTemplate, string editTemplate, string delTemplate, string clearTemplate, Func<string, bool> validator) :
            base(caption, addTemplate, editTemplate, delTemplate, clearTemplate)
        {
            m_dialog.DropDownStyle = style;
            m_validator = validator;
        }

        public override bool Add(out string newName)
        {
            if (m_dialog.ShowDialog(m_addTemplate, m_caption, m_validator) == DialogResult.OK)
            {
                newName = m_dialog.Value;
                return true;
            }
            newName = string.Empty;
            return false;
        }

        public override bool Edit(string name, out string newName)
        {
            if (m_dialog.ShowDialog(m_editTemplate, m_caption, name, m_validator) == DialogResult.OK)
            {
                newName = m_dialog.Value;
                return true;
            }
            newName = string.Empty;
            return false;
        }
    }
}
