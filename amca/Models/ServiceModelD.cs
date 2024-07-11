using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace amca.Models
{ 
    public class ServiceModelD
    {
        public ServiceModelD()
        {

        }
        public static void returnTable(ServiceModel PL)
        {
            try
            {
                SQLConnectivity SC = new SQLConnectivity();
                SqlCommand sqlCmd = new SqlCommand("AllServices", SC.SqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.Parameters.Add("@AutoId", SqlDbType.Int).Value = PL.AutoId;
                sqlCmd.Parameters.Add("@OpCode", SqlDbType.Int).Value = PL.OpCode;
                sqlCmd.Parameters.Add("@ServiceType", SqlDbType.Int).Value = PL.ServiceType;
                sqlCmd.Parameters.Add("@CompanyName", SqlDbType.VarChar).Value = PL.CompanyName;
                sqlCmd.Parameters.Add("@ConcernPerson", SqlDbType.VarChar).Value = PL.ConcernPerson;
                sqlCmd.Parameters.Add("@CountryCodeContact", SqlDbType.VarChar).Value = PL.CountryCodeContact;
                sqlCmd.Parameters.Add("@ContactNumber", SqlDbType.VarChar).Value = PL.ContactNumber;
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar).Value = PL.EmailId;
                sqlCmd.Parameters.Add("@TradeLicenseAuthority", SqlDbType.VarChar).Value = PL.TradeLicenseAuthority;
                sqlCmd.Parameters.Add("@Service", SqlDbType.VarChar).Value = PL.Service;
                sqlCmd.Parameters.Add("@AboutAMCA", SqlDbType.VarChar).Value = PL.AboutAMCA;
                sqlCmd.Parameters.Add("@MessageContact", SqlDbType.VarChar).Value = PL.MessageContact;
                sqlCmd.Parameters.Add("@BlogTitle", SqlDbType.VarChar).Value = PL.BlogTitle;
                sqlCmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = PL.Username;
                sqlCmd.Parameters.Add("@Usercomment", SqlDbType.VarChar).Value = PL.Usercomment;

                sqlCmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = PL.Category;
                sqlCmd.Parameters.Add("@Country", SqlDbType.VarChar).Value = PL.Country;
                sqlCmd.Parameters.Add("@SchoolName", SqlDbType.VarChar).Value = PL.SchoolName;
                sqlCmd.Parameters.Add("@ResearchMode", SqlDbType.VarChar).Value = PL.ResearchMode;
                sqlCmd.Parameters.Add("@Specialization", SqlDbType.VarChar).Value = PL.Specialization;
                sqlCmd.Parameters.Add("@Emirate", SqlDbType.VarChar).Value = PL.Emirate;
                sqlCmd.Parameters.Add("@LeadDataType", SqlDbType.VarChar).Value = PL.LeadDataType;
                sqlCmd.Parameters.AddWithValue("websiteID", 1);
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                PL.dt = new DataTable();
                sqlAdp.Fill(PL.dt);
            }
            catch(Exception ex)
            {
                
            }
        }
    }
}