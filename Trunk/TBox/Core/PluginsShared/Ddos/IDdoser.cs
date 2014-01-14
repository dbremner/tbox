using System.Collections.Generic;
using Mnk.TBox.Core.PluginsShared.Ddos.Settings;
using Mnk.TBox.Core.PluginsShared.Ddos.Statistic;

namespace Mnk.TBox.Core.PluginsShared.Ddos
{
	public interface IDdoser
	{
		void Start(IEnumerable<IOperation> operations, Analizer analizer);
		void Stop();
		bool IsWorks { get; }
	}
}
