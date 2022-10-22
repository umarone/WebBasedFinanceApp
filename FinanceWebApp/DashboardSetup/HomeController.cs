using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.DashboardSetup
{
    public class HomeController : Controller
    {
        private static ILog Log { get; set; }

        ILog log = log4net.LogManager.GetLogger(typeof(HomeController));      //type of class
        // GET: Home
        public ActionResult Dashboard()
        {
           
            long UserID = Convert.ToInt64(Session["UserID"]);
            ViewBag.UserId = UserID;
            return View();
        }
    }
}