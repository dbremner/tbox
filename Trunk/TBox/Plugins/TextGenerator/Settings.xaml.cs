﻿using System.Windows.Controls;
using Interface;

namespace TextGenerator
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public sealed partial class Settings : ISettings
	{
		public Settings()
		{
			InitializeComponent();
		}

		public UserControl Control{get { return this; }}
	}
}
