using System.Collections.Generic;
using PluginsShared.Ddos.Settings;
using PluginsShared.Ddos.Statistic;

namespace PluginsShared.Ddos
{
	public interface IDdoser
	{
		void Start(IEnumerable<IOperation> operations, Analizer analizer);
		void Stop();
		bool IsWorks { get; }
	}
}
