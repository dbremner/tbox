using System;
using System.Windows.Controls;

namespace Mnk.Library.WpfControls.Code.Content
{
	public class TextBoxData : BaseData<TextBox>
	{
		private string value;
		public TextBoxData(TextBox box, string value):base(box)
		{
			Control.LostFocus += OnLostFocus;
			this.value = value;
		}

		private void OnLostFocus(object sender, EventArgs e)
		{
			DoChanged();
		}

		public override void Load()
		{
			Control.Text = value;
		}

		public override void Save()
		{
			value = Control.Text;
		}

		public override bool Changed { get { return value == Control.Text; } }
	}
}
