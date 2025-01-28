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
using CaptchaMvc.HtmlHelpers;
using amca.BusinessLogic;

namespace amca.Controllers
{
    public class VisaSubServicesController : Controller
    {
        public String ServiceModel;
        // GET: VisaSubServices
        public ActionResult Index()
        {
            return View();
        }

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

            //Select Visa Type
          
            PL.OpCode = 20;
            ServiceModelD.returnTable(PL);
            ViewBag.VisaList = ToSelectList(PL.dt, "VisaType", "VisaType");

            //Select Country
            PL.OpCode = 13;
            ServiceModelD.returnTable(PL);
            ViewBag.Nationality = ToSelectList(PL.dt, "Autoid", "CountryName");
        }
        //[HttpGet]
        //public ActionResult VisaQuery()
        //{
        //    BindDropDown();
        //    return View();
        //}
        public DataTable GetAllBlogs()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetBlogsByID(null, null, ServiceModel);
            DataTable dt = ds.Tables[0];

            return dt;
        }
        [HttpGet]
        public ActionResult VisaQuery()
        {
            BindDropDown();
            ServiceModel = "Visa";
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
            //           BlogContent = Convert.ToString(row["BlogContent"]),
            //           Designation = Convert.ToString(row["Designation"])

            //       }

            //       );

            //}
            //ViewData["InsightModel"] = datatable;
            ViewData["BlogServiceModel"] = BlogSearch(9);
            return View();
        }

        [HttpPost]
        public ActionResult VisaQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber,string VisaType, string Catagories, string University,
            string Specialization,string SchoolName, string EmailId, string Service, string AboutAMCA, Validation model , string SubServiceId)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ServiceModel PL = new ServiceModel();
                PL.OpCode = 5;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.ConcernPerson = ConcernPerson;
                if (CompanyName !="")
                {
                    PL.CompanyName = CompanyName;
                }
                else
                {
                    PL.CompanyName = ConcernPerson;
                }
                PL.TradeLicenseAuthority = TradeLicenseAuthority;
                
                PL.CountryCodeContact = CountryCodeContact;
                PL.ContactNumber = ContactNumber;
                PL.EmailId = EmailId;
                PL.Service = Service;
                PL.AboutAMCA = AboutAMCA;
                PL.VisaType = VisaType;
                PL.Category = Catagories;
                PL.University = University;
                PL.Specialization = Specialization;
                PL.SchoolName = SchoolName;
                PL.LeadDataType = LeadDataType;
                PL.ResearchMode = model.University;
                PL.Specialization = Specialization;
                PL.Country = model.country;
                PL.Emirate = model.Emirate;
                PL.ServiceType = model.SubServiceId;

                ServiceModelD.returnTable(PL);

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Marketing Team,</p>";
                body += "<p style='margin-top: 2px'>Please assign the following lead to one of the BDs:</p>";
                body += " <table width='600' border='1' cellpadding='5' cellspacing='0'> " +
                    "<tr> <td> <strong> Contact Person </strong></td> <td>" + ConcernPerson + " </td></tr>" +
                    "<tr> <td> <strong> Contact Number/s </strong></td> <td>" + CountryCodeContact + " " + ContactNumber + " </td></tr>" +
                    "<tr> <td> <strong> Email Id </strong></td> <td>" + EmailId + " </td></tr> " +
                    "<tr> <td> <strong> VisaType </strong></td> <td>" + VisaType + " </td></tr>" +
                    "<tr> <td> <strong> Catagories </strong></td> <td>" + Catagories + " </td></tr>" +
                    "<tr> <td> <strong> Company Name </strong></td> <td>" + CompanyName + " </td></tr>" +
                    "<tr> <td> <strong> Trade License Authority </strong></td> <td>" + TradeLicenseAuthority + " </td></tr>" +
                    "<tr> <td> <strong> University </strong></td> <td>" + University + " </td></tr>" +
                    "<tr> <td> <strong> Specialization </strong></td> <td>" + Specialization + " </td></tr>" +
                    "<tr> <td> <strong> SchoolName </strong></td> <td>" + SchoolName + " </td></tr>" +
                    "<tr> <td> <strong> Service </strong></td> <td>" + Service + " </td></tr>" +
                    "<tr> <td> <strong> Where did you hear about AMCA? </strong></td> <td>" + AboutAMCA + " </td></tr>" +
                  
                    "<tr> <td> <strong> Website </strong></td> <td> amca.ae </td></tr>" +
                    "</table>";
                body += "<p>Regards,<br>AMCA</p>";
                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                // var msg = "";
                return RedirectToAction("Success", "Page");
            }   
            else
            {
                ViewData["BlogServiceModel"] = BlogSearch(9);

                var txtPageName = Session["txtPageName"].ToString(); ;
                var txtPageID = "";
                if (!string.IsNullOrEmpty(Request["txtPageID"]))
                {
                    txtPageID = Request["txtPageID"].ToString();
                }
                BindDropDown();
                ViewBag.ErrMessage = "Error: captcha is not valid.";
                ViewData["BlogServiceModel"] = BlogSearch(9);

                ServiceModel PL = new ServiceModel();

                if (txtPageName == "GoldenVisa")
                {
                    PL.OpCode = 33;
                    ServiceModelD.returnTable(PL);
                    ViewBag.GoldenVisaServices = ToSelectList(PL.dt, "Id", "Name");
                }

                if (txtPageName == "GreenVisa")
                {
                    PL.OpCode = 34;
                    ServiceModelD.returnTable(PL);
                    ViewBag.GreenVisaServices = ToSelectList(PL.dt, "Id", "Name");
                }

                if (txtPageName == "RemoteWorkVisa")
                {
                    PL.OpCode = 36;
                    ServiceModelD.returnTable(PL);
                    ViewBag.RemoteVisaServices = ToSelectList(PL.dt, "Id", "Name");
                }

                if (txtPageName == "EntryPermitsVisa")
                {
                    PL.OpCode = 35;
                    ServiceModelD.returnTable(PL);
                    ViewBag.EntryVisaServices = ToSelectList(PL.dt, "Id", "Name");
                }

                if (txtPageName == "RetirementVisa")
                {
                    PL.OpCode = 37;
                    ServiceModelD.returnTable(PL);
                    ViewBag.RetirementVisaServices = ToSelectList(PL.dt, "Id", "Name");
                }

                return View(txtPageName);
            }
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

        public ActionResult TenYearsVisa()
        {
            Session["txtPageName"] = "TenYearsVisa";
            return VisaQuery();
        }

        [Route("uae-golden-visa")]
        public ActionResult GoldenVisa()
        {
            // Select GOLDEN VISA Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 33;
            ServiceModelD.returnTable(PL);
            ViewBag.GoldenVisaServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "GoldenVisa";
            return VisaQuery();
        }
        //[Route("uae-silver-visa")]
        //public ActionResult SilverVisa()
        //{
        //    Session["txtPageName"] = "SilverVisa";
        //    return VisaQuery();
        //}
        [Route("uae-green-visa")]
        public ActionResult GreenVisa()
        { 
            // Select GREEN VISA Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 34;
            ServiceModelD.returnTable(PL);
            ViewBag.GreenVisaServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "GreenVisa";
            return VisaQuery();
        }
        //[Route("business-visa-uae")]
        //public ActionResult BusinessVisa()
        //{
        //    Session["txtPageName"] = "BusinessVisa";
        //    return VisaQuery();
        //}
        
        [Route("uae-remote-working-visa")]
        public ActionResult RemoteWorkVisa()
        {
            // Select REMOTE WORK VISA Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 36;
            ServiceModelD.returnTable(PL);
            ViewBag.RemoteVisaServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "RemoteWorkVisa";
            return VisaQuery();
        }

        //[Route("tourist-visa-uae")]
        //public ActionResult TouristVisa()
        //{
        //    Session["txtPageName"] = "TouristVisa";
        //    return VisaQuery();
        //}
        [Route("entry-permits-visa-uae")]
        public ActionResult EntryPermitsVisa()
        {
            // Select Entry VISA Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 35;
            ServiceModelD.returnTable(PL);
            ViewBag.EntryVisaServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "EntryPermitsVisa";
            return VisaQuery();
        }
        [Route("retirement-visa-uae")]
        public ActionResult RetirementVisa()
        {
            // Select Entry VISA Services
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 37;
            ServiceModelD.returnTable(PL);
            ViewBag.RetirementVisaServices = ToSelectList(PL.dt, "Id", "Name");

            Session["txtPageName"] = "RetirementVisa";
            return VisaQuery();
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