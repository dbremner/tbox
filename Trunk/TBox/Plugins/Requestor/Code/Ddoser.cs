using Mnk.TBox.Core.PluginsShared.Ddos;
using Mnk.TBox.Core.PluginsShared.Ddos.Statistic;
using Mnk.TBox.Plugins.Requestor.Code.Settings;

namespace Mnk.TBox.Plugins.Requestor.Code
{
	class Ddoser : Ddoser<Op>
	{
		protected override IOperationRunner CreateOperation(Op op, InfoCollector collector)
		{
			return new RequestMaker(op, op.Delay, collector);
		}
	}
}
