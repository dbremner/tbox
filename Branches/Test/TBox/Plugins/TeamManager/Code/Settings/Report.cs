using System;
using System.IO;

namespace TeamManager.Code.Settings
{
    public class Report 
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool FilterResultsByErrors { get; set; }
        public bool AddNotPresentDays { get; set; }
        public int WorkingHoursPerDay { get; set; }
        public string Style { get; set; }
        public string DayStatusStrategy { get; set; }
        public string ExcelFilePath { get; set; }

        public Report()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            FilterResultsByErrors = true;
            AddNotPresentDays = true;
            WorkingHoursPerDay = 8;
            Style = "default.css";
            DayStatusStrategy = "Default.cs";
            ExcelFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "default.xlsx");
        }

        public Report Clone()
        {
            return new Report
                {
                    DateFrom = DateFrom,
                    DateTo = DateTo,
                    FilterResultsByErrors = FilterResultsByErrors,
                    AddNotPresentDays = AddNotPresentDays,
                    WorkingHoursPerDay = WorkingHoursPerDay,
                    Style = Style,
                    ExcelFilePath = ExcelFilePath
                };
        }
    }
}
