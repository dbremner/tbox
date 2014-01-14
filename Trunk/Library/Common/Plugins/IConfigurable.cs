using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mnk.Library.Common.Plugins
{
	public interface IConfigurable
	{
		Type ConfigType { get; }
		object ConfigObject { get; set; }
	}
}
