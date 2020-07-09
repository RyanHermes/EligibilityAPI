using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eligibilty.API.Models
{
    public class Beneficiary : Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public Relationship Relationship { get; set; }

        public Gender Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }

    }
}
