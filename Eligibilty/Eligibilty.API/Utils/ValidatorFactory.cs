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
        public static Rules.IValidator Create(ExcelWorksheet worksheet)
        {
            Rules.IValidator validator = null;
            switch (worksheet.Name)
            {
                case "Beneficiary":
                    validator = new BeneficiaryValidator();
                    break;
                case "Policy":
                    validator = new PolicyValidator();
                    break;
                case "Claim":
                    validator = new ClaimValidator();
                    break;
            }
            return validator;
        }
    }
}
