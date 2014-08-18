using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Mnk.TBox.Locales.Localization.Plugins.TeamManager;
using Mnk.TBox.Plugins.TeamManager.Code.Reports.Contracts;
using DocumentFormat.OpenXml;

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
            var color = "FFb0c4dd";
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
                var stylesPart = doc.WorkbookPart.AddNewPart<WorkbookStylesPart>();
                stylesPart.Stylesheet = new Stylesheet
                {
                    Fills = new Fills { Count = 0 }, 
                    Borders = new Borders { Count = 0 }, 
                    CellFormats = new CellFormats{Count = 0}
                };

                var book = doc.WorkbookPart;
                var sheet = book.WorksheetParts.First();
                var rowNo = 0;
                foreach (var p in items)
                {
                    PrintLine(book, sheet, p.Columns, ++rowNo, color);
                    foreach (var day in p.Days)
                    {
                        PrintLine(book, sheet, new[] { day.Name }.Concat(day.Columns).ToArray(), ++rowNo, GetColor(day));
                    }
                    PrintLine(book, sheet, p.Summaries, ++rowNo, color);
                    /*
                    var nameCell = ws.Cells[oldRowNo, 1, rowNo - 1, 1];
                    PrepareCell(nameCell, p.Name, color);
                    nameCell.Merge = true;
                     */
                }

                doc.WorkbookPart.Workbook.Save();
            }
        }

        private static string GetColor(ReportDay day)
        {
            switch (day.Status)
            {
                case "holyday":
                    return "FFe6e6xe6";
                case "error":
                    return "FFf84935";
            }
            return "FFFFFFFF";
        }

        private static void PrintLine(WorkbookPart workbookPart, WorksheetPart workSheetPart, IList<string> items, int rowNo, string color)
        {
            var col = 1;
            foreach (var t in items)
            {
                var colId = ((char) ((int) 'A' + (++col))).ToString(CultureInfo.InvariantCulture);
                PrepareCell(workbookPart, workSheetPart, colId, rowNo, t, color);
            }
        }

        private static void PrepareCell(WorkbookPart workbookPart, WorksheetPart workSheetPart, string colId, int rowNo, string value, string color)
        {
            var cell = InsertCellInWorksheet(colId, rowNo, workSheetPart);
            cell.CellValue = new CellValue(value);
            cell.DataType = CellValues.String;

            var cellFormat = cell.StyleIndex != null ? (CellFormat)GetCellFormat(workbookPart, cell.StyleIndex).CloneNode(true) : new CellFormat();
            cellFormat.FillId = InsertFill(workbookPart, GenerateFill(color));
            cellFormat.BorderId = InsertBorder(workbookPart, GenerateBorder());

            cell.StyleIndex = InsertCellFormat(workbookPart, cellFormat);

            /*
            cell.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(color);
            
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.WrapText = true;
             */
        }
        private static Border GenerateBorder()
        {
            var border = new Border();

            var leftBorder = new LeftBorder { Style = BorderStyleValues.Medium };
            var rightBorder = new RightBorder { Style = BorderStyleValues.Medium };
            var topBorder = new TopBorder{ Style = BorderStyleValues.Medium };
            var bottomBorder = new BottomBorder{ Style = BorderStyleValues.Medium };

            border.Append(new OpenXmlElement[]{
                leftBorder,
                rightBorder,
                topBorder,
                bottomBorder
            });
            return border;
        }

        private static Fill GenerateFill(string color)
        {
            var fill = new Fill();

            var patternFill = new PatternFill { PatternType = PatternValues.Solid };
            var backgroundColor1 = new BackgroundColor { Rgb = color};

            patternFill.Append(new OpenXmlElement[]{backgroundColor1});

            fill.Append(new OpenXmlElement[]{patternFill});

            return fill;
        }

        private static uint InsertBorder(WorkbookPart workbookPart, Border border)
        {
            var borders = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Borders>().First();
            borders.Append(new OpenXmlElement[]{border});
            return borders.Count++;
        }

        private static uint InsertFill(WorkbookPart workbookPart, Fill fill)
        {
            var fills = workbookPart.WorkbookStylesPart.Stylesheet.Elements<Fills>().First();
            fills.Append(new OpenXmlElement[]{fill});
            return fills.Count++;
        }

        private static CellFormat GetCellFormat(WorkbookPart workbookPart, uint styleIndex)
        {
            return workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First().Elements<CellFormat>().ElementAt((int)styleIndex);
        }

        private static uint InsertCellFormat(WorkbookPart workbookPart, CellFormat cellFormat)
        {
            var cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.Elements<CellFormats>().First();
            cellFormats.Append(new OpenXmlElement[]{cellFormat});
            return (uint)cellFormats.Count++;
        }

        private static Cell InsertCellInWorksheet(string columnName, int rowIndex, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            Row row;
            if (sheetData.Elements<Row>().Count(r => r.RowIndex == rowIndex) != 0)
            {
                row = sheetData.Elements<Row>().First(r => r.RowIndex == rowIndex);
            }
            else
            {
                row = new Row{ RowIndex = (uint)rowIndex };
                sheetData.Append(new OpenXmlElement[]{row});
            }

            if (row.Elements<Cell>().Any(c => c.CellReference.Value == columnName + rowIndex))
            {
                return row.Elements<Cell>().First(c => c.CellReference.Value == cellReference);
            }
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell refCell = null;
            foreach (var cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            var newCell = new Cell { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}
