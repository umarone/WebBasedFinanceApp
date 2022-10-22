using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.MiscellaneousSetup
{
    public class NoticeBoardController : Controller
    {
        private AppEntities db = new AppEntities();

        // GET: NoticeBoard
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<Notification> listobj = new List<Notification>();
            listobj = db.Notifications.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId==BranchId && a.IsRead==false).OrderByDescending(a => a.Id).ToList();
            PagedList<Notification> model = new PagedList<Notification>(listobj, page, pageSize);
            return View(model);
        }
        public ActionResult JsonUpdate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            NotificationViewModel postModel = JsonConvert.DeserializeObject<NotificationViewModel>(model);
            postModel.OrganizationId = OrganizationID;

            var findObj = db.Notifications.Where(a => a.Id == postModel.Id).FirstOrDefault();

            findObj.IsRead = true;
            findObj.UpdatedBy = UserID;
            findObj.UpdatedDate = DateTime.Now;
            db.Entry(findObj).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Notification has been read successfully";

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
