using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows;
using Common.AutoUpdate;
using Common.Base.Log;

namespace TBox.Code.AutoUpdate
{
	class CodePlexUpdater : IUpdater
	{
		private const string CheckUrl = "https://tbox.svn.codeplex.com/svn/lastversion.txt";
		private const string DownloadUrlTemplate = "https://tbox.codeplex.com/downloads/get/{0}";
		private static readonly ILog Log = LogManager.GetLogger<CodePlexUpdater>();
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
						string.Format("Found new version: {0}, current version is: {1}\nDo you want to update TBox?", newVersion, currentVersion),
						"TBox - new version is available!",
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
