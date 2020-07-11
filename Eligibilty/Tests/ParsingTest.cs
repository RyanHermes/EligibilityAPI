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
            Spreadsheet spreadsheet = new Spreadsheet(@"..\..\..\..\..\reports\test.xlsx");
            spreadsheet.GenerateFile();
        }

        [TestMethod]
        public void ValidateStructTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet(@"..\..\..\..\..\reports\test.xlsx");
            spreadsheet.ValidateStructure();
        }

        [TestMethod]
        public void ValidateDataTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet(@"..\..\..\..\..\reports\test.xlsx");
            spreadsheet.ValidateStructure();
            spreadsheet.ParseFile();
            spreadsheet.ValidateData();
        }
    }
}
