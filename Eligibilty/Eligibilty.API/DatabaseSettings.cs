using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibilty.API
{
    public interface IDatabaseSettings
    {
        string BeneficiariesCollectionName { get; set; }
        string ClaimsCollectionName { get; set; }
        string PoliciesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class DatabaseSettings : IDatabaseSettings
    {
        public string BeneficiariesCollectionName { get; set; }
        public string ClaimsCollectionName { get; set; }
        public string PoliciesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

}
