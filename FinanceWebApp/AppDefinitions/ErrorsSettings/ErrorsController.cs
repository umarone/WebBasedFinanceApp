using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MudasirRehmanAlp.AppDefinitions.ErrorsSettings
{
    public class ErrorsController : Controller
    {
        // GET: ErrorsController
      
        public ActionResult Exception( )
        {
            return View();
        }
        public ActionResult NoPermission()
        {
            return View();
        }
        
        public ActionResult Http400(string source)
        {
           
            Response.StatusCode = 400;
            return View();
        }

        public ActionResult Http404(string source)
        {

            Response.StatusCode = 404;
            return View();
        }
        public ActionResult Http500(string source)
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}