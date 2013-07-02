using System.Collections.Generic;

namespace PluginsShared.Ddos.Settings
{
	public interface IProfile
	{
		string Key { get; }
		IEnumerable<IOperation> GetOperations();
	}
}
