using System;
using System.Globalization;
using System.Net;
using System.Linq;
using ScriptEngine;
using ServiceStack.Text;
using PluginsShared.ReportsGenerator;
using WPFControls.Tools;

namespace TBox.Data.TeamManager
{
	public class TargetProcessScript : IReportScript
	{
        class AssignableItem
        {
            public int Id { get; set; }
        }
        class UserItem
        {
            public int Id { get; set; }
            public string Email { get; set; }
        }
        class TimeItem
        {
            public double Spent { get; set; }
            public DateTime Date { get; set; }
            public AssignableItem Assignable { get; set; }
            public UserItem User { get; set; }
        }

        class TimeReport
        {
            public TimeItem[] Items { get; set; }
        }

        [String("dd-MM-yyyy")]
        public string DateTimeFormat { get; set; }
        [String]
        public string Login { get; set; }
        [Password]
        public string Password { get; set; }
        [String("https://targetprocess.com/TargetProcess2/")]
        public string TargetProcessUrl { get; set; }
        [String("{0}/restui/tpview.aspx#task/{1}", CanBeEmpty = false)]
        public string LinkUrlTemplate { get; set; }

	    public void Run(IReportScriptContext context)
		{
            context.Configure(true);
			using (var cl = CreateClient())
			{
			    var query =
			        string.Format(
                        "Times?include=[spent,user[email],date,assignable[id]]&format=json&take=100000&where=(user.email%20in%20({2}))and(date%20gte'{0}')and(date%20lte'{1}')",
                        FormatDate(context.DateFrom), FormatDate(context.DateTo), string.Join(",", context.Persons.Select(x => "'" + x + "'")));
			    foreach (var x in JsonSerializer.DeserializeFromString<TimeReport>(cl.DownloadString(BuildUrl(query))).Items)
			    {
			        var task = x.Assignable != null ? x.Assignable.Id.ToString(CultureInfo.InvariantCulture) : "DELETED";
                    context.AddResult(new LoggedInfo
                        {
                            Date = x.Date,
                            Email =
                                context.Persons.FirstOrDefault(
                                    o => string.Equals(o, x.User.Email, StringComparison.InvariantCultureIgnoreCase)),
                            Task = task,
                            Link = string.Format(LinkUrlTemplate, TargetProcessUrl, task),
                            Spent = x.Spent,
                            Column = context.Name
                        });
			    }
			}
		}

	    private string FormatDate(DateTime dateFrom)
	    {
	        return dateFrom.ToString(DateTimeFormat);
	    }

	    private WebClient CreateClient()
		{
			var cl = new WebClient {Credentials = new NetworkCredential(Login, GetPassword())};
			cl.Headers.Add("Content-Type", "application/json");
			return cl;
		}

	    private string GetPassword()
	    {
	        return Password.DecryptPassword();
	    }

	    private Uri BuildUrl(string relativeUri)
		{
            var url = TargetProcessUrl;
            if (!string.IsNullOrEmpty(url) && url[url.Length - 1] != '/')
            {
                url += "/";
            }
			return new Uri(new Uri(url + "api/v1/"), relativeUri);
		}
	}
}
