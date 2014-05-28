using System.Collections.Generic;
using System.Linq;

namespace Mnk.TBox.Core.PluginsShared.LoadTesting.Statistic
{
	public sealed class Analyzer
	{
		private readonly object locker = new object();
		private readonly IDictionary<string, AnalizeInfo> statistics;
		public IEnumerable<string> Keys{get { return statistics.Keys; }}

		public Analyzer(IEnumerable<string> names)
		{
			statistics = new Dictionary<string, AnalizeInfo>(new[] { string.Empty }.Concat(names).ToDictionary(x => x, x => new AnalizeInfo()));
		}

		public void Calc(IDictionary<string, IEnumerable<StatisticInfo>> infos)
		{
			lock (locker)
			{
				var s = CalcGlobal(
					infos.Select(info => CalForOperation(info.Key, info.Value)));
				AddStatistic(statistics[string.Empty], s);
			}
		}

		private OperationStatistic CalForOperation(string key, IEnumerable<StatisticInfo> infos)
		{
			var s = new OperationStatistic{Key = key};
			foreach (var i in infos)
			{
				++s.Count;
				if (i.Time > s.MaxTime) s.MaxTime = i.Time;
				if (i.Time < s.MinTime) s.MinTime = i.Time;
				s.Time += i.Time;
				AddStatusCode(s.Statutes, i.Status, 1);
			}
			AddStatistic(statistics[key], s);
			return s;
		}

		private static OperationStatistic CalcGlobal(IEnumerable<OperationStatistic> list)
		{
			var s = new OperationStatistic();
			foreach (var i in list)
			{
				UpdateStatistic(s, i);
			}
			return s;
		}

		private static void UpdateStatistic(OperationStatistic collector, OperationStatistic newItem)
		{
			collector.Count += newItem.Count;
			if (newItem.MaxTime > collector.MaxTime) collector.MaxTime = newItem.MaxTime;
			if (newItem.MinTime < collector.MinTime) collector.MinTime = newItem.MinTime;
			collector.Time += newItem.Time;
			foreach (var status in newItem.Statutes)
			{
				AddStatusCode(collector.Statutes, status.Key, status.Value);
			}
		}

		private static void AddStatusCode(IDictionary<string,int> dict, string code, int count)
		{
			if (dict.ContainsKey(code))
			{
				dict[code] += count;
			}
			else
			{
				dict.Add(code, count);
			}
		}

		private static void AddStatistic(AnalizeInfo info, OperationStatistic s)
		{
			info.Values.Add(s);
			info.Graphics.Draw(s);
			UpdateStatistic(info.Result, s);
		}

		public IEnumerable<OperationStatistic> GetValues(string name)
		{
			lock (locker)
			{
				return statistics[name].Values;
			}
		}

		public OperationStatistic GetStatistic(string name)
		{
			lock(locker)
			{
				return statistics[name].Result.Clone();
			}
		}

		public GraphicsInfo GetGraphic(string name)
		{
			lock (locker)
			{
				return statistics[name].Graphics;
			}
		}
	}
}
