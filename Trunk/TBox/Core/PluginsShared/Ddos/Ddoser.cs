using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PluginsShared.Ddos.Settings;
using PluginsShared.Ddos.Statistic;

namespace PluginsShared.Ddos
{
	public abstract class Ddoser<T> : IDdoser
		where T : IOperation
	{
		private readonly object locker = new object();
		private IList<IOperationRunner> requests;
		private Thread thread;

		public bool IsWorks 
		{
			get
			{
				return thread!=null && thread.IsAlive;
			}
		}

		public void Start(IEnumerable<IOperation> ops, Analizer analizer)
		{
			lock (locker)
			{
				var operations = ops.Cast<T>().ToArray();
				var threadsCount = operations.Sum(item => item.Threads);
				if (threadsCount == 0)
				{
					return;
				}
				var collectors = new List<InfoCollector>();
				var threads = CreateRequests(threadsCount, operations, collectors);
				thread = new Thread(() => Work(threads, analizer, collectors));
				thread.Start();
			}
		}

		private static void Work(Thread[] threads, Analizer analizer, IList<InfoCollector> collectors)
		{
			do
			{
				Thread.Sleep(1000);
				Analize(analizer, collectors);
			} while (CheckAnyThreadRun(threads));
			Analize(analizer, collectors);
		}

		private static void Analize(Analizer analizer, IEnumerable<InfoCollector> collectors)
		{
			analizer.Calc(collectors.ToDictionary(x=>x.Key, x=>x.Pop()));
		}

		private static bool CheckAnyThreadRun(IEnumerable<Thread> threads)
		{
			return threads.Any(t => t != null && t.ThreadState != ThreadState.Stopped);
		}

		private Thread[] CreateRequests(int threadsCount, IEnumerable<T> operations, IList<InfoCollector> collectors)
		{
			requests = new IOperationRunner[threadsCount];
			var threads = new Thread[threadsCount];
			var reqNo = 0;
			foreach (var item in operations)
			{
				var collector = new InfoCollector(item.Key);
				collectors.Add(collector);
				for (var i = 0; i < item.Threads; i++)
				{
					var r = requests[reqNo] = CreateOperation(item, collector);
					threads[reqNo] = new Thread(r.Work);
					threads[reqNo].Start();
					++reqNo;
				}
			}
			return threads;
		}

		protected abstract IOperationRunner CreateOperation(T op, InfoCollector collector);

		private void Terminate()
		{
			foreach (var r in requests.Where(r => r != null))
			{
				r.Terminate();
			}
		}

		public void Stop()
		{
			lock (locker)
			{
				if ( thread == null || !thread.IsAlive) return;
				Terminate();
				while (thread.IsAlive) Thread.Sleep(100);
			}
		}
	}
}
