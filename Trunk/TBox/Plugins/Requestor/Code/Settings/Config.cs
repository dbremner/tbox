﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mnk.Library.Common.Network;
using Mnk.Library.Common.UI.ModelsContainers;
using Mnk.TBox.Core.Interface;
using Mnk.Library.WPFControls.Dialogs.StateSaver;

namespace Mnk.TBox.Plugins.Requestor.Code.Settings
{
	[Serializable]
	public sealed class Config : IConfigWithDialogStates
	{
		public string SelectedProfile { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }
		public IDictionary<string, DialogState> States { get; set; }

		public Config()
		{
			SelectedProfile = "Sample";
			Profiles = new ObservableCollection<Profile>
				{
					new Profile
						{
							Key = "Sample",
							Ops = new CheckableDataCollection<Op>
								{
									new Op
										{
											Key = "Sample request",
											Request = new RequestConfig
												{
													Method = HttpMethod.GET,
													Url = "http://tbox.codeplex.com",
													Headers = new ObservableCollection<Header>
														{
															new Header
																{
																	Key = "Accept-Encoding",
																	Value = "gzip, deflate"
																}
														}
												}
										}
								}
						}
				};
			States = new Dictionary<string, DialogState>();
		}

	}
}
