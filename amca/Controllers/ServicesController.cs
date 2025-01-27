using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using amca.Models;
using System.Configuration;
using System.Net.Mail;
using amca.BusinessLogic;
using CaptchaMvc.HtmlHelpers;

namespace amca.Controllers
{
    public class ServicesController : Controller
    {
        public String ServiceModel;

        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        { 
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }
            return new SelectList(list, "Value", "Text");
        }

        [NonAction]
        public string SendMail(string FromMailID, string fromEmailPassword, string ToMailID, string CC, string BCC, string subject, string body, string servername, int PortNo, bool ssl)
        {
            servername = "smtp-relay.sendinblue.com"; PortNo = 587; ssl = false; FromMailID = "notification@amca.ae"; fromEmailPassword = "4J7UwO5p2VDzG8Nq";

            string msg = string.Empty;
            MailMessage MailMsg = new MailMessage();
            try
            {
                if (ToMailID.EndsWith(","))
                {
                    ToMailID = ToMailID.Substring(0, ToMailID.Length - 1);
                }
                if (CC.EndsWith(","))
                {
                    CC = CC.Substring(0, CC.Length - 1);

                }
                if (BCC.EndsWith(","))
                {
                    BCC = BCC.Substring(0, BCC.Length - 1);

                }

                MailMsg.To.Add(ToMailID);
                if (CC != "")
                {
                    MailMsg.CC.Add(CC);
                }
                if (BCC != "")
                {
                    MailMsg.Bcc.Add(BCC);
                }

                //=================
                MailMsg.From = new MailAddress(FromMailID);
                MailMsg.Subject = subject;
                MailMsg.IsBodyHtml = true;
                MailMsg.Body = body;

                SmtpClient tempsmtp = new SmtpClient();
                tempsmtp.Host = servername;
                tempsmtp.Port = PortNo;
                tempsmtp.Credentials = new System.Net.NetworkCredential(FromMailID, fromEmailPassword);
                tempsmtp.EnableSsl = ssl;

                tempsmtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                tempsmtp.Send(MailMsg);
                msg = "Successful";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        public List<InsightModel> BlogSearch(int? ServiceType)
        {
            Insight ins = new Insight();
            //DataSet ds = ins.GetAllBlogs();
            DataSet ds = ins.GetAllBlogsBySearch(ServiceType);
            InsightModel insMod = new InsightModel();
            DataTable dt = ds.Tables[0];
            List<InsightModel> datatable = new List<InsightModel>();
            foreach (DataRow row in dt.Rows)
            {

                datatable.Add(

                   new InsightModel
                   {

                       BlogID = (int)(row["AutoID"]),
                       BlogTitle = Convert.ToString(row["BlogTitle"]),
                       PageTitle = Convert.ToString(row["PageTitle"]),
                       Keyword = Convert.ToString(row["Keyword"]),
                       Description = Convert.ToString(row["Description"]),
                       ImageURL = Convert.ToString(row["ImageURL"]),
                       IsActive = Convert.ToString(row["IsActive"]),
                       CreatedOn = Convert.ToString(row["CreatedOn"]),
                       BlogContent = Convert.ToString(row["BlogContent"]),
                       Designation = Convert.ToString(row["Designation"]),
                       pageUrlText = Convert.ToString(row["pageUrlText"])

                   }


                   );

            }
            return datatable;
        }
        
        public ActionResult Index()
        {
            return View();
        }
        public void bindServiceDdl(string ServiceID)
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = ServiceID;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");

        }
        [Route("business-setup-services-dubai")]
        public ActionResult BusinessSetUp()
        {
            //string BusinessSetUpService = "1";
            string BusinessSetUpService = "9";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 66;
            PL.ServiceType = BusinessSetUpService;
            ServiceModelD.returnTable(PL);
            //ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName"); 
            ViewBag.BusinessSetupServices = ToSelectList(PL.dt, "Id", "SubServiceName"); 
            Session["txtPageName"] = "BusinessSetUp";
            ViewData["BlogServiceModel"] = BlogSearch(5);
            Session["BlogSearch"] = 5;
            return ServiceQuery();
        }
        [Route("advisory-services-in-dubai-uae")]
        public ActionResult AdvisoryServices()
        {
            //string AdvisoryServices = "2";
            string AdvisoryServices = "4";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 66;
            PL.ServiceType = AdvisoryServices;
            ServiceModelD.returnTable(PL);
            //ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            ViewBag.AdvisoryService = ToSelectList(PL.dt, "Id", "SubServiceName");
            Session["txtPageName"] = "AdvisoryServices";
            ViewData["BlogServiceModel"] = BlogSearch(4);
            Session["BlogSearch"] = 4;
            return ServiceQuery();
        }
        [Route("pro-services-in-dubai")]
        public ActionResult PROServices()
        {
            string PROServices = "3";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 3;
            PL.ServiceType = PROServices;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");

            // PRO Service List
            PL.OpCode = 38;
            ServiceModelD.returnTable(PL);
            ViewBag.PROServices = ToSelectList(PL.dt, "Id", "Name");


            Session["txtPageName"] = "PROServices";
            ViewData["BlogServiceModel"] = BlogSearch(9);
            Session["BlogSearch"] = 9;
            return ServiceQuery();
        }
       
        [Route("economic-substance-regulations")]
        public ActionResult ESRService()
        {
            string esrService = "4"; 
            ServiceModel = "esr";
            bindServiceDdl(esrService);

            ServiceModel PL = new ServiceModel();
            PL.OpCode = 27;
            ServiceModelD.returnTable(PL); 
            ViewBag.EsrServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "ESRService";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }
        [Route("the-goaml-system-and-automatic-reporting-system")]
        public ActionResult goAML()
        {
            string goamlsystem = "6";
            ServiceModel PL = new ServiceModel();

            // Select goAML Services
            PL.OpCode = 30;
            ServiceModelD.returnTable(PL);
            ViewBag.goAMLServices = ToSelectList(PL.dt, "Id", "Name");
            /////////////////////////////////////////////////////////////
            ///
            PL.OpCode = 3;
            PL.ServiceType = goamlsystem;
            ServiceModelD.returnTable(PL);            
            ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName"); 
            ServiceModel = "goAML";
            Session["txtPageName"] = "goAML";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }
        [Route("parent-company-officers-in-dmcc")]
        public ActionResult ParentCompany()
        {
            // Select DMCC Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 31;
            ServiceModelD.returnTable(PL);
            ViewBag.DMCCServices = ToSelectList(PL.dt, "Id", "Name");
            //////////////////////////////////////////////////////////////////
            Session["txtPageName"] = "ParentCompany";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }
        [Route("regulatory-compliance-and-reporting-in-the-uae")]
        public ActionResult RegulatoryComplianceReporting()
        {
            //string regulatorycompliancereporting = "7";
            string regulatorycompliancereporting = "14";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 66;
            PL.ServiceType = regulatorycompliancereporting;
            ServiceModelD.returnTable(PL);
            //ViewBag.Service = ToSelectList(PL.dt, "SubServiceName", "SubServiceName");
            ViewBag.RegulatoryComplianceReporting = ToSelectList(PL.dt, "Id", "SubServiceName");
            Session["txtPageName"] = "RegulatoryComplianceReporting";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }
        [Route("the-uae-country-by-country-reporting-cbcr")]
        public ActionResult cbcr()
        {
            // Select CBCR Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 29;
            ServiceModelD.returnTable(PL);
            ViewBag.CBCRServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "cbcr";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }

        [Route("ultimate-beneficial-owner")]
        public ActionResult UltimateBeneficialOwner()
        {
            // Select UBO Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 28;
            ServiceModelD.returnTable(PL);
            ViewBag.UBOServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "UltimateBeneficialOwner";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }

        [Route("virtual-assets-regulatory-authority")]
        public ActionResult VARAService() // Virtual Assets Regulatory Authority
        {
            // Select UBO Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 73;
            ServiceModelD.returnTable(PL);
            ViewBag.VARAService = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "VARAService";
            ViewData["BlogServiceModel"] = BlogSearch(7);
            Session["BlogSearch"] = 7;
            return ServiceQuery();
        }
        public void BindDropDown() {
            //Select Authority
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 1;
            ServiceModelD.returnTable(PL);
            ViewBag.TradeLicenseAutList = ToSelectList(PL.dt, "TradeLicenseAuthority", "TradeLicenseAuthority");

            //Select Country Code
            PL.OpCode = 2;
            ServiceModelD.returnTable(PL);
            ViewBag.CountryList = ToSelectList(PL.dt, "CountryISDCode", "CountryName");

            //Select About AMCA
            PL.OpCode = 4;
            ServiceModelD.returnTable(PL);
            ViewBag.AboutAMCA = ToSelectList(PL.dt, "AboutAMCA", "AboutAMCA");
        }

        [HttpGet]
        public ActionResult ServiceQuery()
        {
            BindDropDown();
            
            //DataTable dt = GetAllBlogs();
            //List<InsightModel> datatable = new List<InsightModel>();
            //foreach (DataRow row in dt.Rows)
            //{

            //    datatable.Add(

            //       new InsightModel
            //       {

            //           BlogID = (int)(row["AutoID"]),
            //           BlogTitle = Convert.ToString(row["BlogTitle"]),
            //           PageTitle = Convert.ToString(row["PageTitle"]),
            //           Keyword = Convert.ToString(row["Keyword"]),
            //           Description = Convert.ToString(row["Description"]),
            //           ImageURL = Convert.ToString(row["ImageURL"]),
            //           IsActive = Convert.ToString(row["IsActive"]),
            //           CreatedOn = Convert.ToString(row["CreatedOn"]),
            //           pageUrlText = Convert.ToString(row["pageUrlText"]),
            //           BlogContent = Convert.ToString(row["BlogContent"]),
            //           Designation = Convert.ToString(row["Designation"])

            //       }


            //       );

            //}
            //ViewData["InsightModel"] = datatable;

            return View();
        }
        public DataTable GetAllBlogs()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetBlogsByID(null, null, ServiceModel);
            DataTable dt = ds.Tables[0];

            return dt;
        }
        

        [HttpPost]
        public ActionResult ServiceQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA,string Amount, Validation model, string SubServiceId)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {

                ServiceModel PL = new ServiceModel();
                PL.OpCode = 5;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.CompanyName = CompanyName;
                PL.TradeLicenseAuthority = TradeLicenseAuthority;
                PL.ConcernPerson = ConcernPerson;
                PL.CountryCodeContact = CountryCodeContact;
                PL.ContactNumber = ContactNumber;
                PL.EmailId = EmailId;
                PL.Service = Service;
                PL.AboutAMCA = AboutAMCA;
                PL.ServiceName = Amount;
                PL.LeadDataType = LeadDataType;
                PL.ServiceType = SubServiceId;
                ServiceModelD.returnTable(PL);

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Marketing Team,</p>";
                body += "<p style='margin-top: 2px'>Please assign the following lead to one of the BDs:</p>";
                body += " <table width='600' border='1' cellpadding='5' cellspacing='0'> " +
                    "<tr> <td> <strong> Company Name </strong></td> <td>" + CompanyName + " </td></tr>" +
                    "<tr> <td> <strong> Contact Person </strong></td> <td>" + ConcernPerson + " </td></tr>" +
                    "<tr> <td> <strong> Trade License Authority </strong></td> <td>" + TradeLicenseAuthority + " </td></tr>" +
                    "<tr> <td> <strong> Contact Number/s </strong></td> <td>" + CountryCodeContact + " " + ContactNumber + " </td></tr>" +
                    "<tr> <td> <strong> Email Id </strong></td> <td>" + EmailId + " </td></tr> " +
                    "<tr> <td> <strong> Service </strong></td> <td>" + Service + " </td></tr>" +
                    "<tr> <td> <strong> Where did you hear about AMCA? </strong></td> <td>" + AboutAMCA + " </td></tr>" +
                    "<tr> <td> <strong> Website </strong></td> <td> amca.ae </td></tr>" +
                    "</table>";
                body += "<p>Regards,<br>AMCA</p>";
                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com.,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                // var msg = "";
                return RedirectToAction("Success", "Page");
            }
            else {
                int BlogNo = Int32.Parse(Session["BlogSearch"].ToString());
                var txtPageName = Request["txtPageName"].ToString();
                var txtPageID="";
                if (!string.IsNullOrEmpty(Request["txtPageID"]))
                {
                     txtPageID = Request["txtPageID"].ToString();
                }
                BindDropDown();
                ViewData["BlogServiceModel"] = BlogSearch(BlogNo);
                bindServiceDdl(txtPageID);
                ViewBag.ErrMessage = "Error: captcha is not valid.";

                ServiceModel PL = new ServiceModel();
                if (txtPageName == "BusinessSetUp")
                {
                    string BusinessSetUpService = "9";
                    PL.OpCode = 66;
                    PL.ServiceType = BusinessSetUpService;
                    ServiceModelD.returnTable(PL);
                    ViewBag.BusinessSetupServices = ToSelectList(PL.dt, "Id", "SubServiceName");
                }
                if (txtPageName == "AdvisoryServices")
                {
                    string AdvisoryServices = "4";
                    PL.OpCode = 66;
                    PL.ServiceType = AdvisoryServices;
                    ServiceModelD.returnTable(PL);
                    ViewBag.AdvisoryService = ToSelectList(PL.dt, "Id", "SubServiceName");
                }
                if (txtPageName == "PROServices")
                {
                    PL.OpCode = 38;
                    ServiceModelD.returnTable(PL);
                    ViewBag.PROServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "ESRService")
                {
                    PL.OpCode = 27;
                    ServiceModelD.returnTable(PL);
                    ViewBag.EsrServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "goAML")
                {
                    PL.OpCode = 30;
                    ServiceModelD.returnTable(PL);
                    ViewBag.goAMLServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "ParentCompany")
                {
                    PL.OpCode = 31;
                    ServiceModelD.returnTable(PL);
                    ViewBag.DMCCServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "RegulatoryComplianceReporting")
                {
                    string regulatorycompliancereporting = "14";
                    PL.OpCode = 66;
                    PL.ServiceType = regulatorycompliancereporting;
                    ServiceModelD.returnTable(PL);
                    ViewBag.RegulatoryComplianceReporting = ToSelectList(PL.dt, "Id", "SubServiceName");
                }
                if (txtPageName == "cbcr")
                {
                    PL.OpCode = 29;
                    ServiceModelD.returnTable(PL);
                    ViewBag.CBCRServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "UltimateBeneficialOwner")
                {
                    PL.OpCode = 28;
                    ServiceModelD.returnTable(PL);
                    ViewBag.UBOServices = ToSelectList(PL.dt, "Id", "Name");
                }
                if (txtPageName == "VARAService")
                {
                    PL.OpCode = 73;
                    ServiceModelD.returnTable(PL);
                    ViewBag.VARAService = ToSelectList(PL.dt, "Id", "Name");
                }
                return View(txtPageName);
            }
        }
        [HttpPost]
        public ActionResult BlogSidebar(string AutoId, string EmailId, Validation model)
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 21;
            PL.EmailId = EmailId;
            ServiceModelD.returnTable(PL);
            DataTable dt = PL.dt;
            ModelState.Clear();
            if (PL.dt.Rows.Count == 0)
            {
                PL.OpCode = 6;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.EmailId = EmailId;
                ServiceModelD.returnTable(PL);
                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Team,</p>";
                body += "<p style='margin-top: 2px'>He wants our daily Newsletter, please save the below Email id in your Newsletter list.</p>";
                body += " <table width='600' border='1' cellpadding='5' cellspacing='0'> " +
                    "<tr> <td> <strong> Email Id </strong></td> <td>" + EmailId + " </td></tr> " +
                    "</table>";
                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                return Json(new { isok = true, message = "<strong class='text-success'>Thank you! We will back soon.</strong>" });
            }
            else
            {
                return Json(new { isok = false, message = "<strong class='text-danger'>" + EmailId + " : Already exist.</strong>" });
            }

        }
        //End BlogSidebar 
    }
}