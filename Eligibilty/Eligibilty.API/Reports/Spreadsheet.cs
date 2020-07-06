using Eligibilty.API.Models;
using Eligibilty.API.Rules;
using Eligibilty.API.Utils;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Eligibilty.API.Reports
{
    public class Spreadsheet
    {
        private void AddWorksheet(ExcelPackage excel, string title, List<string[]> headerRow)
        {
            excel.Workbook.Worksheets.Add(title);
            ExcelWorksheet worksheet = excel.Workbook.Worksheets[title];
            int len = headerRow[0].Length;
            var header = worksheet.Cells[1, 1, 1, len];

            header.LoadFromArrays(headerRow);
            header.Style.Font.Bold = true;
            header.AutoFitColumns();

            worksheet.Protection.IsProtected = true;
            worksheet.Cells[2, 1, 1000, len].Style.Locked = false;
            worksheet.Protection.AllowFormatColumns = true;

        }

        public void GenerateFile()
        {
            ExcelPackage excel = new ExcelPackage();

            AddWorksheet(excel, "Add Beneficiary", new List<string[]>() { new string[] { "Name", "Gender", "Relationship", "Date Of Birth" } });
            AddWorksheet(excel, "Add Policy", new List<string[]>() { new string[] { "Effective Date", "ExpiryDate" } });
            AddWorksheet(excel, "Add Claim", new List<string[]>() { new string[] { "Policy No", "Claimed Amount", "Incurred Date" } });

            FileInfo excelFile = new FileInfo(@"..\..\..\..\..\reports\test.xlsx");
            excel.SaveAs(excelFile);
        }

        public void Parse(String path)
        {
            // path to your excel file
            FileInfo fileInfo = new FileInfo(path);

            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets["Add Beneficiary"];

            // get number of rows and columns in the sheet
            int rows = worksheet.Dimension.Rows;
            int cols = worksheet.Dimension.Columns;

            // loop through the worksheet rows and columns
            for (int i = 2; i <= rows; i++)
            {
                Beneficiary beneficiary = null;
                try
                {
                    beneficiary = new Beneficiary()
                    {
                        Name = worksheet.Cells[i, 1].Value.ToString(),
                        Gender = worksheet.Cells[i, 2].Value.ToString().ParseGender(),
                        Relationship = worksheet.Cells[i, 3].Value.ToString().ParseRelationship(),
                        DateOfBirth = DateTimeOffset.Parse(worksheet.Cells[i, 4].Value.ToString())
                    };
                }
                catch { }

                if (beneficiary == null) continue;
                Debug.WriteLine($"{beneficiary.Name} {beneficiary.Gender} {beneficiary.Relationship} {beneficiary.DateOfBirth}");
                var validations = new BeneficiaryValidator();
                var result = validations.Validate(beneficiary);

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

            worksheet.Cells[1, 1, rows, cols + 1].AutoFitColumns();
            package.Save();
        }

    }

}

