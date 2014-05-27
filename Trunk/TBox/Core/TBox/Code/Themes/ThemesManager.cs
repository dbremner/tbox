﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using Mnk.Library.Common;
using Mnk.Library.Common.Log;
using Mnk.Library.Common.Tools;
using Mnk.TBox.Locales.Localization.TBox;

namespace Mnk.TBox.Core.Application.Code.Themes
{
	class ThemesManager
	{
		private string lastLoaded = string.Empty;
		private static readonly ILog Log = LogManager.GetLogger<ThemesManager>();
		private static readonly string ThemesPath = Path.Combine(Environment.CurrentDirectory, "Themes");
		public ThemesManager()
		{
			AvailableThemes = new List<string>();
			FillThemesList();
		}

		public IList<string> AvailableThemes { get; private set; }

		private void FillThemesList()
		{
			AvailableThemes.Add(TBoxLang.None);
			foreach (var file in new DirectoryInfo(ThemesPath).SafeEnumerateFiles(Log, "*.xaml"))
			{
				AvailableThemes.Add(file.Name);
			}
		}

		public void Load(string name)
		{
			ExceptionsHelper.HandleException(
				() => DoLoad(name),
				() => "Can't load theme: " + name,
				Log
				);
		}

		private void DoLoad(string name)
		{
			if (string.Equals(lastLoaded, name)) return;
			lastLoaded = name;
			System.Windows.Application.Current.Resources.Clear();
			if (string.Equals(AvailableThemes[0], name)) return;
			using (var sr = new StreamReader(Path.Combine(ThemesPath, name)))
			{
				System.Windows.Application.Current.Resources =
					XamlReader.Load(sr.BaseStream) as ResourceDictionary;
			}
		}
	}
}
