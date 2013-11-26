﻿using System;
using System.Collections.ObjectModel;
using Common.UI.ModelsContainers;

namespace ServicesCommander.Code
{
	[Serializable]
	public class Config
	{
		public string SelectedProfile { get; set; }
		public ObservableCollection<Profile> Profiles { get; set; }

		public Config()
		{
			SelectedProfile = "Sample";
			Profiles = new ObservableCollection<Profile>
				{
					new Profile
						{
							Key = SelectedProfile,
							Services = new CheckableDataCollection<ServiceInfo>
								{
									new ServiceInfo
										{
											Key = "Sample service",
											IsChecked = false
										}
								}
						}
				};
		}
	}
}
