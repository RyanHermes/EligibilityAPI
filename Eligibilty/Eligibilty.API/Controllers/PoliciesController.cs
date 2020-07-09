using Eligibilty.API.Models;
using Eligibilty.API.Rules;
using Eligibilty.API.Services;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Eligibilty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly PolicyService _policyService;
        private readonly PolicyValidator _validations;

        public PoliciesController(PolicyService policyService)
        {
            _policyService = policyService;
            _validations = new PolicyValidator();

        }

        [HttpGet]
        public ActionResult<List<Policy>> Get() => _policyService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPolicies")]
        public ActionResult<Policy> Get(string id)
        {
            //var policy = _policyService.Get(id);
            //if (policy is null) return NotFound();
            //return policy;
            return null;
        }

        [HttpGet("{id:length(24)}/beneficiaries")]
        public ActionResult<List<Beneficiary>> GetBeneficiariesForPolicy(string id)
        {
            var beneficiaries = new List<Beneficiary>();
            //var policy = _policyService.Get(id);
            //if (policy is null) return NotFound();
            //foreach (var beneficiary in policy.Beneficiaries) beneficiaries.Add(beneficiary);
            return beneficiaries;
        }

        [HttpPost]
        public ActionResult<Policy> Create(Policy policy)
        {
            ValidationResult result = _validations.Validate(policy);
            if (!result.IsValid)
            {
                foreach(var err in result.Errors)
                {
                    Console.WriteLine(err.PropertyName + ": " + err.ErrorMessage);
                    return BadRequest();
                }
            }
            _policyService.Create(policy);
            //return CreatedAtRoute("GetPolicies", new { id = policy.Id }, policy);
            return null;
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, Policy policy)
        {
            ValidationResult result = _validations.Validate(policy);
            if (!result.IsValid)
            {
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err.PropertyName + ": " + err.ErrorMessage);
                    return BadRequest();
                }
            }
            //var oldPolicy = _policyService.Get(id);
            //if (oldPolicy is null) return NotFound();
            //_policyService.Update(id, policy);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            //var oldPolicy = _policyService.Get(id);
            //if (oldPolicy is null) return NotFound();
            //_policyService.Remove(oldPolicy.Id);
            return NoContent();
        }
    }
}
