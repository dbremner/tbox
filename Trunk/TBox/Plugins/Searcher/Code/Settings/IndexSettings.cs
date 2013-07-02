using System;
using System.Collections.ObjectModel;
using Common.UI.Model;
using Common.UI.ModelsContainers;

namespace Searcher.Code.Settings
{
	[Serializable]
	public sealed class IndexSettings : Data
	{
		public CheckableDataCollection<CheckableData> FileNames { get; set; }
		public CheckableDataCollection<CheckableData> FileTypes { get; set; }
		public bool SkipComments { get; set; }
		public bool DecodeStrings { get; set; }
		public bool DecodeComments { get; set; }
		public ObservableCollection<Data> FileMasksToExclude { get; set; }

		public IndexSettings()
		{
			FileNames = new CheckableDataCollection<CheckableData>();
			FileTypes = new CheckableDataCollection<CheckableData>();
			SkipComments = true;
			DecodeStrings = true;
			DecodeComments = true;
			FileMasksToExclude = new ObservableCollection<Data>();
		}
	}

}
