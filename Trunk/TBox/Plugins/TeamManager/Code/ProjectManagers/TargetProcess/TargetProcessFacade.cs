using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Interface;
using ServiceStack.Text;
using TeamManager.Code.Settings;
using WPFControls.Tools;

namespace TeamManager.Code.ProjectManagers.TargetProcess
{
	class TargetProcessFacade : IProjectManager
	{
	    private readonly IConfigManager<Config> cm;

		public TargetProcessFacade(IConfigManager<Config> cm)
		{
		    this.cm = cm;
		}

        public LoggedTime[] GetTimeReport(DateTime dateFrom, DateTime dateTo, IEnumerable<string> emails)
		{
			using (var cl = CreateClient())
			{
			    var query =
			        string.Format(
                        "Times?include=[spent,user[email],date,assignable[id]]&format=json&take=100000&where=(user.email%20in%20({2}))and(date%20gte'{0}')and(date%20lte'{1}')",
                        FormatDate(dateFrom), FormatDate(dateTo), string.Join(",", emails.Select(x => "'" + x + "'")));
                return JsonSerializer.DeserializeFromString<TimeReport>(cl.DownloadString(BuildUrl(query))).Items
                    .Select(x=>new LoggedTime
                        {
                            Date = x.Date,
                            Email = x.User.Email,
                            Task = x.Assignable.Id,
                            Spent = x.Spent
                        })
                        .ToArray();
			}
		}

	    private static string FormatDate(DateTime dateFrom)
	    {
	        return dateFrom.ToString("dd-MM-yyyy");
	    }

	    private WebClient CreateClient()
		{
			var cl = new WebClient {Credentials = new NetworkCredential(cm.Config.UserEmail, GetPassword())};
			cl.Headers.Add("Content-Type", "application/json");
			return cl;
		}

	    private string GetPassword()
	    {
            using (var ss = cm.Config.UserPassword.DecryptString())
            {
                return ss.ToInsecureString();
            }
	    }

	    private Uri BuildUrl(string relativeUri)
		{
            var url = cm.Config.ProjectManagerUrl;
            if (!string.IsNullOrEmpty(url) && url[url.Length - 1] != '/')
            {
                url += "/";
            }
			return new Uri(new Uri(url + "api/v1/"), relativeUri);
		}
	}
}
