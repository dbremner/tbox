using System;
using System.Collections.Generic;
using System.Linq;
using Common.Communications;
using Common.MT;
using Common.Tools;
using Localization.Plugins.TeamManager;
using PluginsShared.ReportsGenerator;
using TeamManager.Code.Reports.Contracts;
using TeamManager.Code.Settings;
using WPFControls.Tools;

namespace TeamManager.Code
{
    class EmailsSender
    {
        private readonly Profile profile;
        private readonly FullReport fullReport;
        private readonly IList<ReportPerson> items;
        private readonly Func<IList<ReportPerson>, string> reportBuilder;

        public EmailsSender(Profile profile, FullReport fullReport, IList<ReportPerson> items, Func<IList<ReportPerson>, string> reportBuilder)
        {
            this.profile = profile;
            this.fullReport = fullReport;
            this.items = items;
            this.reportBuilder = reportBuilder;
        }

        public void Send(IUpdater u)
        {
            var sender = GetEmailSender(profile);
            SendFull(sender, u);
            SendPersonal(sender, u);
        }

        private void SendFull(IEmailSender sender, IUpdater u)
        {
            var subject = string.Format(TeamManagerLang.FullEmailSubjectTemplate, fullReport.From.ToShortDateString(),
                          fullReport.To.ToShortDateString());
            var emails = GetPersons(TimeReportType.Full)
                .Select(x => x.Key)
                .ToArray();
            if (emails.Length <= 0 && !u.UserPressClose) return;
            u.Update(TeamManagerLang.SendingFullReport, 0);
            sender.Send(subject, reportBuilder(items), true, emails);
        }

        private void SendPersonal(IEmailSender sender, IUpdater u)
        {
            var subject = string.Format(TeamManagerLang.EmailSubjectTemplate, fullReport.From.ToShortDateString(),
                                      fullReport.To.ToShortDateString());
            var needPersonal = GetPersons(TimeReportType.Personal)
                .Select(x => x.Key)
                .ToArray();
            var i = 0;
            foreach (var email in needPersonal)
            {
                var person = items.FirstOrDefault(x => x.Name.EqualsIgnoreCase(email));
                if (person == null || u.UserPressClose) continue;
                u.Update(TeamManagerLang.SendTo + ": " + email, ++i / (float)(needPersonal.Length + 1));
                sender.Send(subject, reportBuilder(new[] { person }), true, new[] { email });
            }
        }

        private IEnumerable<Person> GetPersons(TimeReportType type)
        {
            return profile.Persons.CheckedItems.Where(x => ((TimeReportType)x.ReportType).HasFlag(type));
        }

        private static IEmailSender GetEmailSender(Profile profile)
        {
            var password = profile.Email.Password.DecryptPassword();
            if (profile.Email.IsSmtp)
            {
                return new SmptEmailSender(profile.Email.ServerUrl, profile.Email.Port,
                    profile.Email.Login, password);
            }
            return new ExchangeEmailSender(new Uri(new Uri(profile.Email.ServerUrl), "/ews/Exchange.asmx").ToString(),
                profile.Email.Login, password);
        }

    }
}
