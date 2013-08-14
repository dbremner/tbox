using System;
using Common.AutoUpdate;

namespace TBox.Code.AutoUpdate
{
	[Serializable]
	public class Update
	{
		public UpdateInterval Interval { get; set; }
		public string Directory { get; set; }
		public DateTime Last { get; set; }
		public bool ShowChanglog { get; set; }
		public long LastChanglogPosition { get; set; }

		public Update()
		{
			Interval = UpdateInterval.Startup;
			Directory = @"\\tboxserver\Updates\TBox\";
			ShowChanglog = true;
			LastChanglogPosition = 0;
		}
	}
}
