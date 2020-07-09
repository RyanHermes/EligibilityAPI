using CsvHelper;
using CsvHelper.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API.Reports
{
    public class CsvSheet
    {
        public void GenerateFile()
        {
            using (var reader = new CsvReader(new ExcelParser("path/to/file.xlsx")))
            {
                var people = reader.GetRecords<Person>();
            }

        }
    }
}
