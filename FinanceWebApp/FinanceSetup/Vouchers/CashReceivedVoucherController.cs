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

namespace MudasirRehmanAlp.FinanceSetup.Vouchers
{
    public class CashReceivedVoucherController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        VouchersDAL vouchersDALObj = new VouchersDAL();
        
        // GET: CashReceivedVoucher
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<VouchersHead> listobj = new List<VouchersHead>();
            listobj = db.VouchersHeads.Where(a => a.VoucherType == CommonEnums.VoucherType.CRV && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a=>a.VoucherID).ToList();
            PagedList<VouchersHead> model = new PagedList<VouchersHead>(listobj, page, pageSize);
            return View(model);
        }

        // GET: CashReceivedVoucher/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VouchersHead vouchersHead = db.VouchersHeads.Find(id);
            if (vouchersHead == null)
            {
                return HttpNotFound();
            }

            var listDetails = db.VouchersDetails.Where(a => a.VoucherId == id).ToList();
            ViewBag.ListVouchersDetailsDetails = listDetails;

            return View(vouchersHead);
        }
        // GET: CashReceivedVoucher/Print/5
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VouchersHead vouchersHead = db.VouchersHeads.Find(id);
            if (vouchersHead == null)
            {
                return HttpNotFound();
            }

            var listDetails = db.VouchersDetails.Where(a => a.VoucherId == id).ToList();
            ViewBag.ListVouchersDetailsDetails = listDetails;

            return View(vouchersHead);
        }
        // GET: CashReceivedVoucher/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            VouchersHead obj = new VouchersHead();
            obj.VoucherCode = dALCode.AutoGenerateVouchersCRVCode(OrganizationID, BranchId);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();

            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearsDates =Convert.ToDateTime(findfinancialBookYear.YearStartDate).ToString("dd/mm-yyyy") +"-To-"+Convert.ToDateTime(findfinancialBookYear.YearClosingDate).ToString("dd/mm-yyyy");
                obj.FinancialBookYearId = findfinancialBookYear.Id;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add financial book year befor you add vouchers.";
            }

            return View(obj);
        }

        // POST: CashReceivedVoucher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);


            var getmessage = vouchersDALObj.AddCashReceivedVoucher(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Cash Received Voucher has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CashReceivedVoucher/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VouchersHead vouchersHead = db.VouchersHeads.Find(id);
            if (vouchersHead == null)
            {
                return HttpNotFound();
            }

            var listDetails = db.VouchersDetails.Where(a => a.VoucherId == id).ToList();
            ViewBag.ListVouchersDetailsDetails = listDetails;

            ViewBag.FinancialBookYearsDates = Convert.ToDateTime(vouchersHead.FinancialBookYear.YearStartDate).ToString("dd/mm-yyyy") + "-To-" + Convert.ToDateTime(vouchersHead.FinancialBookYear.YearClosingDate).ToString("dd/mm-yyyy");

            return View(vouchersHead);
        }

        // POST: CashReceivedVoucher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = vouchersDALObj.UpdateCashReceivedVoucher(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Cash Received Voucher has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CashReceivedVoucher/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VouchersHead vouchersHead = db.VouchersHeads.Find(id);
            if (vouchersHead == null)
            {
                return HttpNotFound();
            }
            
            var getmessage = vouchersDALObj.DeleteCashReceivedVoucher(vouchersHead, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Cash Received Voucher has been deleted successfully";
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
        //----------------- Json Functions
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            VouchersHead _searchObject = JsonConvert.DeserializeObject<VouchersHead>(searchModel);
            List<VouchersHead> _listObject = new List<VouchersHead>();
            _listObject = db.VouchersHeads.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.VoucherType == CommonEnums.VoucherType.CRV).ToList();
            if (!String.IsNullOrEmpty(_searchObject.VoucherCode) && !String.IsNullOrEmpty(_searchObject.VoucherCode))
            {
                _listObject = _listObject.Where(r => r.VoucherCode != null && r.VoucherCode.ToString().ToLower().Contains(_searchObject.VoucherCode) || r.VoucherCode.ToString().ToLower().Contains(_searchObject.VoucherCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.VoucherCode))
            {
                _listObject = _listObject.Where(r => r.VoucherCode != null && r.VoucherCode.ToString().ToLower().Contains(_searchObject.VoucherCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.VoucherCode))
            {
                _listObject = _listObject.Where(r => r.VoucherCode != null && r.VoucherCode.ToString().ToLower().Contains(_searchObject.VoucherCode)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
