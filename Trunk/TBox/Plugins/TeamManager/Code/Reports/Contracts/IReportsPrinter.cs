using System.Collections.Generic;

namespace TeamManager.Code.Reports.Contracts
{
    interface IReportsPrinter
    {
        void Print(IList<ReportPerson> items, int time);
    }
}
