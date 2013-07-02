using System.Threading;
using PluginsShared.Ddos;
using PluginsShared.Ddos.Statistic;
using SqlRunner.Code.Settings;

namespace SqlRunner.Code
{
	class RequestMaker : BaseExecutor, IOperationRunner
	{
		private readonly string connectionString;
		private readonly Op operation;
		private readonly int delay;
		private bool mustTeminateThread = false;
		private readonly InfoCollector collector;

		public RequestMaker(string connectionString, Op operation, int delay, InfoCollector collector)
		{
			this.connectionString = connectionString;
			this.operation = operation;
			this.delay = delay*1000;
			this.collector = collector;
		}

		public DatabaseInfo GetResult()
		{
			return GetResult(connectionString, operation);
		}

		public void Work()
		{
			while (!mustTeminateThread)
			{
				collector.Push(GetResult());
				if (delay>0) Thread.Sleep(delay);
			}
		}

		public void Terminate()
		{
			mustTeminateThread = true;
		}
	}
}
