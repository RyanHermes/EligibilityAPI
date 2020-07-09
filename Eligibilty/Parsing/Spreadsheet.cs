using OfficeOpenXml;
using OfficeOpenXml.Style;
using Parsing.Models;
using Parsing.Rules;
using Parsing.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Eligibilty.API.Reports
{
    public class Spreadsheet
    {
        public Dictionary<string, int> Mappings { get; set; } = new Dictionary<string, int>();

        public void GenerateFile(string path)
        {
            ExcelPackage excel = new ExcelPackage();

            var properties = typeof(ExcelSheet).GetProperties();
            foreach (var property in properties)
            {
                Type type = property.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
                string headers = String.Join(",", type.GetProperties().Select(x => x.Name));
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(property.Name);
                worksheet.Cells.LoadFromText(headers);
            }

            FileInfo excelFile = new FileInfo(path);
            excel.SaveAs(excelFile);
        }

        public ExcelSheet Parse(String path)
        {
            // path to your excel file
            FileInfo fileInfo = new FileInfo(path);

            ExcelPackage package = new ExcelPackage(fileInfo);

            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            ValidateStruct(worksheets);

            ExcelSheet excelSheet = new ExcelSheet();
            for (int i = 0; i < worksheets.Count; i++)
            {
                string sheetName = worksheets[i].Name;
                PropertyInfo prop = excelSheet.GetType().GetProperty(sheetName);

                Type type = prop.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    var newcollection = worksheets[i].ConvertSheetToObjects(type);
                    prop.SetValue(excelSheet, newcollection, null);
                }
                else
                {
                    var obj = worksheets[i].ConvertSheetToObject(type);
                    prop.SetValue(excelSheet, obj, null);
                }
            }

            ErrorHandling(package, excelSheet);
            return excelSheet;
        }

        public void ErrorHandling(ExcelPackage package, ExcelSheet excel)
        {
            ExcelSheetValidator validationRules = new ExcelSheetValidator();
            var result = validationRules.Validate(excel);

            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                {
                    Debug.WriteLine(err.PropertyName + ": " + err.ErrorMessage);

                    string[] split = err.PropertyName.Split(new Char[] { '[', ']' });
                    string sheetname = split[0];
                    int row = int.Parse(split[1]) + 2;

                    string colHeader = split[2];
                    int col = Mappings[$"{sheetname}{colHeader}"];

                    var worksheet = package.Workbook.Worksheets[sheetname];
                    var invalidCell = worksheet.Cells[row, col];
                    invalidCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    invalidCell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffc7ce"));
                    invalidCell.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));

                    int cols = worksheet.Dimension.Columns;
                    worksheet.Cells[row, cols + 1].Value += $"{err.ErrorMessage} ";
                    worksheet.Cells[row, cols + 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));
                }

                FileInfo excelFile = new FileInfo(@"..\..\..\..\..\reports\test.xlsx");
                package.SaveAs(excelFile);

            }
        }

        public bool ValidateStruct(ExcelWorksheets worksheets)
        {
            var properties = typeof(ExcelSheet).GetProperties().Select(x => x.Name);
            var sheestName = worksheets.Select(x => x.Name);
            if (!Enumerable.SequenceEqual(properties, sheestName)) return false;

            foreach (var sheet in worksheets)
            {
                var prop = typeof(ExcelSheet).GetProperty(sheet.Name);
                Type type = prop.PropertyType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
                var props = type.GetProperties().Select(x => x.Name);

                var header = new List<string>();
                for (int i = 1; i <= sheet.Dimension.Columns; i++)
                {
                    string val = sheet.Cells[1, i].Value.ToString();
                    header.Add(val);
                    Mappings.Add($"{sheet.Name}.{val}", i);
                }
                if (!Enumerable.SequenceEqual(props, header)) return false;

            }

            return true;
        }

    }



}









