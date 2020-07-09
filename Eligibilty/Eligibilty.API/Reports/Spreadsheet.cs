using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Wordprocessing;
using Eligibilty.API.Models;
using Eligibilty.API.Rules;
using Eligibilty.API.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;


namespace Eligibilty.API.Reports
{
    public class Spreadsheet
    {
        private void AddWorksheet(ExcelPackage excel, string title, List<string> headerRow)
        {
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(title);
            worksheet.Cells.LoadFromText(String.Join(", ", headerRow));
        }

        public void GenerateFile(string path)
        {
            ExcelPackage excel = new ExcelPackage();

            AddWorksheet(excel, "Beneficiary", new List<string>() { "Name", "Gender", "Relationship", "Date Of Birth" });
            AddWorksheet(excel, "Policy", new List<string>() { "Policy No", "Effective Date", "ExpiryDate" });
            AddWorksheet(excel, "Claim", new List<string>() { "Claim No", "Claimed Amount", "Incurred Date" });

            FileInfo excelFile = new FileInfo(path);
            excel.SaveAs(excelFile);
        }

        public void Parse(String path)
        {
            // path to your excel file
            FileInfo fileInfo = new FileInfo(path);

            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            List<List<Entity>> lists = new List<List<Entity>>();
            foreach (var worksheet in worksheets)
            {
                // get number of rows and columns in the sheet
                int rows = worksheet.Dimension.Rows;

                List<Entity> list = new List<Entity>();
                // loop through the worksheet rows and columns
                for (int i = 2; i <= rows; i++)
                    list.Add(EntityFactory.Create(worksheet, i));

                lists.Add(list);

            }

            ErrorHandling(worksheets, lists);
            package.Save();
        }

        public void ErrorHandling(ExcelWorksheets worksheets, List<List<Entity>> lists)
        {
            for (int i = 0; i < lists.Count; i++)
            {
                ExcelWorksheet worksheet = worksheets[i + 1];
                var validations = ValidatorFactory.Create(worksheet);
                var list = lists[i];
                var cols = list.Count;

                var result = validations.Validate(list);

                if (!result.IsValid)
                {
                    var errorCol = worksheet.Cells[1, cols + 1];
                    errorCol.Value = "Error Log";
                    errorCol.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));
                    errorCol.Style.Font.Bold = true;

                    foreach (var err in result.Errors)
                    {
                        Debug.WriteLine(err.PropertyName + ": " + err.ErrorMessage);

                        var invalidCell = worksheet.Cells[i, err.PropertyName.GetIndex()];
                        invalidCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        invalidCell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffc7ce"));
                        invalidCell.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));

                        worksheet.Cells[i, cols + 1].Value += $"{err.ErrorMessage} ";
                        worksheet.Cells[i, cols + 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));

                    }
                }
            }

            
        }

    }
}


