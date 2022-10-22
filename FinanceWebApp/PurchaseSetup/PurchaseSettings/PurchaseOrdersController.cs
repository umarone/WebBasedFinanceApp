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

    public class PurchaseOrdersController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        PurchaseOrderDAL purchaseOrderDAL = new PurchaseOrderDAL();
        SystemDAL systemDAL = new SystemDAL();

        // GET: PurchaseOrders

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<PurchaseOrder> listobj = new List<PurchaseOrder>();
            listobj = db.PurchaseOrders.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a=>a.PurchaseOrderId).ToList();
            PagedList<PurchaseOrder> model = new PagedList<PurchaseOrder>(listobj, page, pageSize);
            return View(model);

        }

        // GET: PurchaseOrders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            var listPODetails = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == id).ToList();
            ViewBag.ListPurchaseOrderDetails = listPODetails;
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            var listPODetails = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == id).ToList();
            ViewBag.ListPurchaseOrderDetails = listPODetails;
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            PurchaseOrder obj = new PurchaseOrder();
            obj.PurchaseOrderNO = dALCode.AutoGeneratePurchaseOrderCode(OrganizationID, BranchId);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            var findCurrency = db.CurrencyDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID ).FirstOrDefault();
            var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.Account.OrganizationID == OrganizationID && a.BranchId == BranchId && a.AccountDefaultType == CommonEnums.AccountDefaultType.Purchases).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearsNo = findfinancialBookYear.YearCode;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add financial book year befor you add vouchers.";
            }
            if (findCurrency !=null)
            {
                obj.CurrencyID =Convert.ToInt64(findCurrency.CurrencyID);
                ViewBag.CurrencyName = findCurrency.CurrencySymbol + " - " + findCurrency.CurrencyName;

            }
            if (findAccounts == null)
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please set one account for purchases.";
            }
            return View(obj);
        }


        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string ObjMasterItem, string ObjChilds, HttpPostedFileBase[] uploadPurchaseOrderImage)
        {
            HttpPostedFileBase POfile = Request.Files["uploadPurchaseOrderImage"];
            long UserID = Convert.ToInt64(Session["UserID"]);

            var getmessage = purchaseOrderDAL.AddPurchaseOrder(ObjMasterItem, ObjChilds, POfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Order has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: PurchaseOrders/Edit/5
        public ActionResult Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            var listPODetails = db.PurchaseOrderDetails.Where(a => a.PurchaseOrderID == id).ToList();
            ViewBag.ListPurchaseOrderDetails = listPODetails;
            ViewBag.TotalItamCount = listPODetails.Count();

            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(string ObjMasterItem, string ObjChilds, HttpPostedFileBase[] uploadPurchaseOrderImage)
        {
            HttpPostedFileBase POfile = Request.Files["uploadPurchaseOrderImage"];
            long UserID = Convert.ToInt64(Session["UserID"]);


            var getmessage = purchaseOrderDAL.UpdatePurchaseOrder(ObjMasterItem, ObjChilds, POfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Order has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: PurchaseOrders/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            var getmessage = purchaseOrderDAL.DeletePurchaseOrder(purchaseOrder, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Order has been deleted successfully";
                return RedirectToAction("Index");
            }
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeletePurchaseOrderDetailSingle(string ID)
        {

            var getmessage = purchaseOrderDAL.DeletePurchaseOrderDetails(ID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Purchase Order has been update successfully";
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
        // GET: PurchaseOrders/POCompleted/5
        public ActionResult POCompleted(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
         
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            purchaseOrder.IsCompleted = true;
            purchaseOrder.CompletedBy = UserID;
            purchaseOrder.CompletedDate = DateTime.Now;

            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }

            db.Entry(purchaseOrder).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Purchase Order Completed has been successfully";
            return RedirectToAction("Index");
        }


        public JsonResult CheckAccountNoInSupplier(string ID)
        {
            long id = 0;
            String Message = "";
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var result = db.SupplierDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.Id == id).FirstOrDefault();
            if (result.AccountID != null && result.AccountID != 0)
            {
                Message = "Yes";
            }
            else
            {
                Message = "No";
            }

            return Json(Message, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadAccountNo(string ID)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var result = (from s in db.SupplierDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where s.Id == id
                          select new AccountsViewModel
                          {
                              AccountId = s.Account.AccountId,
                              AccountNo = s.Account.AccountNo
                          }).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            PurchaseOrder _searchObject = JsonConvert.DeserializeObject<PurchaseOrder>(searchModel);
            List<PurchaseOrder> _listObject = new List<PurchaseOrder>();
            _listObject = db.PurchaseOrders.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.PurchaseOrderNO))
            {
                _listObject = _listObject.Where(r => r.PurchaseOrderNO != null && r.PurchaseOrderNO.ToString().ToLower().Contains(_searchObject.PurchaseOrderNO)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.PurchaseOrderNO))
            {
                _listObject = _listObject.Where(r => r.PurchaseOrderNO != null && r.PurchaseOrderNO.ToString().ToLower().Contains(_searchObject.PurchaseOrderNO)).ToList();
            }

            return PartialView("_Search", _listObject);
        }
    }
}
