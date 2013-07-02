using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFControls.Components.Controls {
	public partial class Combo : UserControl {
		public Combo() {
			InitializeComponent();
		}

		public string Caption {
			get { return lCaption.Text; }
			set { lCaption.Text = value; }
		}

		public string[] Values {
			get{
				if ( cbData.Items.Count == 0 ) return null;
				var ret = new string[cbData.Items.Count];
				for ( int i = 0; i < cbData.Items.Count; ++i ) {
					ret[i] = cbData.Items[i].ToString();
				}
				return ret;
			}
			set{
				cbData.Items.Clear();
				if ( value != null ) cbData.Items.AddRange( value );
			}
		}

		public string Value {
			get { return cbData.Text; }
			set { cbData.Text = value; }
		}

		public ComboBoxStyle DropDownStyle{
			get { return cbData.DropDownStyle; }
			set { cbData.DropDownStyle = value; }
		}

		public event EventHandler ValueChanged;
		private void cbData_TextChanged( object sender, EventArgs e ){
			if ( ValueChanged != null) ValueChanged( sender, e );
		}

		public event EventHandler SelectedIndexChanged;
		private void cbData_SelectedIndexChanged( object sender, EventArgs e ) {
			if ( SelectedIndexChanged != null ) SelectedIndexChanged(sender, e);
		}
	}
}
