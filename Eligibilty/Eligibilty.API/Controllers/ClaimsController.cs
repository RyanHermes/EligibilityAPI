using Eligibilty.API.Models;
using Eligibilty.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Eligibilty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClaimsController : ControllerBase
    {
        private readonly ClaimService _claimService;

        public ClaimsController(ClaimService claimService)
        {
            _claimService = claimService;
        }

        [HttpGet]
        public ActionResult<List<Claim>> Get() => _claimService.Get();

        [HttpGet("{id:length(24)}", Name = "GetClaims")]
        public ActionResult<Claim> Get(string id)
        {
            var claim = _claimService.Get(id);
            if (claim is null) return NotFound();
            return claim;
        }

        [HttpPost]
        public ActionResult<Claim> Create(Claim claim)
        {
            _claimService.Create(claim);
            return CreatedAtRoute("GetClaims", new { id = claim.Id }, claim);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, Claim claim)
        {
            var oldClaim = _claimService.Get(id);
            if (oldClaim is null) return NotFound();
            _claimService.Update(id, claim);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var oldClaim = _claimService.Get(id);
            if (oldClaim is null) return NotFound();
            _claimService.Remove(oldClaim.Id);
            return NoContent();
        }
    }
}
