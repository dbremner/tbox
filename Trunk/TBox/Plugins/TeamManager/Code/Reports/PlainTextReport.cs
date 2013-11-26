using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Tools;
using Localization.Plugins.TeamManager;
using TeamManager.Code.Reports.Contracts;

namespace TeamManager.Code.Reports
{
    class PlainTextReport : IReportsPrinter
    {
        private readonly Action<string> setter;
        public PlainTextReport(Action<string> setter)
        {
            this.setter = setter;
        }

        public void Print(IList<ReportPerson> items, int time)
        {
            var sb = new StringBuilder();
            foreach (var p in items)
            {
                sb.AppendLine(p.Name)
                  .AppendLine(new StringBuilder().PrintTable(
                        new []{
                                p.Columns
                            }.
                        Concat(
                            p.Days.Select(x=>new[]{x.Name}.Concat(x.Columns).ToArray()).ToArray()
                        ).Concat(
                        new []{
                                p.Summaries
                            })
                        .ToArray()
                        ).ToString());
            }
            sb.AppendLine(TeamManagerLang.ReportFooter + time);
            setter(sb.ToString());
        }
    }
}
