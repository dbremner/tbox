using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;
using Localization.TBox;

namespace TBox.Code.AutoUpdate
{
	class CodePlexApplicationUpdater : IApplicationUpdater
	{
        private const string CheckUrl = "https://tbox.codeplex.com/releases/";
		private static readonly ILog Log = LogManager.GetLogger<CodePlexApplicationUpdater>();
        private string downloadLink;
        private string version;

		public bool NeedUpdate()
		{
			try
			{
				GetLastInfo();
				var newVersion = new Version(version);
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
			using (Process.Start(downloadLink)) { }
		}

		private void GetLastInfo()
		{
			using (var cl = new WebClient())
			{
			    var str = cl.DownloadString(CheckUrl);
                version = new Regex(@"TBox (?<version>\d{1,}.\d{1,})").Match(str).Groups["version"].Value;
                downloadLink = new Regex(@"href=""(?<url>https://tbox.codeplex.com/downloads/get/\d{1,})""").Match(str).Groups["url"].Value;
			}
		}
	}
}
