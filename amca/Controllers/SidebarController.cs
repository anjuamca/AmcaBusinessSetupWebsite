using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using amca.Models;

namespace amca.Controllers
{
    public class SidebarController : Controller
    { 
        // GET: Sidebar
        //[Route("error")]
        public ActionResult Index()
        {
            ViewBag.result = "";
            return View();
        }
        [HttpPost]
        public ActionResult Index(string AutoId, string Username, string BlogTitle, string Useremail, string Userwebsite, string Usercomment, Validation model)
        {
            ServiceModel PL = new ServiceModel();
            PL.OpCode = 11;
            PL.AutoId = Convert.ToInt32(AutoId);
            PL.Username = Username;
            PL.BlogTitle = BlogTitle;
            PL.Usercomment = Usercomment;
            ServiceModelD.returnTable(PL);
            return RedirectToAction("BlogThankyou", "Page");
        }
    }
}