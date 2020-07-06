using Eligibilty.API.Models;
using Eligibilty.API.Services;
using Eligibilty.API.Rules;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FluentValidation.Results;

namespace Eligibilty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BeneficiariesController : ControllerBase
    {
        private readonly BeneficiaryService _beneficiaryServive;
        private readonly BeneficiaryValidator _validations;

        public BeneficiariesController(BeneficiaryService beneficiaryService)
        {
            _beneficiaryServive = beneficiaryService;
            _validations = new BeneficiaryValidator();
        }

        [HttpGet]
        public ActionResult<List<Beneficiary>> Get() => _beneficiaryServive.Get();

        [HttpGet("{id:length(24)}", Name = "GetBeneficiary")]
        public ActionResult<Beneficiary> Get(string id)
        {
            var beneficiary = _beneficiaryServive.Get(id);
            if (beneficiary is null) return NotFound();
            return beneficiary;
        }

        [HttpPost]
        public ActionResult<Beneficiary> Create(Beneficiary beneficiary)
        {
            ValidationResult result = _validations.Validate(beneficiary);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors) System.Console.WriteLine(err.PropertyName + ": " + err.ErrorMessage);
                return BadRequest();
            }
            _beneficiaryServive.Create(beneficiary);
            return CreatedAtRoute("GetBeneficiary", new { id = beneficiary.Id }, beneficiary);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, Beneficiary beneficiary)
        {
            ValidationResult result = _validations.Validate(beneficiary);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors) System.Console.WriteLine(err.PropertyName + ": " + err.ErrorMessage);
                return BadRequest();
            }
            var oldBeneficiary = _beneficiaryServive.Get(id);
            if (oldBeneficiary is null) return NotFound();
            _beneficiaryServive.Update(id, beneficiary);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var oldBeneficiary = _beneficiaryServive.Get(id);
            if (oldBeneficiary is null) return NotFound();
            _beneficiaryServive.Remove(oldBeneficiary.Id);
            return NoContent();
        }
    }
}
