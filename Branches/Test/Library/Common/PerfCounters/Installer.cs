using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Common.PerfCounters
{
	public class Installer
	{
		private readonly string categoryName;
		public Installer(string categoryName)
		{
			this.categoryName = categoryName;
		}

		public void Delete()
		{
			if (PerformanceCounterCategory.Exists(categoryName))
			{
				PerformanceCounterCategory.Delete(categoryName);
			}
		}

		public void Create(string categoryHelp, IList<CounterInfo> countersToAdd)
		{
			if (categoryName == null || countersToAdd == null || countersToAdd.Count() == 0) return;
			if (PerformanceCounterCategory.Exists(categoryName)) return;
			var counters = new CounterCreationDataCollection();
			foreach (var counter in countersToAdd)
			{
				counters.Add(CreateCounter(counter.Name, counter.Help, counter.CounterType));
			}
			PerformanceCounterCategory.Create(categoryName, categoryHelp,
			                                  PerformanceCounterCategoryType.SingleInstance, counters);
		}

		private static CounterCreationData CreateCounter(string counterName, string counterHelp, PerformanceCounterType counterType)
		{
			return new CounterCreationData()
			{
				CounterName = counterName,
				CounterHelp = counterHelp,
				CounterType = counterType,
			};
		}
	}
}