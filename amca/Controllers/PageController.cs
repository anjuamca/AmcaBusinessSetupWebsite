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
using System.Web.Services;

namespace amca.Controllers
{
    public class PageController : Controller
    {
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

            //select service
            PL.OpCode = 22;
            ServiceModelD.returnTable(PL);
            ViewBag.ServiceMain = ToSelectList(PL.dt, "AutoID", "Name");

            //Select Services
            PL.OpCode = 24;
            ServiceModelD.returnTable(PL);
            ViewBag.AllServices = ToSelectList(PL.dt, "Id", "Name");

            //Select Services
            PL.OpCode = 65;
            ServiceModelD.returnTable(PL);
            ViewBag.AmcaAeServices = ToSelectList(PL.dt, "AutoID", "Name");
        }
        
        [HttpGet]
        public ActionResult PageQuery()
        {
            BindDropDown();
            ViewData["InsightModel"] = BindInsight();
            return View();
        }
        [HttpPost] 
        public ActionResult PageQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model, string SubServiceId)
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
                PL.LeadDataType = LeadDataType;
                PL.ServiceType = SubServiceId; //new Md owais ansari
                ServiceModelD.returnTable(PL);

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Customer Support,</p>";
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
                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                // var msg = "";
                if(msg == "Successfull")
                {
                    return RedirectToAction("Success", "Page");
                }
                else
                {
                    return RedirectToAction("Success", "Page");
                }
            }
            else
            {
                ViewData["InsightModel"] = BindInsight();
                //GetAllBlogs();
                GetAllBlogsHome();
                BindDropDown();

                //get service
                ServiceModel PL = new ServiceModel();
                PL.OpCode = 7;
                ServiceModelD.returnTable(PL);
                ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");

                var txtPageName = Session["txtPageName"].ToString();
                var txtPageID = "";
                if (!string.IsNullOrEmpty(Request["txtPageID"]))
                {
                    txtPageID = Request["txtPageID"].ToString();
                }

                ViewBag.ErrMessage = "Error: captcha is not valid.";
                return View(txtPageName);

            }
        }
        [HttpPost]
        public ActionResult RequestProposalInsert(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model)
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
                PL.LeadDataType = LeadDataType;
                ServiceModelD.returnTable(PL);

                //Sending Mail
                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Customer Support,</p>";
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
                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                //var msg = "";
                if (msg == "Successfull")
                {
                    return RedirectToAction("Success", "Page");
                }
                else
                {
                    return RedirectToAction("Success", "Page");
                }
            }
            else
            {
                BindDropDown();
                //get service
                ServiceModel PL = new ServiceModel();
                PL.OpCode = 7;
                ServiceModelD.returnTable(PL);
                ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");
                ViewBag.ErrMessage = "Error: captcha is not valid.";
                return View("RequestProposal");
            }
        }
        [HttpPost]
        public ActionResult ContactUsInsert(string AutoId, string NameContact, string EmailContact, string CountryCodeContact, string MobileContact, string MessageContact, Validation model)
        {
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ServiceModel PL = new ServiceModel();
                PL.OpCode = 8;
                PL.AutoId = Convert.ToInt32(AutoId);
                PL.ConcernPerson = NameContact;
                PL.EmailId = EmailContact;
                PL.CountryCodeContact = CountryCodeContact;
                PL.ContactNumber = MobileContact;
                PL.MessageContact = MessageContact;
                ServiceModelD.returnTable(PL);

                //Sending Mail

                var body = "";
                body += "<p style='margin-top: 0px'>Dear AMCA Team,</p>";
                body += "<p style='margin-top: 2px'>Please Contact the following details</p>";
                body += " <table width='300' border='1' cellpadding='5' cellspacing='0'> <tr> <td> <strong> Name </strong></td> <td>" + NameContact + " </td></tr> <tr> <td> <strong> Email Id </strong></td> <td>" + EmailContact + " </td></tr> " +
                    "<tr> <td> <strong> Contact Details </strong></td> <td>" + CountryCodeContact + " " + MobileContact + " </td></tr>" +
                    "<tr> <td> <strong> Message </strong></td> <td>" + MessageContact + " </td></tr>" +
                    "</table>";

                var msg = SendMail("notification@amcaauditing.com", "4J7UwO5p2VDzG8Nq", "crm@amcaauditing.com", "mohammad@amcaauditing.com,md@amcaauditing.com", "", "Assign Lead to BD", body, "", 465, true);
                //var msg = "";
                if (msg == "Successfull")
                {
                    return RedirectToAction("Success", "Page");
                }
                else
                {
                    return RedirectToAction("Success", "Page");
                }
            }
            else
            {
                BindDropDown();
                ViewBag.ErrMessage = "Error: captcha is not valid.";
                return View("ContactUs");
            }
        }

        public ActionResult Index()
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 7;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");
            Session["txtPageName"] = "Index";
            return PageQuery();
        }
        public DataTable GetAllBlogs()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetAllBlogs();
            DataTable dt = ds.Tables[0];
            
            return dt;
        }
        public DataTable GetAllBlogsHome()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetAllBlogsHome("Home");
            DataTable dt = ds.Tables[0];

            return dt;
        }

        
        [HttpGet]
        public ActionResult ContactUsInsert()
        {
            //Select Country Code
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 2;
            ServiceModelD.returnTable(PL);
            ViewBag.CountryList = ToSelectList(PL.dt, "CountryISDCode", "CountryName");
            Session["txtPageName"] = "Index";
            return View();
        }

        
        

        [Route("contact-us")]
        public ActionResult ContactUs()
        {
            return ContactUsInsert();
        }
        [Route("about-us")]
        public ActionResult AboutUs()
        {
            return View();
        }
        [Route("industries")]
        public ActionResult Industries()
        {
            return View();
        }
        [Route("jobs-in-uae")]
        public ActionResult AuditFirmsInDubaiCareers()
        {
            return View();
        }

        [Route("TermsandConditions")]
        public ActionResult TermsandConditions()
        {
            return View();
        }
        public List<InsightModel> BindInsight() {
            DataTable dt = GetAllBlogsHome();

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
        public ActionResult BlogSearch(int? ServiceType)
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
            return PartialView("_BlogPartialData", datatable);
        }
        [Route("insights")]
        public ActionResult Insights()
         {
            BindDropDown();
            return View();
            //Insight ins = new Insight();
            //DataSet ds = ins.GetAllBlogs();
            //InsightModel insMod = new InsightModel();
            //DataTable dt = ds.Tables[0];

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
            //           Designation = Convert.ToString(row["Designation"]),
            //           pageUrlText = Convert.ToString(row["pageUrlText"])

            //       }


            //       );

            //}
            //return View(datatable);

        }

        
        [Route("disclaimer")]
        public ActionResult Disclaimer()
        {
            return View();
        }
        [Route("privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        [Route("thankyou")]
        public ActionResult Success()
        {
            return View();
        }
        [Route("application-submitted")]
        public ActionResult ThankyouCareer()
        {
            return View();
        }
        [Route("thanks-for-comments")]
        public ActionResult BlogThankyou()
        {
            return View();
        }
        [Route("guest")]
        public ActionResult Guest()
        {
            return View();
        }
        [Route("request-proposal")]
        public ActionResult RequestProposal()
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 7;
            ServiceModelD.returnTable(PL);
            ViewBag.Service = ToSelectList(PL.dt, "ServiceName", "ServiceName");
            Session["txtPageName"] = "RequestProposal";
            return PageQuery();
        }
        public JsonResult GetSubServices(int Id)
        {
            string jsondata = "";
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 25;
            PL.AutoId = Id;
            ServiceModelD.returnTable(PL);
            jsondata = JSONfromDT(PL.dt);
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }
        public static string JSONfromDT(DataTable dataTable)
        {
            try
            {
                System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> row;
                foreach (DataRow dr in dataTable.Rows)
                {
                    row = new Dictionary<string, object>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        row.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(row);
                }
                return serializer.Serialize(rows);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}