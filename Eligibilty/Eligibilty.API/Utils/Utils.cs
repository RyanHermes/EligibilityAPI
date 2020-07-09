using Eligibilty.API.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Eligibilty.API.Utils
{
    public static class Utils
    {
        private static readonly Dictionary<string, Relationship> RelationshipDic = new Dictionary<string, Relationship>()
        {
            {"Self", Relationship.Self },
            {"Spouse", Relationship.Spouse },
            {"Child", Relationship.Child }
        };

        private static readonly Dictionary<string, Gender> GenderDic = new Dictionary<string, Gender>()
        {
            {"Male", Gender.Male },
            {"Female", Gender.Female }
        };

        private static readonly Dictionary<string, int> nameIndex = new Dictionary<string, int>()
        {
            {"Name", 1 },
            {"Gender", 2 },
            {"Relationship", 3 },
            {"DateOfBirth", 4 },

            {"Id", 1 },
            {"EffectiveDate", 2 },
            {"ExpiryDate", 3 },


            {"PolicyNo", 1 },
            {"ClaimedAmount", 2 },
            {"IncurredDate", 3 },
        };

        public static int GetCurrentAge(this DateTimeOffset dateTimeOffset)
        {
            var currentDate = DateTime.UtcNow;
            int age = currentDate.Year - dateTimeOffset.Year;

            if (currentDate < dateTimeOffset.AddYears(age)) age--;
            return age;
        }

        public static Gender ParseGender(this string gender)
        {
            return !GenderDic.ContainsKey(gender) ? (Gender)(-1) : GenderDic[gender];
        }

        public static Relationship ParseRelationship(this string relationship)
        {
            return !RelationshipDic.ContainsKey(relationship) ? (Relationship)(-1) : RelationshipDic[relationship];
        }

        public static int GetIndex(this string indexName)
        {
            return nameIndex[indexName];
        }
    }
}
