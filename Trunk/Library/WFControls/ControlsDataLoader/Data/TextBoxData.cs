using System;
using System.Windows.Forms;

namespace WFControls.ControlsDataLoader.Data
{
	public class TextBoxData : BaseData<TextBox>
	{
		private string m_value;
		public TextBoxData(TextBox box, string value):base(box)
		{
			m_ctrl.LostFocus += OnLostFocus;
			m_value = value;
		}

		private void OnLostFocus(object sender, EventArgs e)
		{
			DoChanged();
		}

		public override void Load()
		{
			m_ctrl.Text = m_value;
		}

		public override void Save()
		{
			m_value = m_ctrl.Text;
		}

		public override bool Changed { get { return m_value == m_ctrl.Text; } }
	}
}
