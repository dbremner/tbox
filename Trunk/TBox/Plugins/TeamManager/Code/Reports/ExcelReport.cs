using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Localization.Plugins.TeamManager;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TeamManager.Code.Reports.Contracts;

namespace TeamManager.Code.Reports
{
    class ExcelReport : IReportsPrinter
    {
        private readonly string fileName;
        public ExcelReport(string fileName)
        {
            this.fileName = fileName;
        }

        public void Print(IList<ReportPerson> items, int time, string[] links)
        {
            var newFile = new FileInfo(fileName);
            if(newFile.Exists)newFile.Delete();
            var color = Color.FromArgb(0xb0, 0xc4, 0xdd );
            using (var pck = new ExcelPackage(newFile))
            {
                var ws = pck.Workbook.Worksheets.Add(TeamManagerLang.Report);
                ws.DefaultColWidth = 24;
                var row = 1; 
                foreach (var p in items)
                {
                    var oldRow = row;
                    PrintLine(ws, p.Columns, color, ref row);
                    foreach (var day in p.Days)
                    {
                        PrintLine(ws, new[] { day.Name }.Concat(day.Columns).ToArray(), GetColor(day), ref row);
                    }
                    PrintLine(ws, p.Summaries, color, ref row);
                    var nameCell = ws.Cells[oldRow, 1, row - 1, 1];
                    PrepareCell(nameCell, p.Name, color);
                    nameCell.Merge = true;
                }
                pck.Save();
            }
        }

        private static Color GetColor(ReportDay day)
        {
            switch (day.Status)
            {
                case "holyday":
                    return Color.FromArgb(0xe6, 0xe6, 0xe6);
                case "error":
                    return Color.FromArgb(0xf8, 0x49, 0x35);
            }
            return Color.White;
        }

        private static void PrintLine(ExcelWorksheet ws, IList<string> items, Color color, ref int row)
        {
            for (var i = 0; i < items.Count; ++i)
            {
                var cell = ws.Cells[row, i + 2];
                PrepareCell(cell, items[i], color);
            }
            ++row;
        }

        private static void PrepareCell(ExcelRange cell, string value, Color color)
        {
            cell.Value = value;
            cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.WrapText = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
        }
    }
}
