using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PurchaseSetup.PurchaseSettings
{
    public class AdjustmentNotesController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        AdjustmentNotesDAL adjustmentNotesDAL = new AdjustmentNotesDAL();
        
        // GET: AdjustmentNotes
        public ActionResult Index(int page = 1, int pageSize = 15)
        {

            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<AdjustmentNote> listobj = new List<AdjustmentNote>();
            listobj = db.AdjustmentNotes.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a=>a.AdjustmentNoteId).ToList();
            PagedList<AdjustmentNote> model = new PagedList<AdjustmentNote>(listobj, page, pageSize);
            return View(model);

        }
        // GET: AdjustmentNotes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentNote adjustmentNote = db.AdjustmentNotes.Find(id);
            var listADDetails = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteID == id).ToList();
            ViewBag.ListAdjustmentNoteDetails = listADDetails;
            if (adjustmentNote == null)
            {
                return HttpNotFound();
            }
            return View(adjustmentNote);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentNote adjustmentNote = db.AdjustmentNotes.Find(id);
            var listADDetails = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteID == id).ToList();
            ViewBag.ListAdjustmentNoteDetails = listADDetails;
            if (adjustmentNote == null)
            {
                return HttpNotFound();
            }
            return View(adjustmentNote);
        }

        // GET: AdjustmentNotes/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            AdjustmentNote obj = new AdjustmentNote();
            obj.AdjustmentNoteNO = dALCode.AutoGenerateAdjustmentNoteCode(OrganizationID, BranchId);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;


            return View(obj);
        }

        // POST: AdjustmentNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = adjustmentNotesDAL.AddAdjustmentNotes(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Adjustment Notes has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: AdjustmentNotes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentNote adjustmentNote = db.AdjustmentNotes.Find(id);
            var listADDetails = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteID == id).ToList();
            ViewBag.ListAdjustmentNoteDetails = listADDetails;
            if (adjustmentNote == null)
            {
                return HttpNotFound();
            }
           
            return View(adjustmentNote);
        }

        // POST: AdjustmentNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = adjustmentNotesDAL.UpdateAdjustmentNotes(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Adjustment Notes has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: AdjustmentNotes/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdjustmentNote adjustmentNote = db.AdjustmentNotes.Find(id);
            if (adjustmentNote == null)
            {
                return HttpNotFound();
            }
            var getmessage = adjustmentNotesDAL.DeleteAdjustmentNotes(adjustmentNote, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Adjustment Notes has been deleted successfully";
                return RedirectToAction("Index");
            }
        }

       
        public ActionResult DeleteAdjustmentNotesDetailSingle(string ID)
        {

            var getmessage = adjustmentNotesDAL.DeleteAdjustmentNotesDetails(ID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Adjustment Notes Detail Item has been deleted successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            AdjustmentNote _searchObject = JsonConvert.DeserializeObject<AdjustmentNote>(searchModel);
            List<AdjustmentNote> _listObject = new List<AdjustmentNote>();
            _listObject = db.AdjustmentNotes.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.AdjustmentNoteNO) && !String.IsNullOrEmpty(_searchObject.AdjustmentNoteNO))
            {
                _listObject = _listObject.Where(r => r.AdjustmentNoteNO != null && r.AdjustmentNoteNO.ToString().ToLower().Contains(_searchObject.AdjustmentNoteNO)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.AdjustmentNoteNO))
            {
                _listObject = _listObject.Where(r => r.AdjustmentNoteNO != null && r.AdjustmentNoteNO.ToString().ToLower().Contains(_searchObject.AdjustmentNoteNO)).ToList();
            }

            return PartialView("_Search", _listObject);
        }
    }
}
