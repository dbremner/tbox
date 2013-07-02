using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WFControls.Components.Controls
{
	public interface IPathGetter
	{
		bool Get(ref string path);
	}
}
