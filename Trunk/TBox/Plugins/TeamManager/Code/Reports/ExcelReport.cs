using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Mnk.TBox.Locales.Localization.Plugins.TeamManager;
using Mnk.TBox.Plugins.TeamManager.Code.Reports.Contracts;
using DocumentFormat.OpenXml;
using Color = System.Drawing.Color;

namespace Mnk.TBox.Plugins.TeamManager.Code.Reports
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
            using (var doc = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                doc.AddWorkbookPart().AddNewPart<WorksheetPart>().Worksheet = new Worksheet(new SheetData());
                doc.WorkbookPart.Workbook =
                    new Workbook(
                        new Sheets(
                            new Sheet
                            {
                                Id = doc.WorkbookPart.GetIdOfPart(doc.WorkbookPart.WorksheetParts.First()),
                                SheetId = 1,
                                Name = TeamManagerLang.Report,
                            }));
                doc.WorkbookPart.WorksheetParts.First().Worksheet.AppendChild(new SheetData());

                var sheet = doc.WorkbookPart.WorksheetParts.First().Worksheet.First();
                foreach (var p in items)
                {
                    PrintLine(sheet, p.Columns, color);
                    foreach (var day in p.Days)
                    {
                        PrintLine(sheet, new[] { day.Name }.Concat(day.Columns).ToArray(), GetColor(day));
                    }
                    PrintLine(sheet, p.Summaries, color);
                    /*
                    var nameCell = ws.Cells[oldRowNo, 1, rowNo - 1, 1];
                    PrepareCell(nameCell, p.Name, color);
                    nameCell.Merge = true;
                     */
                }

                doc.WorkbookPart.Workbook.Save();
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

        private static void PrintLine(OpenXmlElement ws, IList<string> items, Color color)
        {
            var row = new Row();
            ws.AppendChild(row);
            row.AppendChild(new Cell());
            row.AppendChild(new Cell());

            foreach (var t in items)
            {
                var cell = new Cell{DataType = CellValues.String};
                row.AppendChild(cell);
                PrepareCell(cell, t, color);
            }
        }

        private static void PrepareCell(Cell cell, string value, Color color)
        {
            cell.CellValue = new CellValue(value);
            /*
            cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.WrapText = true;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
             */
        }
    }
}
