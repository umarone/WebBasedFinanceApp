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
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PurchaseSetup.GoodsSettings
{
    public class GoodReceivedsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        GoodReceivedDAL goodReceivedDAL = new GoodReceivedDAL();
        
        // GET: GoodReceiveds
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            
                long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
                long BranchId = Convert.ToInt64(Session["BranchId"]);
                List<GoodReceived> listobj = new List<GoodReceived>();
                listobj = db.GoodReceiveds.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a => a.GoodReceivedNoteId).ToList();
                PagedList<GoodReceived> model = new PagedList<GoodReceived>(listobj, page, pageSize);
                return View(model);
           
        }

        // GET: GoodReceiveds/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceived goodReceived = db.GoodReceiveds.Find(id);
            var list = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteID == id).ToList();
            ViewBag.GoodsReceivedDetailsList = list;
            if (goodReceived == null)
            {
                return HttpNotFound();
            }
            return View(goodReceived);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceived goodReceived = db.GoodReceiveds.Find(id);
            var list = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteID == id).ToList();
            ViewBag.GoodsReceivedDetailsList = list;
            if (goodReceived == null)
            {
                return HttpNotFound();
            }
            return View(goodReceived);
        }
        // GET: GoodReceiveds/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            GoodReceived obj = new GoodReceived();
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
           
            obj.GRNNO = dALCode.AutoGenerateGoodReceivedsCode(OrganizationID, BranchId);
            return View(obj);
        }

        // POST: GoodReceiveds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = goodReceivedDAL.AddGoodsReceived(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Goods Received has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GoodReceiveds/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceived goodReceived = db.GoodReceiveds.Find(id);
            var list = db.GoodReceivedDetails.Where(a => a.GoodReceivedNoteID == id).ToList();
            ViewBag.GoodsReceivedDetailsList = list;
            if (goodReceived == null)
            {
                return HttpNotFound();
            }

            return View(goodReceived);
        }

        // POST: GoodReceiveds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = goodReceivedDAL.UpdateGoodsReceived(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Goods Received has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: GoodReceiveds/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodReceived goodReceived = db.GoodReceiveds.Find(id);
            if (goodReceived == null)
            {
                return HttpNotFound();
            }
            var getmessage = goodReceivedDAL.DeleteGoodsReceived(goodReceived, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Goods Received has been deleted successfully";
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

        public JsonResult LoadPurcahseOrderDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from p in db.PurchaseOrders.Where(a => a.PurchaseOrderId == id)
                          select new PurchaseOrderViewModel
                          {
                              PurchaseOrderId = p.PurchaseOrderId,
                              PurchaseOrderNO = p.PurchaseOrderNO,
                              OrganizationID = p.OrganizationID,
                              OrganizationName = p.OrganizationDefinition.OrganizationUnitName,
                              PurchaseOrderDate = p.PurchaseOrderDate,
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfPurcahseOrderDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == id && a.Active == true).ToList();

            return PartialView("_PartialViewPurchaseorderDetails", Result);
        }
        public JsonResult LoadAdjustmentNoteDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from p in db.AdjustmentNotes.Where(a => a.AdjustmentNoteId == id)
                          select new AdjustmentNotesViewModel
                          {
                              AdjustmentNoteId = p.AdjustmentNoteId,
                              AdjustmentNoteNO = p.AdjustmentNoteNO,
                              OrganizationID = p.OrganizationID,
                              OrganizationName = p.OrganizationDefinition.OrganizationUnitName,
                              Date = p.Date,
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfAdjustmentNoteDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.AdjustmentNoteDetails.Where(a => a.AdjustmentNoteID == id && a.Active == true).ToList();

            return PartialView("_PartialViewAdjustmentNoteDetails", Result);
        }
        public JsonResult LoadSTNDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from p in db.StoreTransferNotes.Where(a => a.Id == id)
                          select new StoreTransferNotesViewModel
                          {
                              Id = p.Id,
                              Code = p.Code,
                              OrganizationID = p.OrganizationID,
                              OrganizationName = p.OrganizationDefinition.OrganizationUnitName,
                              Date = p.STNDate,
                              ToBranchId=p.ToBranchId
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfSTNDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.StoreTransferNoteDetails.Where(a => a.STNId == id).ToList();

            return PartialView("_PartialViewSTNDetails", Result);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            GoodReceived _searchObject = JsonConvert.DeserializeObject<GoodReceived>(searchModel);
            List<GoodReceived> _listObject = new List<GoodReceived>();
            _listObject = db.GoodReceiveds.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.GRNNO) && !String.IsNullOrEmpty(_searchObject.GRNNO))
            {
                _listObject = _listObject.Where(r => r.GRNNO != null && r.GRNNO.ToString().ToLower().Contains(_searchObject.GRNNO)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.GRNNO))
            {
                _listObject = _listObject.Where(r => r.GRNNO != null && r.GRNNO.ToString().ToLower().Contains(_searchObject.GRNNO)).ToList();
            }
            
            return PartialView("_Search", _listObject);
        }
    }
}
