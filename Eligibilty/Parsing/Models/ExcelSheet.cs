using Eligibilty.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Models
{
    public class ExcelSheet
    {
        public Version Version { get; set; }
        public List<Beneficiary> Beneficiaries { get; set; }
        public List<Policy> Policies { get; set; }
        public List<Claim> Claims { get; set; }
    }
}
