using Eligibilty.API.Reports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ParseTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.Parse(@"..\..\..\..\..\reports\test.xlsx");
        }

        [TestMethod]
        public void GenerateTest()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.GenerateFile();
        }

    }
}
