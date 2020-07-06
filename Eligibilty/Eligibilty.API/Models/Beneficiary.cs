using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Eligibilty.API.Models
{
    public class Beneficiary
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Relationship Relationship { get; set; }

        [BsonRepresentation(BsonType.String)]
        public Gender Gender { get; set; }

        public DateTimeOffset DateOfBirth { get; set; }

    }
}
