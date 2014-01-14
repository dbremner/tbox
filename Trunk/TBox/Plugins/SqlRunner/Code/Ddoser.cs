using Mnk.TBox.Core.PluginsShared.Ddos;
using Mnk.TBox.Core.PluginsShared.Ddos.Statistic;
using Mnk.TBox.Plugins.SqlRunner.Code.Settings;

namespace Mnk.TBox.Plugins.SqlRunner.Code
{
	class Ddoser : Ddoser<Op>
	{
		private Config config;
		public void OnConfigUpdated(Config cfg)
		{
			config = cfg;
		}

		protected override IOperationRunner CreateOperation(Op op, InfoCollector collector)
		{
			return new RequestMaker(config.ConnectionString, op, op.Delay, collector);
		}
	}
}
