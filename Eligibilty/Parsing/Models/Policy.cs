using System;
using System.Collections.Generic;

namespace Eligibilty.API.Models
{
    public class Policy
    {
        public string Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
