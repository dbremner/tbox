using System;
using System.IO;
using System.Net;
using System.Windows;
using PluginsShared.Encoders;
using TeamManager.Code.Settings;

namespace TeamManager.Code.ProjectManagers
{
	class TargetProcessFacade : IProjectManager
	{
		private readonly string userName;
		private readonly string password;
		private readonly string mainUrl;

		public TargetProcessFacade(string userName, string password, string mainUrl)
		{
			this.userName = userName;
			this.password = password;
			this.mainUrl = mainUrl;
            if (!string.IsNullOrEmpty(mainUrl) && mainUrl[mainUrl.Length - 1] != '/')
            {
                this.mainUrl += "/";
            }
		}

		public string[] GetAllUserStories()
		{
			using (var cl = CreateClient())
			{
				var data = cl.DownloadString(BuildUrl("Userstories"));
				MessageBox.Show(data);
			}
			return new string[0];
		}

		private WebClient CreateClient()
		{
			var cl = new WebClient {Credentials = new NetworkCredential(userName, password)};
			cl.Headers.Add("Content-Type", "application/json");
			return cl;
		}

		private Uri BuildUrl(string relativeUri)
		{
			return new Uri(new Uri(mainUrl + "api/v1/"), relativeUri);
		}
	}
}
