using System.Collections.Generic;
using System.Linq;

namespace PluginsShared.Ddos.Statistic
{
	public sealed class InfoCollector
	{
		public string Key { get; private set; }
		private readonly IList<StatisticInfo> infoList = new List<StatisticInfo>();

		public InfoCollector(string key)
		{
			Key = key;
		}

		public void Push(StatisticInfo statisticInfo)
		{
			lock (infoList)
			{
				infoList.Add(statisticInfo);
			}
		}

		public IEnumerable<StatisticInfo> Pop()
		{
			lock (infoList)
			{
				var arr = infoList.ToArray();
				infoList.Clear();
				return arr;
			}
		}
	}
}
