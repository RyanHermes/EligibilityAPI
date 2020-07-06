using Eligibilty.API.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace Eligibilty.API.Services
{
    public class BeneficiaryService
    {
        private readonly IMongoCollection<Beneficiary> _beneficiaries;

        public BeneficiaryService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _beneficiaries = database.GetCollection<Beneficiary>(settings.BeneficiariesCollectionName);
        }

        public List<Beneficiary> Get() => _beneficiaries.Find(beneficiary => true).ToList();
        public Beneficiary Get(string id) => _beneficiaries.Find(beneficiary => beneficiary.Id == id).FirstOrDefault();
        public Beneficiary Create(Beneficiary beneficiary)
        {
            _beneficiaries.InsertOne(beneficiary);
            return beneficiary;
        }
        public void Update(string id, Beneficiary beneficiary) => _beneficiaries.ReplaceOne(oldBeneficiary => oldBeneficiary.Id == id, beneficiary);
        public void Remove(string id) => _beneficiaries.DeleteOne(beneficiary => beneficiary.Id == id);
        public void Remove(Beneficiary beneficiary) => _beneficiaries.DeleteOne(oldBeneficiary => oldBeneficiary.Id == beneficiary.Id);

    }
}
