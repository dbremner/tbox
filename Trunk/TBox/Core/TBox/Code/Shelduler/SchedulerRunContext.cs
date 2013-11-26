using System;
using TBox.Code.Shelduler.Settings;

namespace TBox.Code.Shelduler
{
	class SchedulerRunContext
	{
		public DateTime StartTime { get; set; }
		public SchedulerTask[] Tasks { get; set; }
	}
}
