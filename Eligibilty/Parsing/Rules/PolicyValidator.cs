﻿using Eligibilty.API.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API.Rules
{
    public class PolicyValidator : AbstractValidator<Policy>
    {
        public PolicyValidator()
        {
            RuleFor(policy => policy.Id).Length(24);
            RuleFor(policy => policy.EffectiveDate).GreaterThan(DateTime.UtcNow);
            RuleFor(policy => policy.ExpiryDate).GreaterThan(DateTime.UtcNow);
        }
    }
}
