using System.Collections.Generic;

namespace Mnk.TBox.Core.PluginsShared.Ddos.Settings
{
	public interface IProfile
	{
		string Key { get; }
		IEnumerable<IOperation> GetOperations();
	}
}
