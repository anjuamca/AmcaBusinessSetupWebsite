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
    public class ESRSubServicesController : Controller
    {
        public String ServiceModel;
        // GET: ESRSubServices 
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
        [HttpGet]
        public ActionResult ESRQuery()
        {
            BindDropDown();
            ViewData["BlogServiceModel"] = BlogSearch(7);
            //ServiceModel = "esr";
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

            return View();
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
        public void BindDropDown()
        { 
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
        public DataTable GetAllBlogs()
        {

            Insight ins = new Insight();
            DataSet ds = ins.GetBlogsByID(null, null, ServiceModel);
            DataTable dt = ds.Tables[0];

            return dt;
        }
        [HttpPost]
        public ActionResult ESRQuery(string LeadDataType, string AutoId, string CompanyName, string TradeLicenseAuthority, string ConcernPerson, string CountryCodeContact, string ContactNumber, string EmailId, string Service, string AboutAMCA, Validation model , string SubServiceId)
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
                if(SubServiceId == "ESR Notification Filing")
                {
                    SubServiceId = "88";
                }
                else if (SubServiceId == "ESR Report")
                {
                    SubServiceId = "121";
                }
                else if (SubServiceId == "ESR Testing")
                {
                    SubServiceId = "120";
                }
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
                //var msg = "";
                return RedirectToAction("Success", "Page");
            }
            else
            {
                ViewData["BlogServiceModel"] = BlogSearch(7);
                var txtPageName = Session["txtPageName"].ToString(); ;
                var txtPageID = "";
                if (!string.IsNullOrEmpty(Request["txtPageID"]))
                {
                    txtPageID = Request["txtPageID"].ToString();
                }
                BindDropDown();
                ViewBag.ErrMessage = "Error: captcha is not valid.";
                return View(txtPageName);
            }
        }

        [Route("esr-notification-filing")]
        public ActionResult ESRNotificationFiling()
        {
            Session["txtPageName"] = "ESRNotificationFiling";
            return ESRQuery();
        }

        [Route("esr-report-in-the-uae")]
        public ActionResult ESRReport()
        {
            Session["txtPageName"] = "ESRReport";
            return ESRQuery();
        }

        [Route("economic-substance-testing")]
        public ActionResult ESRTesting()
        {
            Session["txtPageName"] = "ESRTesting";
            return ESRQuery();
        }
    }
}