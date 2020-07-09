using Eligibilty.API.Rules;
using FluentValidation;
using Parsing.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Rules
{
    public class ExcelSheetValidator : AbstractValidator<ExcelSheet>
    {
        public ExcelSheetValidator()
        {
            RuleFor(excel => excel.Beneficiaries).ForEach(x => x.SetValidator(new BeneficiaryValidator()));
            RuleFor(excel => excel.Policies).ForEach(x => x.SetValidator(new PolicyValidator()));
            RuleFor(excel => excel.Claims).ForEach(x => x.SetValidator(new ClaimValidator()));
        }
    }
}
