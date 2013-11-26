using PluginsShared.Ddos;
using PluginsShared.Ddos.Statistic;
using Requestor.Code.Settings;

namespace Requestor.Code
{
	class Ddoser : Ddoser<Op>
	{
		protected override IOperationRunner CreateOperation(Op op, InfoCollector collector)
		{
			return new RequestMaker(op, op.Delay, collector);
		}
	}
}
