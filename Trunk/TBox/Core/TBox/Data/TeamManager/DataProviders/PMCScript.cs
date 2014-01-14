using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Mnk.Library.ScriptEngine;
using Mnk.TBox.Core.PluginsShared.ReportsGenerator;
using Mnk.Library.WPFControls.Tools;

namespace TBox.Data.TeamManager
{
    public class PmcScript : IReportScript
	{
        [String("M/d/yyyy")]
        public string DateTimeFormat { get; set; }
        [String]
        public string TokenLogin { get; set; }
        [Password]
        public string TokenPassword { get; set; }
        [String]
        public string Login { get; set; }
        [Password]
        public string Password { get; set; }
        [File("Tools/PMCTimeReportGenerator.exe")]
        public string ReportGeneratorExe { get; set; }


	    public void Run(IReportScriptContext context)
	    {
	        context.Configure(true);
			var info = new ProcessStartInfo
			{
				FileName = ReportGeneratorExe,
                Arguments = GetArgs(context.DateFrom, context.DateTo, context.Persons),
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};
			using (var p = Process.Start(info) )
			{
				var s = p.StandardOutput;
				while (!s.EndOfStream)
				{
					var line = s.ReadLine();
					if (string.IsNullOrEmpty(line)) continue;
					var cols = line.Split('\t');
					if (cols.Length < 4) continue;
                    var email = GetEmail(cols[0].Trim(), context.Persons);
                    var date = DateTime.ParseExact(cols[3].Trim().Split(' ').FirstOrDefault(), DateTimeFormat, new DateTimeFormatInfo());
                    if (context.Persons.Contains(email))
					{
						context.AddResult(new LoggedInfo
						{
							Email = email,
							Date = date,
                            Spent = double.Parse(cols[2]),
							Task = cols[1].Split(' ').FirstOrDefault(),
							Column = context.Name
						});
					}
				}
                if (p.ExitCode < 0)
                {
                    throw new ArgumentException(p.StandardOutput.ReadToEnd());
                }
			}
		}

        private static string GetEmail(string email, IList<string> emails)
	    {
	        if (emails.Contains(email))
	            return email;
            email = email.Replace(" ", "_");
	        return emails.FirstOrDefault(x => (x.Split('@').FirstOrDefault()??string.Empty).StartsWith(email, StringComparison.InvariantCultureIgnoreCase));
	    }

		private string GetArgs(DateTime dateFrom, DateTime dateTo, IEnumerable<string> persons)
		{
            var args = string.Format("{0} {1} {2} {3} {4} {5}",
                TokenLogin, TokenPassword.DecryptPassword(),
                Login, Password.DecryptPassword(),
                FormatDate(dateFrom), FormatDate(dateTo.AddDays(1)));
		    return persons.Aggregate(args, (current, p) => current + (" " + p));
		}

		private string FormatDate(DateTime dateFrom)
		{
			return dateFrom.ToString(DateTimeFormat);
		}

	}
}
