using System;

namespace Eligibilty.API.Models
{
    public class Claim
    {
        public string Id { get; set; }
        public string PolicyNo { get; set; }
        public double ClaimedAmount { get; set; }
        public DateTime IncurredDate { get; set; }
    }
}
