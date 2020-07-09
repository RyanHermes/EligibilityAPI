using Eligibilty.API.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfficeOpenXml;
using System.IO;

namespace Tests
{
    [TestClass]
    public class ParsingTest
    {
        [TestMethod]
        public void GenerateSheetTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.GenerateFile(@"..\..\..\..\..\reports\test.xlsx");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ParseSheetTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Parse(@"..\..\..\..\..\reports\test.xlsx");
        }
    }
}
