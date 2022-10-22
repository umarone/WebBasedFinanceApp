using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using PagedList;

namespace MudasirRehmanAlp.PurchaseSetup.PurchaseSettings
{
    public class StoreTransferNotesController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        StoreTransferNotesDAL storeTransferNotesDAL = new StoreTransferNotesDAL();
        // GET: StoreTransferNotes
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<StoreTransferNote> listobj = new List<StoreTransferNote>();
            listobj = db.StoreTransferNotes.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.FromBranchId == BranchId).OrderByDescending(a => a.Id).ToList();
            PagedList<StoreTransferNote> model = new PagedList<StoreTransferNote>(listobj, page, pageSize);
            return View(model);
        }

        // GET: StoreTransferNotes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreTransferNote storeTransferNote = db.StoreTransferNotes.Find(id);
            if (storeTransferNote == null)
            {
                return HttpNotFound();
            }
            return View(storeTransferNote);
        }

        // GET: StoreTransferNotes/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            StoreTransferNote obj = new StoreTransferNote();
            obj.Code = dALCode.AutoGenerateStoreTransferNotesCode(OrganizationID, BranchId);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.FromBranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: StoreTransferNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = storeTransferNotesDAL.Add(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Store Transfer Notes has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: StoreTransferNotes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreTransferNote storeTransferNote = db.StoreTransferNotes.Find(id);
            if (storeTransferNote == null)
            {
                return HttpNotFound();
            }
            var findToBranchObj = db.BranchDefinitions.Where(a => a.Id == storeTransferNote.ToBranchId).FirstOrDefault();
            ViewBag.ToBranchName = findToBranchObj.Name;

            var findSTNDetailsList = db.StoreTransferNoteDetails.Where(a => a.STNId == storeTransferNote.Id).ToList();
            ViewBag.StoreTransferNoteDetailsList = findSTNDetailsList;
            return View(storeTransferNote);
        }

        // POST: StoreTransferNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = storeTransferNotesDAL.Update(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Store Transfer Notes has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: StoreTransferNotes/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreTransferNote storeTransferNote  = db.StoreTransferNotes.Find(id);
            if (storeTransferNote == null)
            {
                return HttpNotFound();
            }
            var getmessage = storeTransferNotesDAL.Delete(storeTransferNote, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Store Transfer Notes has been deleted successfully";
                return RedirectToAction("Index");
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
        public ActionResult DeleteStoreTransferNotesDetailSingle(string Id)
        {

            var getmessage = storeTransferNotesDAL.DeleteSTNSingleDetails(Id);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Store Transfer Notes Detail Item has been deleted successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        //---------------------------------- Js Functions
        public JsonResult loadStockQuantity(string Id)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(Id))
            {
                id = Convert.ToInt64(Id);
            }
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var jsonResult = (from s in db.StockofGoods.Where(a => a.Active == true)
                              where s.StockID == id
                              select new
                              {
                                  ProductID = s.ProductID,
                                  Quantity = s.Quantity,
                              }).FirstOrDefault();

            return Json(jsonResult, JsonRequestBehavior.AllowGet);

        }
    }
}
