using Eligibilty.API.Rules;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Eligibilty.API.Reports
{
    public class Spreadsheet
    {

        public void GenerateFile(string path, ExcelSheet sheet)
        {
            ExcelPackage excel = new ExcelPackage();

            var properties = sheet.GetType().GetProperties();
            foreach (var prop in properties)
            {
                Type type = prop.PropertyType;
                string headers = string.Empty;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Type itemType = type.GetGenericArguments()[0]; // use this...
                    var propertiesNames = itemType.GetProperties().Select(x => x.Name);
                    headers = String.Join(", ", propertiesNames);
                } else
                {
                    var propertiesNames = type.GetProperties().Select(x => x.Name);
                    headers = String.Join(", ", propertiesNames);
                }
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(prop.Name);
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

            return excelSheet;
        }

        public void ErrorHandling(ExcelSheet excel)
        {
            ExcelSheetValidator validationRules = new ExcelSheetValidator();
            var result = validationRules.Validate(excel);

            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                {
                    Debug.WriteLine(err.PropertyName + ": " + err.ErrorMessage);

                    /*                    var invalidCell = worksheet.Cells[i, err.PropertyName.GetIndex()];
                                          invalidCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                          invalidCell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffc7ce"));
                                          invalidCell.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));

                                          worksheet.Cells[i, cols + 1].Value += $"{err.ErrorMessage} ";
                                          worksheet.Cells[i, cols + 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));*/

                }

            }
        }

    }



}

public class ExcelSheet
{
    public Version Version { get; set; }

    public List<Policy> Policies { get; set; }
}

public class Version
{
    public string VersionNumber { get; set; }
}

public class Policy
{
    public string Number { get; set; }
    public string Name { get; set; }
    public int Identifier { get; set; }
}


public static class EPPLusExtensions
{
    public static IList ConvertSheetToObjects(this ExcelWorksheet worksheet, Type type)
    {

        var rows = worksheet.Cells.Select(cell => cell.Start.Row);

        Type itemType = type.GetGenericArguments()[0];

        var properties = itemType.GetProperties().ToList();

        IList collection = (IList)Activator.CreateInstance(type);
        for (int i = 2; i <= rows.Count(); i++)
        {
            var item = Activator.CreateInstance(itemType);
            for (int j = 0; j < properties.Count - 1; j++)
            {
                var val = worksheet.Cells[i, j];
                if (val.Value != null)
                {
                    properties[j].SetValue(item, val.GetValue<string>());
                }
            }

            properties[properties.Count - 1].SetValue(item, i);
            collection.Add(item);
        }
        return collection;
    }

    public static object ConvertSheetToObject(this ExcelWorksheet worksheet, Type type)
    {

        var rows = worksheet.Cells.Select(cell => cell.Start.Row);

        Type itemType = type.GetGenericArguments()[0];

        var properties = itemType.GetProperties().ToList();



        var item = Activator.CreateInstance(itemType);
        for (int j = 0; j < properties.Count - 1; j++)
        {
            var val = worksheet.Cells[2, j];
            if (val.Value != null)
            {
                properties[j].SetValue(item, val.GetValue<string>());
            }
        }

        properties[properties.Count - 1].SetValue(item, 2);

        return item;
    }
}

