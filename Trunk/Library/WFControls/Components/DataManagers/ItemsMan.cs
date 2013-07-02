using System;
using System.Windows.Forms;
using WFControls.Components.Dialogs;

namespace WFControls.Components.DataManagers
{
    public class ItemsMan
    {
        private readonly IItemsWorker m_items;
        protected IDataMan dataMan = null;
        private Base formDialogs = null;

        readonly Button m_btnAdd;
        readonly Button m_btnEdit;
        readonly Button m_btnDel;
        readonly Button m_btnClear;

        protected static void InitButton(ref Button thisBtn, Button target, EventHandler handler)
        {
            thisBtn = target;
            if (thisBtn != null) thisBtn.Click += handler;
        }

        public ItemsMan(IItemsWorker items, Button btnAdd, Button btnEdit, Button btnDel, Button btnClear)
        {
            m_items = items;
            InitButton(ref this.m_btnAdd, btnAdd, OnAdd);
            InitButton(ref this.m_btnEdit, btnEdit, OnEdit);
            InitButton(ref this.m_btnDel, btnDel, OnDel);
            InitButton(ref this.m_btnClear, btnClear, OnClear);
            m_items.SelectionChanged += OnSelectedIndexChanged;
            UpdateInterface();
        }

        protected static void EnableButton(Button btn, bool setting)
        {
            if (btn != null) btn.Enabled = setting;
        }

        protected virtual void UpdateInterface()
        {
            EnableButton(m_btnClear, m_items.Count > 1);
            EnableButton(m_btnEdit, m_items.Selected != -1);
            EnableButton(m_btnDel, m_items.Count > 0 && m_btnEdit.Enabled);
        }

        public event EventHandler SelectionChanged;

        protected void SetValues(string[] value)
        {
            int id = m_items.Selected;
            m_items.Clear();
            if (value != null)
            {
                m_items.AddRange(value);
                if (m_items.Count > 0)
                {
                    m_items.Selected = Math.Max(id, m_items.Count - 1);
                }
            }
        }

        public virtual IDataMan DataMan
        {
            get { return dataMan; }
            set
            {
                dataMan = value;
                m_items.Clear();
                if (dataMan != null)
                {
                    SetValues(dataMan.GetValues());
                }
                UpdateInterface();
                OnSelectedIndexChanged(null, null);
            }
        }

        public Base FormDialogs
        {
            get { return formDialogs; }
            set
            {
                formDialogs = value;
                UpdateInterface();
            }
        }

        public bool IsValueExist(string name)
        {
            for (int i = 0; i < m_items.Count; ++i)
            {
                if (string.Compare(m_items[i], name, true) == 0) return true;
            }
            return false;
        }

        public bool IsValueIncluded(string name)
        {
            name = name.ToLower();
            for (int i = 0; i < m_items.Count; ++i)
            {
                string value = m_items[i].ToLower();
                if (value.StartsWith(name) || name.StartsWith(value))
                    return true;
            }
            return false;
        }

        public bool Empty { get { return m_items.Count == 0; } }
        public int Selected { get { return m_items.Selected; } set { m_items.Selected = value; } }
        public string SelectedText { get { return m_items.SelectedText; } }

        protected virtual void AfterAddInit(int id) { }

        private void OnAdd(object sender, EventArgs e)
        {
            string newValue;
            if (formDialogs.Add(out newValue))
            {
                int id = m_items.Add(newValue);
                if (dataMan != null)
                    dataMan.Add(newValue);
                AfterAddInit(id);
                m_items.Selected = id;
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            if (m_items.Selected != -1)
            {
                int id = m_items.Selected;
                string newValue;
                if (formDialogs.Edit(m_items[id], out newValue))
                {
                    m_items[id] = newValue;
                    if (dataMan != null)
                        dataMan.Change(id, newValue);
                    UpdateInterface();
                }
            }
        }

        private void OnDel(object sender, EventArgs e)
        {
            if (formDialogs.Del(m_items[m_items.Selected].ToString()))
            {
                int id = m_items.Selected;
                m_items.RemoveAt(id);
                if (dataMan != null)
                    dataMan.Del(id);
                if (!Empty)
                {
                    m_items.Selected = Math.Min(id, m_items.Count - 1);
                }
                else
                {
                    OnSelectedIndexChanged(sender, e);
                }
            }
        }

        private void OnClear(object sender, EventArgs e)
        {
            if (formDialogs.Clear())
            {
                m_items.Clear();
                if (dataMan != null)
                    dataMan.Clear();
                OnSelectedIndexChanged(sender, e);
            }
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInterface();
            if (SelectionChanged != null)
            {
                SelectionChanged(this, null);
            }
        }
    }
}
