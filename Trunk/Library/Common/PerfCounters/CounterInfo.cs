using System.Diagnostics;

namespace Common.PerfCounters
{
	public class CounterInfo
	{
		public string Name { get; set; }
		public string Help { get; set; }
		public PerformanceCounterType CounterType { get; set; }
	}
}
