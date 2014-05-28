using System;
using System.Globalization;

namespace Mnk.Library.WpfControls.Dialogs.PerfomanceCounters
{
	[Serializable]
	public class SelectedEntity
	{
		public string Category { get; set; }
		public string Name { get; set; }
		public string Instance { get; set; }

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}\t{1}\t{2}", Category, Name, Instance);
		}
	}
}
