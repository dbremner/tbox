using System;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace AvailabilityChecker
{
	[Serializable]
	public class Config
	{
		public int CheckInterval { get; set; }
		public bool Started { get; set; }
		public CheckableDataCollection<CheckableData> Items { get; set; }

		public Config()
		{
			CheckInterval = 60;
			Started = false;
			Items = new CheckableDataCollection<CheckableData>();
		}
	}
}
