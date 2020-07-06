using Eligibilty.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Eligibilty.API.Services
{
    public class ClaimService
    {
        private readonly IMongoCollection<Claim> _claims;

        public ClaimService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _claims = database.GetCollection<Claim>(settings.ClaimsCollectionName);
        }


        public List<Claim> Get() => _claims.Find(claim => true).ToList();
        public Claim Get(string id) => _claims.Find(claim => claim.Id == id).FirstOrDefault();
        public Claim Create(Claim claim)
        {
            _claims.InsertOne(claim);
            return claim;
        }
        public void Update(string id, Claim claim) => _claims.ReplaceOne(oldclaim => oldclaim.Id == id, claim);
        public void Remove(string id) => _claims.DeleteOne(claim => claim.Id == id);
        public void Remove(Claim claim) => _claims.DeleteOne(oldClaim => oldClaim.Id == claim.Id);
    }
}
