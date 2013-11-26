using System;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace AvailabilityChecker.Code
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
			(Items = new CheckableDataCollection<CheckableData>())
				.FillCollection("http://tbox.codeplex.com");
		}
	}
}
