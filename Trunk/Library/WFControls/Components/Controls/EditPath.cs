using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WFControls.Components.Controls
{
	public partial class EditPath : Edit
	{
		public EditPath()
		{
			InitializeComponent();
		}

		public IPathGetter PathGetter { get; set; }

		private void btnEdit_Click(object sender, EventArgs e)
		{
			var path = Value;
			if(PathGetter.Get(ref path))
			{
				Value = path;
			}
		}
	}
}
