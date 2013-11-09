﻿using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;
using Localization.TBox;

namespace TBox.Code.AutoUpdate
{
	class CodePlexApplicationUpdater : IApplicationUpdater
	{
		private const string CheckUrl = "https://tbox.svn.codeplex.com/svn/lastversion.txt";
		private const string DownloadUrlTemplate = "https://tbox.codeplex.com/downloads/get/{0}";
		private static readonly ILog Log = LogManager.GetLogger<CodePlexApplicationUpdater>();
		private string[] updateInfo;

		public bool NeedUpdate()
		{
			try
			{
				updateInfo = GetLastInfo();
				var newVersion = new Version(updateInfo[0]);
				var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
				if ( newVersion > currentVersion)
				{
					return MessageBox.Show(
						string.Format(TBoxLang.FoundNewVersionTemplate, newVersion, currentVersion),
						TBoxLang.NewVersionAvailable,
						MessageBoxButton.YesNo,
						MessageBoxImage.Question,
						MessageBoxResult.Yes,
						MessageBoxOptions.ServiceNotification) == MessageBoxResult.Yes;
				}
			}
			catch (Exception ex)
			{
				Log.Write(ex, "Can't get last verison of TBox");
			}
			return false;
		}

		public void Copy(string newPath)
		{
			throw new NotImplementedException();
		}

		public void Update()
		{
			using (Process.Start(string.Format(DownloadUrlTemplate, updateInfo[1]))) { }
		}

		private static string[] GetLastInfo()
		{
			using (var cl = new WebClient())
			{
				return cl.DownloadString(CheckUrl).Split(new[] {"#"}, StringSplitOptions.RemoveEmptyEntries);
			}
		}
	}
}