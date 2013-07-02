using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFControls.Components.DataManagers
{
	public partial class CheckedList : UserControl
	{
		public CheckedList()
		{
			InitializeComponent();
		}

		public string[] Values
		{
			get { return GetValues(clbItems.Items); }
			set
			{
				Do(() =>{
					clbItems.Items.Clear();
					clbItems.Items.AddRange(value);
				});
			}
		}

		public string[] CheckedValues
		{
			get { return GetValues(clbItems.CheckedItems); }
			set
			{
				Do(() =>{
					for (int i = 0; i < clbItems.Items.Count; i++)
					{
						string name = clbItems.Items[i].ToString();
						clbItems.SetItemChecked(i, value.Any(x => x == name));
					}
				});
			}
		}

		public int CheckedValuesCount{get { return clbItems.CheckedItems.Count; }}

		private static string[] GetValues(ICollection items)
		{
			var ret = new string[items.Count];
			int i = 0;
			foreach (var s in items)
			{
				ret[i++] = s.ToString();
			}
			return ret;
		}

		public event EventHandler OnSelectedIndexChanged;
		public event EventHandler OnCheckChanged;
		private bool m_enableChecking = true;
		private void clbItems_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!m_enableChecking)return;
			if ( OnSelectedIndexChanged != null){
				OnSelectedIndexChanged(sender, e);
			}
			if(clbItems.Items.Count == 0)
			{
				btnAll.Enabled = btnNone.Enabled = false;
			}
			else
			{
				if ( OnCheckChanged !=null){
					OnCheckChanged(sender, e);
				}
				if(clbItems.CheckedItems.Count==0)
				{
					btnNone.Enabled = false;
					btnAll.Enabled = true;
				}
				else
				{
					btnAll.Enabled = clbItems.CheckedItems.Count != clbItems.Items.Count;
					btnNone.Enabled = true;
				}
			}
			
		}

		public int SelectedIndex{
			get { return clbItems.SelectedIndex; }
			set { if(clbItems.Items.Count>0){
						clbItems.SelectedIndex=value;
					} }
		}

		public int[] CheckedIndexed{
			get{
				var list = new int[clbItems.CheckedItems.Count]; 
				for (int i = 0, j=0; i < clbItems.Items.Count; ++i){
					if(clbItems.GetItemChecked(i)){
						list[j] = i;
						++j;
					}
				}
				return list;
			}
		}

		private void btnAll_Click(object sender, EventArgs e)
		{
			SetCheck(true);
		}

		private void btNone_Click(object sender, EventArgs e)
		{
			SetCheck(false);
		}

		private void Do(Action a)
		{
			m_enableChecking = false;
			a();
			m_enableChecking = true;
			clbItems_SelectedIndexChanged(this, null);
		}

		public void SetCheck(bool value)
		{
			Do(() =>{
				for (int i = 0; i < clbItems.Items.Count; ++i)
				{
					clbItems.SetItemChecked(i, value);
				}
			});
		}
	}
}
