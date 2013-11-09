using PluginsShared.Ddos;
using PluginsShared.Ddos.Statistic;
using SqlRunner.Code.Settings;

namespace SqlRunner.Code
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
