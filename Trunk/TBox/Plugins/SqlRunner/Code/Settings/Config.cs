using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Tools;
using Common.UI.Model;
using Common.UI.ModelsContainers;
using Interface;
using WPFControls.Dialogs.StateSaver;

namespace SqlRunner.Code.Settings
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public string SelectedProfile { get; set; }
		public string ConnectionString { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }
		public CheckableDataCollection<CheckableData> ConnectionStrings { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			SelectedProfile = "Sample";
			Profiles = new ObservableCollection<Profile>
				{
					new Profile
						{
							Key = SelectedProfile,
							Ops = new CheckableDataCollection<Op>
								{
									new Op
										{
											Key = "test command",
											Command = "select * from sampleTable",
										}
								}
						}
				};
			ConnectionString = "Data Source=(local);Initial Catalog=DB";
			(ConnectionStrings = new CheckableDataCollection<CheckableData>())
				.FillCollection(ConnectionString);
			States = new Dictionary<string, DialogState>();
		}

	}
}
