﻿using System.Globalization;
using System.Threading;
using Common.Network;
using PluginsShared.Ddos;
using PluginsShared.Ddos.Statistic;
using Requestor.Code.Settings;

namespace Requestor.Code
{
	class RequestMaker : Request, IOperationRunner
	{
		private readonly Op op;
		private readonly int delay;
		private bool mustTeminateThread = false;
		private readonly InfoCollector collector;

		public RequestMaker(Op op, int delay, InfoCollector collector)
		{
			this.op = op;
			this.delay = delay*1000;
			this.collector = collector;
		}

		public StatisticInfo GetResult()
		{
			var r = GetResult(op.Request.Url, 
								op.Request.Method, 
								op.Request.Body, 
								op.Request.Headers,
								op.Timeout*1000);
			return new StatisticInfo{Time = r.Time, Status = ((int)r.HttpStatusCode).ToString(CultureInfo.InvariantCulture)};
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
