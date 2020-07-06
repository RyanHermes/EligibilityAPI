using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Eligibilty.API.Models
{
    public class Policy
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; } = new List<Beneficiary>();
    }
}
