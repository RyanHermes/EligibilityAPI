using DocumentFormat.OpenXml.Drawing;
using Eligibilty.API.Models;
using MongoDB.Bson;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API.Utils
{
    public class EntityFactory
    {
        public static Entity Create(ExcelWorksheet worksheet, int row)
        {
            Entity entity = null;
            switch (worksheet.Name)
            {
                case "Beneficiary":
                    entity = new Beneficiary()
                    {
                        Name = worksheet.Cells[row, 1].Value.ToString(),
                        Gender = worksheet.Cells[row, 2].Value.ToString().ParseGender(),
                        Relationship = worksheet.Cells[row, 3].Value.ToString().ParseRelationship(),
                        DateOfBirth = DateTimeOffset.Parse(worksheet.Cells[row, 4].Value.ToString())
                    };
                    break;
                case "Claim":
                    entity = new Claim()
                    {
                        Id = worksheet.Cells[row, 1].Value.ToString(),
                        ClaimedAmount = double.Parse(worksheet.Cells[row, 2].Value.ToString()),
                        IncurredDate = DateTimeOffset.Parse(worksheet.Cells[row, 3].Value.ToString())
                    };
                    break;
                case "Policy":
                    entity = new Policy()
                    {
                        Id = worksheet.Cells[row, 1].Value.ToString(),
                        EffectiveDate = DateTimeOffset.Parse(worksheet.Cells[row, 2].Value.ToString()),
                        ExpiryDate = DateTimeOffset.Parse(worksheet.Cells[row, 3].Value.ToString())
                    };
                    break;
            }
            return entity;
        }

    }
}
