using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Common.Tools;
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
			(FileNames = new CheckableDataCollection<CheckableData>())
				.FillCollection(Path.GetTempPath());
			(FileTypes = new CheckableDataCollection<CheckableData>())
				.FillCollection("cs", "cpp", "h", "hpp", "xaml", "csproj", 
					"config", "js", "css", "html", "htm", "ascx", "aspx");
			SkipComments = true;
			DecodeStrings = true;
			DecodeComments = true;
			(FileMasksToExclude = new ObservableCollection<Data>())
                .FillCollection("*.min.js", "*.min.css", "*\\bin\\*");
		}
	}

}
