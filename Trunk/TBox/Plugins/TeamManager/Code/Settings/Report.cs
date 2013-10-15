using System;

namespace TeamManager.Code.Settings
{
    public class Report
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int TargetTime { get; set; }
        public bool FilterResultsByTime { get; set; }

        public Report()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            FilterResultsByTime = true;
            TargetTime = 40;
        }
    }
}
