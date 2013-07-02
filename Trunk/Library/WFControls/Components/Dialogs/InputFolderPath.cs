using System;
using System.Windows.Forms;

namespace WFControls.Components.Dialogs
{
    public class InputFolderPath : Base
    {
        readonly FolderBrowserDialog m_dialog = new FolderBrowserDialog();
        readonly Func<string, bool> m_validator;
        readonly string m_invalidPathTemplate;

        public InputFolderPath(string caption, string addTemplate, string editTemplate, string delTemplate, string clearTemplate, string invalidPathTemplate, Func<string, bool> validator) :
            base(caption, addTemplate, editTemplate, delTemplate, clearTemplate)
        {
            m_invalidPathTemplate = invalidPathTemplate;
            m_validator = validator;
            m_dialog.Description = caption;
            m_dialog.ShowNewFolderButton = true;
        }

        public override bool Add(out string newName)
        {
            if (m_dialog.ShowDialog() == DialogResult.OK)
            {
                if (m_validator(m_dialog.SelectedPath))
                {
                    newName = m_dialog.SelectedPath;
                    return true;
                }
                else
                {
                    MessageBox.Show(string.Format(m_invalidPathTemplate, m_dialog.SelectedPath), m_caption,
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            newName = string.Empty;
            return false;
        }

        public override bool Edit(string name, out string newName)
        {
            if (!string.IsNullOrEmpty(name))
            {
                m_dialog.SelectedPath = name;
            }
            if (m_dialog.ShowDialog() == DialogResult.OK)
            {
                if (m_validator(m_dialog.SelectedPath))
                {
                    newName = m_dialog.SelectedPath;
                    return true;
                }
                else
                {
                    MessageBox.Show(string.Format(m_invalidPathTemplate, m_dialog.SelectedPath), m_caption,
                            MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            newName = string.Empty;
            return false;
        }
    }
}
