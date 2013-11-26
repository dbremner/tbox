using System;

namespace WPFControls.Dialogs.PerfomanceCounters
{
	[Serializable]
	public class SelectedEntity
	{
		public string Category { get; set; }
		public string Name { get; set; }
		public string Instance { get; set; }

		public override string ToString()
		{
			return string.Format("{0}\t{1}\t{2}", Category, Name, Instance);
		}
	}
}
