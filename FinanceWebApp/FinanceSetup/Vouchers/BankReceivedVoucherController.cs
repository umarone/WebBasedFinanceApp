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
    public class BankReceivedVoucherController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        VouchersDAL vouchersDALObj = new VouchersDAL();
        
        // GET: BankReceivedVoucher
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<VouchersHead> listobj = new List<VouchersHead>();
            listobj = db.VouchersHeads.Where(a => a.VoucherType == CommonEnums.VoucherType.BRV && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).OrderByDescending(a => a.VoucherID).ToList();
            PagedList<VouchersHead> model = new PagedList<VouchersHead>(listobj, page, pageSize);
            return View(model);
        }

        // GET: BankReceivedVoucher/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VouchersHead vouchersHead = db.VouchersHeads.Find(id);
            var listDetails = db.VouchersDetails.Where(a => a.VoucherId == id).ToList();
            ViewBag.ListVouchersDetailsDetails = listDetails;
            if (vouchersHead == null)
            {
                return HttpNotFound();
            }
            return View(vouchersHead);
        }
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
        // GET: BankReceivedVoucher/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            VouchersHead obj = new VouchersHead();
            obj.VoucherCode = dALCode.AutoGenerateVouchersBRVCode(OrganizationID, BranchId);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID && a.BranchId == BranchId).FirstOrDefault();


            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearsNo = findfinancialBookYear.YearCode;
                obj.FinancialBookYearId = findfinancialBookYear.Id;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add financial book year befor you add vouchers.";
            }

            return View(obj);
        }

        // POST: BankReceivedVoucher/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = vouchersDALObj.AddBankReceivedVoucher(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Bank Received Voucher has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: BankReceivedVoucher/Edit/5
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

            return View(vouchersHead);
        }

        // POST: BankReceivedVoucher/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = vouchersDALObj.UpdateBankReceivedVoucher(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Bank Received Voucher has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: BankReceivedVoucher/Delete/5
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
            
            var getmessage = vouchersDALObj.DeleteBankReceivedVoucher(vouchersHead, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Bank Received Voucher has been deleted successfully";
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
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            VouchersHead _searchObject = JsonConvert.DeserializeObject<VouchersHead>(searchModel);
            List<VouchersHead> _listObject = new List<VouchersHead>();
            _listObject = db.VouchersHeads.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.VoucherType == CommonEnums.VoucherType.BRV).ToList();
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
