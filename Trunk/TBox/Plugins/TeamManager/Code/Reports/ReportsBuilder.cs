using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Tools;
using Localization.Plugins.TeamManager;
using TeamManager.Code.ProjectManagers;

namespace TeamManager.Code.Reports
{
    class ReportsBuilder
    {
        public string BuildLoggedTimeReport(IList<LoggedTime> times)
        {
            return string.Join(Environment.NewLine+Environment.NewLine,
                times.OrderBy(x => x.Email)
                 .GroupBy(x => x.Email)
                 .Select(x=>x.Key + BuildLoggedTimeReportForUser(x.ToArray()))
                );
        }

        private static string BuildLoggedTimeReportForUser(IList<LoggedTime> times)
        {
            var report = times
                .Where(x=>!x.Fake)
                .OrderBy(x => x.Date)
                .GroupBy(x => x.Date.ToShortDateString())
                .Select(x => FormatTimeLine(x.Key, x.ToArray()))
                .ToList();
            report.Add(new[] { TeamManagerLang.Days + ":" + report.Count, TeamManagerLang.Spent  +":" + times.Sum(x => x.Spent), string.Empty });
            report.Insert(0, new[] {TeamManagerLang.Date, TeamManagerLang.Spent, TeamManagerLang.Assignable});

            return new StringBuilder().FormatTable(report.ToArray()).ToString();
        }

        private static string[] FormatTimeLine(string date, LoggedTime[] items)
        {
            return new[]
                {
                    date,
                    items.Sum(x => x.Spent).ToString(CultureInfo.InvariantCulture),
                    string.Join(";", items.GroupBy(x=>x.Task).Select(x =>
                        {
                            var count = x.Count();
                            if (count < 2) return x.Key.ToString(CultureInfo.InvariantCulture);
                            return string.Format("{0}(x{1})", x.Key, count);
                        }))
                };
        }
    }
}
