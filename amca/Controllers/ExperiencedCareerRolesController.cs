using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using amca.Models;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI.WebControls;
using CaptchaMvc.HtmlHelpers;

namespace amca.Controllers
{
    public class ExperiencedCareerRolesController : Controller
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


        [HttpPost] 
        public ActionResult SubmitJob(ExperienceDesignation job, string JobHeading)
        {
            TempData["Job"] = job;
            Session["txtJob"] = job;
            return RedirectToAction("ApplyJobs", "ExperiencedCareerRoles");
        }
        [HttpGet]
        [Route("amca-jobs")]
        public ActionResult ApplyJobs()
        {
            ExperienceDesignation job = Session["txtJob"] as ExperienceDesignation;

            //Select About Job
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 12;
            ServiceModelD.returnTable(PL);
            ViewBag.AboutJob = ToSelectList(PL.dt, "Id", "Name");

            //Select Country
            PL.OpCode = 13;
            ServiceModelD.returnTable(PL);
            ViewBag.Nationality = ToSelectList(PL.dt, "Autoid", "CountryName");
            //Select Country

            PL.OpCode = 13;
            ServiceModelD.returnTable(PL);
            ViewBag.CurrentLocation = ToSelectList(PL.dt, "Autoid", "CountryName");

            PL.OpCode = 18;
            ServiceModelD.returnTable(PL);
            ViewBag.JobTitle = ToSelectList(PL.dt, "Id", "JobPosition");

            PL.OpCode = 19;
            ServiceModelD.returnTable(PL);
            ViewBag.CountryCodeContact = ToSelectList(PL.dt, "CountryISDCode", "CountryName");

            return View(job);
        }
        

        [HttpPost]
        public ActionResult RequestJob(string AboutJob, string JobPosition, string JobTitle,string AutoId, string CandidateName, string EmailId, string MobileNo, string Nationality, DateTime DoB, string Gender, string MaritalStatus, string Experience, string UAEExperience, string LastDesignation, string CurrentLocation, string SalaryExpectation, string NoticePeriod, string VisaType ,string CountryCodeContact, HttpPostedFileBase PostedFile, ExperienceDesignation model)
        {

            if (!this.IsCaptchaValid(errorText: ""))
            {
                ViewBag.ErrMessage = "Captacha is not valid";
                TempData["Job"] = Session["txtJob"].ToString();
                ApplyJobs();
                return View("ApplyJobs");
            }
            else
            {
                try
                {
                    if (PostedFile.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(PostedFile.FileName);
                        string extension = Path.GetExtension(PostedFile.FileName);
                        _FileName = _FileName + DateTime.Now.ToString("yymmddssfff") + extension;
                        string relativeFileName = "CandidateCV/" + _FileName;
                        _FileName = Path.Combine(Server.MapPath("~/CandidateCV/"), _FileName);
                        PostedFile.SaveAs(_FileName);
                        Candidate_Data_PL PL = new Candidate_Data_PL();
                        PL.OpCode = 1;
                        PL.AboutJob = AboutJob;
                        //PL.AutoId = Convert.ToInt32(AutoId);
                        PL.AutoId = Convert.ToInt32(JobPosition);
                        PL.JobTitle = JobTitle;
                        PL.JobPosition = JobTitle;
                        PL.CandidateName = CandidateName;
                        PL.EmailId = EmailId;
                        PL.MobileNo = MobileNo;
                        PL.Nationality = Nationality;
                        PL.DoB = DoB;
                        PL.Gender = Gender;
                        PL.MaritalStatus = MaritalStatus;
                        PL.Experience = Experience;
                        PL.UAEExperience = UAEExperience;
                        PL.LastDesignation = LastDesignation;
                        PL.CurrentLocation = CurrentLocation;
                        PL.SalaryExpectation = SalaryExpectation;
                        PL.NoticePeriod = NoticePeriod;
                        PL.VisaType = VisaType;
                        PL.CV = relativeFileName;
                        PL.CountryCode = CountryCodeContact;
                        Candidate_Data_DL.returnTable(PL);
                        if (PL.dt.Rows.Count > 0)
                        {
                            string Autoid = PL.dt.Rows[0]["Autoid"].ToString();
                            PL.OpCode = 2;
                            PL.AutoId = Convert.ToInt32(Autoid);
                            Candidate_Data_DL.returnTable(PL);

                            /// Select Candidate Id
                            PL.OpCode = 3;
                            PL.AutoId = Convert.ToInt32(Autoid);
                            Candidate_Data_DL.returnTable(PL);
                            DataTable dt = PL.dt;
                            TempData["Id"] = PL.dt.Rows[0]["CandidateId"].ToString();
                            TempData["Name"] = PL.dt.Rows[0]["CandidateName"].ToString();
                            TempData["ApplyJob"] = PL.dt.Rows[0]["JobTitle"].ToString();

                            //Sending Mail
                            var mail = "<p>Dear HR,</p>";
                            mail += "<p>Please go thru the link: <a href='https://portal.amca.ae'>AMCA Portal</a> and find the below candidate details.</p>";

                            mail += "Candidate Id: " + PL.dt.Rows[0]["CandidateId"].ToString();
                            mail += "<br>Candidate Name: " + PL.dt.Rows[0]["CandidateName"].ToString();
                            mail += "<br>Job Position: " + PL.dt.Rows[0]["JobTitle"].ToString();
                            mail += "<p>Thank you!</p>";
                            mail += "<p>Regards,<br>AMCA</p>";

                           //var msg = SendMail("careers@amca.ae", "Careers@1244.ae", "hr@amcaauditing.com", "info@amcaauditing.com", "", "Job Application", mail, "win1.server.ae", 465, true);
                           var msg = "";
                            return RedirectToAction("ThankyouCareer", "Page");
                        }
                    }
                    return RedirectToAction("ThankyouCareer", "Page");
                }
                catch
                {
                    ViewBag.ErrMessage = "Error: captcha is not valid.";
                    ViewBag.Message = "File upload failed!!";
                    return View("ApplyJobs");
                }
            }
        }
        [Route("telesales-jobs-in-dubai")]
        public ActionResult TelesalesExecutive()
        {
            return View();
        }
        [Route("admin-executive-jobs-in-dubai")]
        public ActionResult AdminExecutive()
        {
            return View();
        }
        [Route("content-writer-jobs-in-dubai")]
        public ActionResult ContentWriter()
        {
            return View();
        }
        [Route("digital-marketing-jobs-in-dubai")]
        public ActionResult DigitalMarketingSpecialist()
        {
            return View();
        }
        [Route("hr-executive-jobs-in-dubai")]
        public ActionResult HRExecutive()
        {
            return View();
        }
        [Route("it-coordinator-jobs-in-dubai")]
        public ActionResult ITCoordinator()
        {
            return View();
        }

        [Route("ITSystemAdministrator-jobs-in-dubai")]
        public ActionResult ITSystemAdministrator()
        {
            return View();
        }
        [Route("office-boy-jobs-in-dubai")]
        public ActionResult OfficeBoy()
        {
            return View();
        }
        [Route("marketing-specialist-jobs-in-dubai")]
        public ActionResult MarketingSpecialist()
        {
            return View();
        }
        
        [Route("aspdotnet-developer-jobs-in-dubai")]
        public ActionResult ASPDotNET()
        {
            return View();
        }

        
        [Route("video-editor-jobs-in-dubai")]
        public ActionResult VideoEditor()
        {
            return View();
        }
        [Route("company-formation-specialist-jobs-in-dubai")]
        public ActionResult CompanyFormationSpecialist()
        {
            return View();
        }
        [Route("pro-assistant-jobs-in-dubai")]
        public ActionResult PROAssistant()
        {
            return View();
        }
    }
}