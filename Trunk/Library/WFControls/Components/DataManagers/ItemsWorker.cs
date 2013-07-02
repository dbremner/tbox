using System;
using System.Windows.Forms;

namespace WFControls.Components.DataManagers
{
    public interface IItemsWorker
    {
        int Count
        {
            get;
        }
        string this[int id]
        {
            get;
            set;
        }
        int Selected
        {
            get;
            set;
        }
        string SelectedText
        {
            get;
        }
        void AddRange(string[] values);
        int Add(string value);
        void RemoveAt(int id);
        void Clear();
        event EventHandler SelectionChanged;
    }

    public class ComboBoxItemsWorker : IItemsWorker
    {
        private readonly ComboBox m_owner;
        public ComboBoxItemsWorker(ComboBox owner)
        {
            m_owner = owner;
            m_owner.SelectedIndexChanged += OnSelectedIndexChanged;
        }
        public event EventHandler SelectionChanged;
        public void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null) SelectionChanged(sender, e);
        }
        public int Count
        {
            get { return m_owner.Items.Count; }
        }
        public string this[int id]
        {
            get { return m_owner.Items[id].ToString(); }
            set { m_owner.Items[id] = value; }
        }
        public int Selected
        {
            get { return m_owner.SelectedIndex; }
            set { m_owner.SelectedIndex = value; }
        }
        public string SelectedText
        {
            get { return m_owner.SelectedItem.ToString(); }
        }
        public void AddRange(string[] values)
        {
            m_owner.Items.AddRange(values);
        }
        public int Add(string value)
        {
            return m_owner.Items.Add(value);
        }
        public void RemoveAt(int id)
        {
            m_owner.Items.RemoveAt(id);
        }
        public void Clear()
        {
            m_owner.Items.Clear();
        }
    }

    public class ListBoxItemsWorker : IItemsWorker
    {
        private readonly ListBox m_owner;
        public ListBoxItemsWorker(ListBox owner)
        {
            m_owner = owner;
            m_owner.SelectedIndexChanged += OnSelectedIndexChanged;
        }
        public event EventHandler SelectionChanged;
        public void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null) SelectionChanged(sender, e);
        }
        public int Count
        {
            get { return m_owner.Items.Count; }
        }
        public string this[int id]
        {
            get { return m_owner.Items[id].ToString(); }
            set { m_owner.Items[id] = value; }
        }
        public int Selected
        {
            get { return m_owner.SelectedIndex; }
            set { m_owner.SelectedIndex = value; }
        }
        public string SelectedText
        {
            get { return m_owner.SelectedItem.ToString(); }
        }
        public void AddRange(string[] values)
        {
            m_owner.Items.AddRange(values);
        }
        public int Add(string value)
        {
            return m_owner.Items.Add(value);
        }
        public void RemoveAt(int id)
        {
            m_owner.Items.RemoveAt(id);
        }
        public void Clear()
        {
            m_owner.Items.Clear();
        }
    }
}
