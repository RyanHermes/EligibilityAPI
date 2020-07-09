using Eligibilty.API.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GenerateExcelTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.GenerateFile(@"..\..\..\..\..\reports\test.xlsx", new ExcelSheet());
        }

        [TestMethod]
        public void ParseExcelTest()    
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            var excel = spreadsheet.Parse(@"..\..\..\..\..\reports\test.xlsx");

            spreadsheet.ErrorHandling(excel);
            Assert.IsTrue(true);
        }
    }
}
