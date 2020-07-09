using Eligibilty.API.Models;
using FluentValidation;
using System;
using System.Linq;

namespace Eligibilty.API.Rules
{
    public class BeneficiaryValidator : AbstractValidator<Beneficiary>
    {
        public BeneficiaryValidator()
        {
            RuleFor(beneficiary => beneficiary.Id).Length(24);
            RuleFor(beneficiary => beneficiary.Name).NotNull().MinimumLength(2).MaximumLength(32).Must(name => name.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c)));
            RuleFor(beneficiary => beneficiary.Gender).IsInEnum();
            RuleFor(beneficiary => beneficiary.Relationship).IsInEnum();
            RuleFor(beneficiary => beneficiary.DateOfBirth).LessThan(DateTime.UtcNow).GreaterThan(DateTime.UtcNow.AddYears(-100));
        }
    }
}
