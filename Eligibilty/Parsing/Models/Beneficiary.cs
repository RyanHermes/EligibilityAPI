using System;

namespace Eligibilty.API.Models
{
    public class Beneficiary
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string RowIdentifier { get; set; }

    }
}
