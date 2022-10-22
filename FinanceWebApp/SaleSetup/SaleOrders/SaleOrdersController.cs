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
using static MudasirRehmanAlp.ModelsView.CommonEnums;

namespace MudasirRehmanAlp.SaleSetup.SaleOrders
{
    public class SaleOrdersController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        SaleOrderDAL saleOrderDAL = new SaleOrderDAL();

        // GET: SaleOrders

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<SaleOrder> listobj = new List<SaleOrder>();
            listobj = db.SaleOrders.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a => a.SaleOrdeID).ToList();
            PagedList<SaleOrder> model = new PagedList<SaleOrder>(listobj, page, pageSize);
            return View(model);

        }
        // GET: SaleOrders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleOrder saleOrder = db.SaleOrders.Find(id);
            var listSODetails = db.SaleOrderDetails.Where(a => a.SaleOrderID == id).ToList();
            ViewBag.ListSaleOrderDetails = listSODetails;
            
            if (saleOrder == null)
            {
                return HttpNotFound();
            }
            return View(saleOrder);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleOrder saleOrder = db.SaleOrders.Find(id);
            var listSODetails = db.SaleOrderDetails.Where(a => a.SaleOrderID == id).ToList();
            ViewBag.ListSaleOrderDetails = listSODetails;
            var ListObj = db.GuarantorDefinitions.Where(a => a.IsDeleted == false && a.CustomerStatementId == saleOrder.CustomerStatementID).ToList();
            ViewBag.GuarantorDefinitionsList = ListObj;

            if (saleOrder == null)
            {
                return HttpNotFound();
            }
            return View(saleOrder);
        }
        // GET: SaleOrders/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SaleOrder obj = new SaleOrder();
            obj.SaleOrderNo = dALCode.AutoGenerateSaleOrderCode(OrganizationID, BranchId);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.Account.OrganizationID == OrganizationID && a.BranchId == BranchId && a.AccountDefaultType == CommonEnums.AccountDefaultType.Sales).FirstOrDefault();
            if (findAccounts == null)
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please set one account for Sales.";
            }

            return View(obj);
        }

        // POST: SaleOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds,HttpPostedFileBase[] uploadSaleOrderImage)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase SOfile = Request.Files["uploadSaleOrderImage"];


            var getmessage = saleOrderDAL.AddSaleOrder(ObjMasterItem, ObjChilds, SOfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Order has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SaleOrders/Edit/5
        public ActionResult Edit(long? id)
        {
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleOrder saleOrder = db.SaleOrders.Find(id);
            var listSODetails = db.SaleOrderDetails.Where(a => a.SaleOrderID == id).ToList();
            ViewBag.ListSaleOrderDetails = listSODetails;
            if (saleOrder == null)
            {
                return HttpNotFound();
            }

            return View(saleOrder);
        }

        // POST: SaleOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds, HttpPostedFileBase[] uploadSaleOrderImage)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase SOfile = Request.Files["uploadSaleOrderImage"];


            var getmessage = saleOrderDAL.UpdateSaleOrder(ObjMasterItem, ObjChilds, SOfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Order has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SaleOrders/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleOrder saleOrder = db.SaleOrders.Find(id);
            if (saleOrder == null)
            {
                return HttpNotFound();
            }
            var getmessage = saleOrderDAL.DeleteSaleOrder(saleOrder, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Order has been deleted successfully";
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeleteSaleOrderDetailSingle(string ID)
        {

            var getmessage = saleOrderDAL.DeleteSaleOrderDetails(ID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Order Item has been deleted successfully";
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
        public JsonResult CheckAccountNoInDistributor(string ID)
        {
            long id = 0;
            String Message = "";
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var result = db.DistributorDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.Id == id).FirstOrDefault();
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
        public JsonResult CheckAccountNoInCustomerStatement(string ID)
        {
            long id = 0;
            String Message = "";
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var result = db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false && a.Id == id).FirstOrDefault();
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

            var result = (from s in db.DistributorDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)
                          where s.Id == id
                          select new AccountsViewModel
                          {
                              AccountId = s.Account.AccountId,
                              AccountNo = s.Account.AccountNo
                          }).FirstOrDefault();

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadAccountNoCustomerStatement(string ID)
        {
            long id = 0;
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var result = (from s in db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false)
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
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SaleOrder _searchObject = JsonConvert.DeserializeObject<SaleOrder>(searchModel);
            List<SaleOrder> _listObject = new List<SaleOrder>();
            _listObject = db.SaleOrders.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.SaleOrderNo) && !String.IsNullOrEmpty(_searchObject.SaleOrderNo))
            {
                _listObject = _listObject.Where(r => r.SaleOrderNo != null && r.SaleOrderNo.ToString().ToLower().Contains(_searchObject.SaleOrderNo)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.SaleOrderNo))
            {
                _listObject = _listObject.Where(r => r.SaleOrderNo != null && r.SaleOrderNo.ToString().ToLower().Contains(_searchObject.SaleOrderNo)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
        public ActionResult InstallmentsScheduler(string getModel)
        {
            int srno = 0;
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            GetSchedulerViewModel _searchObject = JsonConvert.DeserializeObject<GetSchedulerViewModel>(getModel);
            List<InstallmentsSchedulerViewModel> _listObject = new List<InstallmentsSchedulerViewModel>();
            for (int i = 0; i < _searchObject.NoOfMonths; i++)
            {
                srno = srno + 1;
                InstallmentsSchedulerViewModel _Object = new InstallmentsSchedulerViewModel();

                _Object.SrNo = srno;
                _Object.PaymentDate = Convert.ToDateTime(_searchObject.StartDate).AddMonths(i);
                
                _Object.MonthName = Convert.ToDateTime(_searchObject.StartDate).AddMonths(i).ToString("MMMM");
                _Object.PerMonthInstallment =Convert.ToDecimal(_searchObject.PerMonthAmount);
                _Object.InstallmentPaid = 0;
                _Object.ExtraCharges = 0;
                //_Object.DateReceived = DateTime.now;
                _Object.PaymentStatus = PaymentStatus.UnPaid;
                _Object.Notes = "";
                _listObject.Add(_Object);
            }

            return PartialView("_Scheduler", _listObject);
        }
       
     
    }
}
