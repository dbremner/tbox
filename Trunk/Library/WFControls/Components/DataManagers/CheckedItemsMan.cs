using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common.Data;

namespace WFControls.Components.DataManagers
{

	public class CheckedListBoxItemsWorker : ListBoxItemsWorker
	{
		private readonly CheckedListBox m_owner;
		public CheckedListBoxItemsWorker(CheckedListBox owner)
			: base(owner)
		{
			m_owner = owner;
			m_owner.ItemCheck += OnItemCheck;
		}
		public event EventHandler<ItemCheckEventArgs> ItemCheck;
		private void OnItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (ItemCheck != null) ItemCheck(sender, e);
		}
		public void SetCheck(bool setting)
		{
			for (int i = 0; i < Count; ++i)
			{
				m_owner.SetItemChecked(i, setting);
			}
		}
		public void SetCheck(int id, bool setting)
		{
			m_owner.SetItemChecked(id, setting);
		}
		public void SetCheck(string name, bool setting)
		{
			SetCheck(m_owner.Items.IndexOf(name), setting);
		}
		public bool GetCheck(string name)
		{
			return m_owner.CheckedItems.IndexOf(name) != -1;
		}
	}

	public interface ICheckeable
	{
		bool Checked { get; set; }
	}
	public interface ICheckeableItems
	{
		void CheckItems(bool setting);
		void SetCheck(int id, bool setting);
		bool GetCheck(int id);
	}

	public class CheckedDataMan<T> : DataMan<T>, ICheckeableItems where T : ICheckeable, new()
	{
		public void CheckItems(bool setting)
		{
			foreach (Pair<string, T> t in MValues)
			{
				t.Value.Checked = setting;
			}
		}

		public void SetCheck(int id, bool setting)
		{
			MValues[id].Value.Checked = setting;
		}

		public bool GetCheck(int id)
		{
			return MValues[id].Value.Checked;
		}

		public Pair<string, T>[] Selected
		{
			get
			{
				return (from value in Values
						where value.Value.Checked
						select new Pair<string, T>(value.Key, value.Value)).ToArray();
			}
		}
	}

	public class CheckedItemsMan : ItemsMan
	{
		private readonly CheckedListBoxItemsWorker m_items;
		private ICheckeableItems m_checkableItems;
		readonly Button m_btnCheckAll;
		readonly Button m_btnCheckNone;

		public CheckedItemsMan(CheckedListBoxItemsWorker items, Button btnAdd, Button btnEdit, Button btnDel, Button btnClear, Button btnCheckAll, Button btnCheckNone) :
			base(items, btnAdd, btnEdit, btnDel, btnClear)
		{
			m_items = items;
			InitButton(ref this.m_btnCheckAll, btnCheckAll, OnCheckAll);
			InitButton(ref this.m_btnCheckNone, btnCheckNone, OnCheckNone);
			m_items.ItemCheck += OnItemCheck;
			UpdateInterface();
		}

		protected override void UpdateInterface()
		{
			base.UpdateInterface();
			if (m_items != null)
			{
				EnableButton(m_btnCheckAll, m_items.Count > 0);
				EnableButton(m_btnCheckNone, m_items.Count > 0);
			}
		}

		public override IDataMan DataMan
		{
			set
			{
				dataMan = value;
				m_checkableItems = (ICheckeableItems)dataMan;
				m_items.Clear();
				if (dataMan != null)
				{
					SetValues(dataMan.GetValues());
					for (int i = 0; i < m_items.Count; ++i)
					{
						m_items.SetCheck(i, m_checkableItems.GetCheck(i));
					}
				}
				UpdateInterface();
				OnSelectedIndexChanged(null, null);
			}
		}
		protected override void AfterAddInit(int id)
		{
			m_items.SetCheck(id, m_checkableItems.GetCheck(id));
		}

		public void SetCheck(string name, bool setting)
		{
			m_items.SetCheck(name, setting);
		}
		public bool GetCheck(string name)
		{
			return m_items.GetCheck(name);
		}

		private void OnCheckAll(object sender, EventArgs e)
		{
			m_items.SetCheck(true);
			m_checkableItems.CheckItems(true);
		}
		private void OnCheckNone(object sender, EventArgs e)
		{
			m_items.SetCheck(false);
			m_checkableItems.CheckItems(false);
		}

		private void OnItemCheck(object sender, ItemCheckEventArgs e)
		{
			m_checkableItems.SetCheck(e.Index, e.NewValue == CheckState.Checked);
			UpdateInterface();
		}

	}
}
