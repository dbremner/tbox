using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace Mnk.Library.Common.PerfCounters
{
	public sealed class CounterManager
	{
		private readonly object locker = new object();
		private readonly string categoryName;
		private readonly Dictionary<string, PerformanceCounter> counters = new Dictionary<string, PerformanceCounter>();
		private readonly Dictionary<string, long> countersValues = new Dictionary<string, long>();
		private readonly Timer timer;

		public CounterManager(string categoryName, int refreshTime = 1000)
		{
			this.categoryName = categoryName;
			timer = new Timer
			{
				Interval = refreshTime,
				Enabled = true,
			};
			timer.Elapsed += UpdateCounters;
		}

		public void Add(string name)
		{
			lock (locker)
			{
				counters[name] = new PerformanceCounter(categoryName, name, false);
			}
		}

		public void Set(string name, long value)
		{
			lock (locker)
			{
				if (countersValues.ContainsKey(name))
				{
					var old = countersValues[name];
					if (old >= value)
					{
						return;
					}
				}
				countersValues[name] = value;
			}
		}

		private void UpdateCounters(object sender, ElapsedEventArgs e)
		{
			lock (locker)
			{
				foreach (var x in countersValues)
				{
					counters[x.Key].RawValue = x.Value;
				}
				countersValues.Clear();
			}
		}
	}
}
