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

namespace MudasirRehmanAlp.PurchaseSetup.PurchaseSettings
{
    public class PurchaseOrderReturnsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        PurchaseReturnDAL purchaseReturnDAL = new PurchaseReturnDAL();
        
        // GET: PurchaseOrderReturns
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<PurchaseOrderReturn> listobj = new List<PurchaseOrderReturn>();
            listobj = db.PurchaseOrderReturns.Where(a=>a.IsDeleted==false && a.OrganizationID== OrganizationID && a.BranchId == BranchId).ToList();
            PagedList<PurchaseOrderReturn> model = new PagedList<PurchaseOrderReturn>(listobj, page, pageSize);
            return View(model);

        }

        // GET: PurchaseOrderReturns/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderReturn purchaseOrderReturn = db.PurchaseOrderReturns.Find(id);
            var listofPurchasrReturn = db.PurchaseOrderReturnDetails.Where(a => a.PurchaseOrderReturnID == id).ToList();
            ViewBag.PurchaseReturnListDetails = listofPurchasrReturn;
            if (purchaseOrderReturn == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderReturn);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderReturn purchaseOrderReturn = db.PurchaseOrderReturns.Find(id);
            var listofPurchasrReturn = db.PurchaseOrderReturnDetails.Where(a => a.PurchaseOrderReturnID == id).ToList();
            ViewBag.PurchaseReturnListDetails = listofPurchasrReturn;
            if (purchaseOrderReturn == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderReturn);
        }
        // GET: PurchaseOrderReturns/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            PurchaseOrderReturn obj = new PurchaseOrderReturn();
            obj.PurchaseOrderReturnNO = dALCode.AutoGeneratePurchaseReturnCode(OrganizationID, BranchId);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;

            
            return View(obj);
        }

        // POST: PurchaseOrderReturns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = purchaseReturnDAL.AddPurchaseReturn(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Return has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: PurchaseOrderReturns/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderReturn purchaseOrderReturn = db.PurchaseOrderReturns.Find(id);
            var listofPurchasrReturn = db.PurchaseOrderReturnDetails.Where(a => a.PurchaseOrderReturnID == id).ToList();
            ViewBag.PurchaseReturnListDetails = listofPurchasrReturn;
            if (purchaseOrderReturn == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderReturn);
        }

        // POST: PurchaseOrderReturns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
  
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = purchaseReturnDAL.UpdatePurchaseReturn(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Return has been updated successfully";
                return RedirectToAction("Index");
            }
        }

        // GET: PurchaseOrderReturns/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrderReturn purchaseOrderReturn = db.PurchaseOrderReturns.Find(id);
            if (purchaseOrderReturn == null)
            {
                return HttpNotFound();
            }
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = purchaseReturnDAL.DeletePurchaseReturn(purchaseOrderReturn,UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Return has been deleted successfully";
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
        public JsonResult LoadGoodReturnDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from p in db.GoodReceivedReturns.Where(a => a.GoodsReceivedReturnID == id)
                          select new GoodsReturnViewModel
                          {
                              PurchaseOrderId = p.GoodReceived.PurchaseOrder.PurchaseOrderId,
                              PurchaseOrderNO = p.GoodReceived.PurchaseOrder.PurchaseOrderNO,                             
                              PurchaseOrderDate = p.GoodReceived.PurchaseOrder.PurchaseOrderDate,
                              PuchaseOrderType = p.GoodReceived.PurchaseOrder.PuchaseOrderType,
                              TransactionType = p.GoodReceived.PurchaseOrder.TransactionType,                           
                              PaymentTerms = p.GoodReceived.PurchaseOrder.PaymentTerms,
                              SupplierName = p.GoodReceived.PurchaseOrder.SupplierDefinition.SupplierName,
                              SupplierID = p.GoodReceived.PurchaseOrder.SupplierID,
                              AccountNo = p.GoodReceived.PurchaseOrder.Account.AccountNo,
                              AccountID = p.GoodReceived.PurchaseOrder.AccountID,

                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfGoodReturnDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.GoodReceivedReturnDetails.Where(a => a.GoodsReceivedReturnID == id && a.Active == true).ToList();

            return PartialView("_PartialViewPurchaseRetunDetails", Result);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            PurchaseOrderReturn _searchObject = JsonConvert.DeserializeObject<PurchaseOrderReturn>(searchModel);
            List<PurchaseOrderReturn> _listObject = new List<PurchaseOrderReturn>();
            _listObject = db.PurchaseOrderReturns.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.PurchaseOrderReturnNO))
            {
                _listObject = _listObject.Where(r => r.PurchaseOrderReturnNO != null && r.PurchaseOrderReturnNO.ToString().ToLower().Contains(_searchObject.PurchaseOrderReturnNO)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.PurchaseOrderReturnNO))
            {
                _listObject = _listObject.Where(r => r.PurchaseOrderReturnNO != null && r.PurchaseOrderReturnNO.ToString().ToLower().Contains(_searchObject.PurchaseOrderReturnNO)).ToList();
            }
            
            return PartialView("_Search", _listObject);
        }
    }
}
