using System.Collections.Generic;

namespace Mnk.TBox.Core.PluginsShared.Ddos.Statistic
{
	class AnalizeInfo
	{
		public IList<OperationStatistic> Values { get; set; }
		public GraphicsInfo Graphics { get; set; }
		public OperationStatistic Result { get; set; }  

		public AnalizeInfo()
		{
			Values = new List<OperationStatistic>();
			Graphics = new GraphicsInfo();
			Result = new OperationStatistic();
		}
	}
}
