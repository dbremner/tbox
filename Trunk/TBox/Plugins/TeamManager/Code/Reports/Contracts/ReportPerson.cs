﻿using System.Collections.Generic;

namespace TeamManager.Code.Reports.Contracts
{
    class ReportPerson
    {
        public string Name { get; set; }
        public IList<string> Columns { get; set; }
        public IList<ReportDay> Days { get; set; }
        public IList<string> Summaries { get; set; }
    }
}
