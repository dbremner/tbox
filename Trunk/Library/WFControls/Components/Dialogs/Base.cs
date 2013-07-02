using System.Windows.Forms;

namespace WFControls.Components.Dialogs
{
    public abstract class Base
    {
        protected string m_caption;
        protected string m_delTemplate;
        protected string m_clearTemplate;
        protected string m_addTemplate;
        protected string m_editTemplate;


        protected Base(string caption, string addTemplate, string editTemplate, string delTemplate, string clearTemplate)
        {
            m_caption = caption;
            m_addTemplate = addTemplate;
            m_editTemplate = editTemplate;
            m_delTemplate = delTemplate;
            m_clearTemplate = clearTemplate;
        }

        public abstract bool Add(out string newName);

        public abstract bool Edit(string name, out string newName);

        public virtual bool Del(string name)
        {
            return MessageBox.Show(string.Format(m_delTemplate, name), m_caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public virtual bool Clear()
        {
            return MessageBox.Show(m_clearTemplate, m_caption,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
