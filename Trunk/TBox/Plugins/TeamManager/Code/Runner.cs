using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Common.Tools;
using Interface;
using TeamManager.Code.ProjectManagers;
using TeamManager.Code.ProjectManagers.TargetProcess;
using TeamManager.Code.Reports;
using TeamManager.Code.Settings;

namespace TeamManager.Code
{
    public class Runner
    {
        private readonly IConfigManager<Config> cm;
        private readonly ReportsBuilder reportBuilder;
        private readonly TargetProcessFacade facade;

        public Runner(IConfigManager<Config> cm)
        {
            this.cm = cm;
            reportBuilder = new ReportsBuilder();
            facade = new TargetProcessFacade(cm);
        }

        public string GetTimeTable(DateTime from, DateTime to, ref List<string> emails)
        {
            var items = facade.GetTimeReport(from, to, emails).ToList();
            if (cm.Config.Report.FilterResultsByTime)
            {
                var passed = items.GroupBy(x => x.Email)
                    .Where(x=>(int)x.Sum(y=>y.Spent) == cm.Config.Report.TargetTime)
                    .Select(x=>x.Key)
                    .ToArray();
                emails = emails.Where(x => !passed.Contains(x.ToLower(), new EqualityNoCaseComparer())).ToList();
                items = items.Where(x => !passed.Contains(x.Email)).ToList();
            }
            foreach (var email in emails
                .Where(email => !items.Any(x => x.Email.EqualsIgnoreCase(email))))
            {
                items.Add(new LoggedTime
                    {
                        Fake = true,
                        Email = email
                    });
            }

            return reportBuilder.BuildLoggedTimeReport(items);
        }
    }
}
