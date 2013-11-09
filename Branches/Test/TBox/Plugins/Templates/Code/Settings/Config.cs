using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.UI.Model;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace Templates.Code.Settings
{
	[Serializable]
	public class Config : IConfigWithDialogStates
	{
		public string ItemTemplate { get; set; }
		public ObservableCollection<PairData> KnownValues { get; set; }
		public ObservableCollection<Template> StringTemplates { get; set; }
		public ObservableCollection<Template> FileTemplates { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			ItemTemplate = "{#item#}";
			KnownValues = new ObservableCollection<PairData>
				{
					new PairData{Key = "SampleKey", Value = "SampleValue"}
				};
			StringTemplates = new ObservableCollection<Template>
				{
					new Template
						{
							Key = "nunit",
							Value = "[Test]\npublic void Should_{#name#}()\n{\n//Arrange\n\n//Act\n\n//Assert\n}"
						}
				};
			FileTemplates = new ObservableCollection<Template>();
			States = new Dictionary<string, DialogState>();
		}

	}
}
