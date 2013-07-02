using System;
using System.Windows.Forms;
using WFControls.Forms;

namespace WFControls.Components.Dialogs
{
    public sealed class InputText : Base
    {
        readonly InputBox m_dialog = new InputBox();
        readonly Func<string, bool> m_validator;


        public InputText(string caption, string addTemplate, string editTemplate, string delTemplate, string clearTemplate, Func<string, bool> validator) :
            base(caption, addTemplate, editTemplate, delTemplate, clearTemplate)
        {
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
