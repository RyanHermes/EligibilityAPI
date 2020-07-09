using Eligibilty.API.Models;
using Eligibilty.API.Rules;
using FluentValidation;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API.Utils
{
    public class ValidatorFactory
    {
        public static void Validate(ExcelWorksheet worksheet, List<Entity> list)
        {
            if (worksheet.Name == "Beneficiary") {
                var validator = new BeneficiaryValidator();
                //validator.Validate(list);
            }
            if (worksheet.Name == "Policy") {
                var validator = new PolicyValidator();
            }
            if (worksheet.Name == "Claim") { 
                    var validator = new ClaimValidator();
            }
            }
            //return validator;
        }
    }
}
