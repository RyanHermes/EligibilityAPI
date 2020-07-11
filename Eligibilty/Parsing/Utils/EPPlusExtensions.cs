using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Parsing.Utils
{
    public static class EPPLusExtensions
    {
        public static Dictionary<string, int> Mappings { get; set; }

        public static IList ConvertSheetToObjects(this ExcelWorksheet worksheet, Type type)
        {

            var rows = worksheet.Dimension.Rows;

            Type itemType = type.GetGenericArguments()[0];

            var properties = itemType.GetProperties().Where(x => x.Name != "RowIdentifier").ToList();

            IList collection = (IList)Activator.CreateInstance(type);
            for (int i = 2; i <= rows; i++)
            {
                var item = Activator.CreateInstance(itemType);
                int j = 0;
                for (j = 0; j < properties.Count; j++)
                {
                    var key = $"{worksheet.Name}.{properties[j].Name}";
                    var val = worksheet.Cells[i, Mappings[key]];
                    if (val.Value != null) properties[j].BindValue(item, val);
                }

                itemType.GetProperty("RowIdentifier").SetValue(item, $"{worksheet.Name}.{j}");
                collection.Add(item);
            }
            return collection;
        }


        public static object ConvertSheetToObject(this ExcelWorksheet worksheet, Type type)
        {
            var properties = type.GetProperties().Where(x => x.Name != "RowIdentifier").ToList();

            var item = Activator.CreateInstance(type);
            int j = 0;
            for (j = 0; j < properties.Count; j++)
            {
                var val = worksheet.Cells[2, j + 1];
                if (val.Value != null) properties[j].BindValue(item, val);
            }
            type.GetProperty("RowIdentifier").SetValue(item, $"{worksheet.Name}.{j}");
            return item;

        }

        public static void BindValue(this PropertyInfo property, object item, ExcelRange val)
        {
            if (property.PropertyType == typeof(Int32)) property.SetValue(item, val.GetValue<int>());
            else if (property.PropertyType == typeof(double)) property.SetValue(item, val.GetValue<double>());
            else if (property.PropertyType == typeof(DateTime)) property.SetValue(item, val.GetValue<DateTime>());
            else property.SetValue(item, val.GetValue<string>());
        }


        public static void Invalid(this ExcelRange cell)
        {
            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            cell.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#ffc7ce"));
            cell.Style.Font.Color.SetColor(ColorTranslator.FromHtml("#be0006"));
        }

    }



}
