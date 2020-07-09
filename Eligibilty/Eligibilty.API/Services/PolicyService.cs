using Eligibilty.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
namespace Eligibilty.API.Services
{
    public class PolicyService
    {
        private readonly IMongoCollection<Policy> _policies;

        public PolicyService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _policies = database.GetCollection<Policy>(settings.PoliciesCollectionName);
        }

        public List<Policy> Get() => _policies.Find(policy => true).ToList();
        //public Policy Get(string id) => _policies.Find(policy => policy.Id == id).FirstOrDefault();
        public Policy Create(Policy policy)
        {
            _policies.InsertOne(policy);
            //foreach(var beneficiary in policy.Beneficiaries)
            //{
            //    beneficiary.Id = ObjectId.GenerateNewId().ToString();
            //}
            return policy;
        }
        //public void Update(string id, Policy policy) => _policies.ReplaceOne(oldpolicy => oldpolicy.Id == id, policy);
        //public void Remove(string id) => _policies.DeleteOne(policy => policy.Id == id);
        //public void Remove(Policy policy) => _policies.DeleteOne(oldPolicy => oldPolicy.Id == policy.Id);
    }
}
