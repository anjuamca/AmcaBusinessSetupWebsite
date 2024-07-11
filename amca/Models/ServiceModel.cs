using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace amca.Models
{
    public class ServiceModel
    {
        public int OpCode { get; set; }
        public int ServiceID { get; set; } 
        public string ServiceType { get; set; }
        public int AutoId { get; set; }
        public string ServiceName { get; set; }
        public string CompanyName { get; set; }
        public string TradeLicenseAuthority { get; set; }
        public object ScriptManager { get; set; }
        public string ConcernPerson { get; set; }
        public string CountryCodeContact { get; set; }
        public string ContactNumber { get; set; }
        public string EmailId { get; set; }
        public string Service { get; set; }
        public string AboutAMCA { get; set; }
        public string MessageContact { get; set; }
        public string BlogTitle { get; set; }
        public string Username { get; set; }
        public string Usercomment { get; set; }
        public string exceptionMessage { get; set; }
        public bool isException { get; set; }
        public DataTable dt { get; set; }
        public string VisaType { get; set; }

        public string University { get; set; }

        public string Specialization { get; set; }
        public string SchoolName { get; set; }
        public string Category { get; set; }
        public string ResearchMode { get; set; }
        public string Country { get; set; }
        public string Emirate { get; set; }

        public string  LeadDataType { get; set; }
    }
}