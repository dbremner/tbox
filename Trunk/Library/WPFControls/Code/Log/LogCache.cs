using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace Mnk.Library.WpfControls.Code.Log
{
	public class LogCache : ICaptionedLog
	{
		private readonly DispatcherTimer timer = new DispatcherTimer();
		private readonly ICaptionedLog log;
		private readonly Action<string,string> onChanged;
		private IList<KeyValuePair<string, string>> data;
		public LogCache(ICaptionedLog log, Action<string, string> onChanged)
		{
			this.log = log;
			this.onChanged = onChanged;
			data = new List<KeyValuePair<string, string>>();
			timer.Tick += TimerOnTick;
			timer.Interval = new TimeSpan(0,0,0,1);
		}

		private void TimerOnTick(object sender, EventArgs eventArgs)
		{
			var list = data;
			lock (data)
			{
				if(list.Count == 0)return;
				data = new List<KeyValuePair<string, string>>();
			}
			foreach (var pair in list)
			{
				log.Write(pair.Key, pair.Value);
			}
			var last = list.Last();
			onChanged(last.Key, last.Value);
		}

		public void Write(string caption, string value)
		{
			lock (data)
			{
				data.Add(new KeyValuePair<string, string>(caption, value));
			}
		}

        public void Refresh()
        {
            TimerOnTick(null, null);
        }

		public void Start()
		{
			timer.Start();
		}

		public void Stop()
		{
			timer.Stop();
		}
	}
}
