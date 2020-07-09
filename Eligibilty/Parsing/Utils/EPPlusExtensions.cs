using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Parsing.Utils
{
    public static class EPPLusExtensions
    {
        public static IList ConvertSheetToObjects(this ExcelWorksheet worksheet, Type type)
        {

            var rows = worksheet.Dimension.Rows;

            Type itemType = type.GetGenericArguments()[0];

            var properties = itemType.GetProperties().ToList();

            IList collection = (IList)Activator.CreateInstance(type);
            for (int i = 2; i <= rows; i++)
            {
                var item = Activator.CreateInstance(itemType);
                for (int j = 0; j < properties.Count; j++)
                {
                    var val = worksheet.Cells[i, j + 1];
                    if (val.Value != null) properties[j].BindValue(item, val);
                }

                collection.Add(item);
            }
            return collection;
        }


        public static object ConvertSheetToObject(this ExcelWorksheet worksheet, Type type)
        {
            var properties = type.GetProperties().ToList();

            var item = Activator.CreateInstance(type);
            for (int j = 0; j < properties.Count; j++)
            {
                var val = worksheet.Cells[2, j + 1];
                if (val.Value != null) properties[j].BindValue(item, val);
            }
            return item;

        }

        public static void BindValue(this PropertyInfo property, object item, ExcelRange val)
        {
            if (property.PropertyType == typeof(Int32)) property.SetValue(item, val.GetValue<int>());
            else if (property.PropertyType == typeof(double)) property.SetValue(item, val.GetValue<double>());
            else if (property.PropertyType == typeof(DateTime)) property.SetValue(item, val.GetValue<DateTime>());
            else property.SetValue(item, val.GetValue<string>());
        }




    }



}
