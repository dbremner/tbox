using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFControls.Components.Controls {
	public partial class Edit : UserControl {
		public Edit() {
			InitializeComponent();
		}

		public string Caption {
			get { return lCaption.Text; }
			set { lCaption.Text = value; }
		}

		public string Value {
			get { return tbEdit.Text; }
			set { tbEdit.Text = value; }
		}

		public event EventHandler ValueChanged;
		private void tbEdit_TextChanged( object sender, EventArgs e ) {
			if(ValueChanged!=null)ValueChanged( sender, e );
		}

	}
}
