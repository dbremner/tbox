﻿using System.Windows.Controls;
using Mnk.TBox.Core.Interface;

namespace Mnk.TBox.Plugins.LocalizationTool
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ISettings
	{
		public Settings()
		{
			InitializeComponent();
		}

		public UserControl Control { get { return this; } }
	}
}
