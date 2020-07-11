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
        public Dictionary<string, int> Mappings { get; set; }
        private readonly ExcelPackage package;
        private readonly ExcelSheet excelSheet;
        private readonly FileInfo fileInfo;

        public Spreadsheet(string path)
        {
            Mappings = new Dictionary<string, int>();

            fileInfo = new FileInfo(path);
            package = new ExcelPackage(fileInfo);

            excelSheet = new ExcelSheet();

        }

        private ExcelWorksheet AddWorksheet(PropertyInfo property)
        {
            Type type = property.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)) type = type.GetGenericArguments()[0];
            string headers = String.Join(",", type.GetProperties().Where(x => x.Name != "RowIdentifier").Select(x => x.Name));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(property.Name);
            worksheet.Cells.LoadFromText(headers);
            return worksheet;
        }

        public void GenerateFile()
        {
            var properties = typeof(ExcelSheet).GetProperties();
            foreach (var property in properties) AddWorksheet(property);
            package.SaveAs(fileInfo);
        }

        public ExcelSheet ParseFile()
        {
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

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

            return excelSheet;
        }

        public void ValidateData()
        {
            ExcelSheetValidator validationRules = new ExcelSheetValidator();
            var result = validationRules.Validate(excelSheet);

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
                    worksheet.Cells[row, col].Invalid();
                    
                    int cols = worksheet.Dimension.Columns;

                    worksheet.Cells[row, cols + 1].Value += $"{err.ErrorMessage}{Environment.NewLine}";
                    worksheet.Cells[row, cols].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));
                }

                package.SaveAs(fileInfo);

            }
        }

        public void ValidateStructure()
        {
            var worksheets = package.Workbook.Worksheets;
            var requiredSheetNames = typeof(ExcelSheet).GetProperties().Select(x => x.Name);
            var providedSheetNames = worksheets.Select(x => x.Name);

            var missingSheets = requiredSheetNames.Except(providedSheetNames).ToList();

            foreach (var missingSheet in missingSheets)
            {
                var worksheet = AddWorksheet(typeof(ExcelSheet).GetProperty(missingSheet));
                int cols = worksheet.Dimension.Columns;
                worksheet.Cells[1, 1, 1, cols].Invalid();
                worksheet.TabColor = ColorTranslator.FromHtml("#ffc7ce");
            }


            foreach (var sheet in worksheets)
            {
                var sheetProperty = typeof(ExcelSheet).GetProperty(sheet.Name);

                if (sheetProperty == null)
                {
                    sheet.TabColor = ColorTranslator.FromHtml("#ffc7ce");
                    continue;
                }

                Type sheetType = sheetProperty.PropertyType;
                if (sheetType.IsGenericType && sheetType.GetGenericTypeDefinition() == typeof(List<>)) sheetType = sheetType.GetGenericArguments()[0];
                
                var requiredSheetHeader = sheetType.GetProperties().Where(x => x.Name != "RowIdentifier").Select(x => x.Name);

                var providedSheetHeader = new List<string>();

                for (int i = 1; i <= sheet.Dimension.Columns; i++)
                {
                    var value = sheet.Cells[1, i].Value;
                    if (value == null)
                    {
                        sheet.Cells[1, i].Invalid();
                        continue;
                    }

                    providedSheetHeader.Add(value.ToString());
                    Mappings.Add($"{sheet.Name}.{value}", i);
                }

                var missingHeaders = requiredSheetHeader.Except(providedSheetHeader).ToList();

                foreach (var missingHeader in missingHeaders)
                {
                    var cell = sheet.Cells[1, sheet.Dimension.Columns + 1];
                    cell.Value = missingHeader;
                    cell.Invalid();
                }


                var hashSet = new HashSet<string>(requiredSheetHeader);
                for (int i = 0; i < providedSheetHeader.Count; i++) if (!hashSet.Contains(providedSheetHeader[i])) sheet.Cells[1, i + 1].Invalid();

            }

            EPPLusExtensions.Mappings = Mappings;
            package.SaveAs(fileInfo);
        }
    }


}













