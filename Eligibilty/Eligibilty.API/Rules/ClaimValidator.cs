﻿using Eligibilty.API.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API.Rules
{
    public class ClaimValidator : AbstractValidator<Claim>
    {
        public ClaimValidator()
        {
            RuleFor(claim => claim.Id).Length(24);
            RuleFor(claim => claim.PolicyNo).Length(24);
            RuleFor(claim => claim.ClaimedAmount).GreaterThan(0).LessThan(2000000);
            RuleFor(claim => claim.IncurredDate).GreaterThan(DateTime.MinValue);
        }
    }

    public class ExcelSheetValidator : AbstractValidator<ExcelSheet>
    {
        public ExcelSheetValidator()
        {
            RuleFor(excel => excel.Policies).ForEach(x => x.SetValidator(new PolicyValidator()));
        }
    }
}
