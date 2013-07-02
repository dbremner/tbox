using System;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Common.MT
{
	public sealed class SafeTimer : IDisposable
	{
		private readonly Timer timer;
		private volatile bool operationInProgress = false;
		public SafeTimer(Action action)
		{
			timer = new Timer { AutoReset = false, Enabled = false };
			timer.Elapsed += (o, e) =>
			{
				operationInProgress = true;
				action();
				if (Enabled)timer.Start();
				operationInProgress = false;
			};
		}

		public void Start(int interval)
		{
			lock (timer)
			{
				WailtOperaionEnd();
				Enabled = true;
				timer.Interval = interval;
				timer.Start();
			}
		}

		private void WailtOperaionEnd()
		{
			Enabled = false;
			while (operationInProgress) Thread.Sleep(10);
		}

		public void Stop()
		{
			lock (timer)
			{
				Enabled = false;
			}
		}

		public bool Enabled { get; private set; }

		public void Dispose()
		{
			lock (timer)
			{
				WailtOperaionEnd();
				timer.Stop();
				timer.Dispose();
			}
		}
	}
}
