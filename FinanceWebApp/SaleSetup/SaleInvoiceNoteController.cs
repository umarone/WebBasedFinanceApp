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

namespace MudasirRehmanAlp.SaleSetup
{
    public class SaleInvoiceNoteController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        SaleInvoiceDAL invoiceDAL = new SaleInvoiceDAL();

        // GET: SaleInvoiceNote

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<SaleInvoice> listobj = new List<SaleInvoice>();
            listobj = db.SaleInvoices.Where(a => a.SaleInvoiceType == SaleInvoiceTypeEnum.SaleInvoice && a.IsDeleted == false && a.CID == OrganizationID).ToList();
            PagedList<SaleInvoice> model = new PagedList<SaleInvoice>(listobj, page, pageSize);
            return View(model);

        }

        // GET: SaleInvoiceNote/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleInvoice saleInvoice = db.SaleInvoices.Find(id);
            var listSIDetails = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceID == id).ToList();
            ViewBag.ListSaleInvoiceDetails = listSIDetails;
            if (saleInvoice == null)
            {
                return HttpNotFound();
            }
            return View(saleInvoice);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleInvoice saleInvoice = db.SaleInvoices.Find(id);
            var listSIDetails = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceID == id).ToList();
            ViewBag.ListSaleInvoiceDetails = listSIDetails;
            if (saleInvoice == null)
            {
                return HttpNotFound();
            }
            return View(saleInvoice);
        }
        // GET: SaleInvoiceNote/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            SaleInvoice obj = new SaleInvoice();
            obj.SaleInvoiceNo = dALCode.AutoGenerateSaleInvoiceCode(OrganizationID);
            var OrganizationObj = dALCode.GetOrganizationDefinition(OrganizationID);
            obj.OrganizationID = OrganizationObj.Id;
            ViewBag.OrganizationUnitName = OrganizationObj.OrganizationUnitName;
            var findfinancialBookYear = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.IsDefault == true && a.OrganizationID == OrganizationID).FirstOrDefault();
            var findAccounts = db.AccountMapping.Where(a => a.Account.IsActive == true && a.Account.IsDeleted == false && a.OrganizationId == OrganizationID && a.AccountDefaultType == CommonEnums.AccountDefaultType.Sales).FirstOrDefault();
            if (findfinancialBookYear != null)
            {
                ViewBag.FinancialBookYearsNo = findfinancialBookYear.YearCode;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add financial book year befor you add vouchers.";
            }

            if (findAccounts == null)
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please set one account for sales.";
            }
            return View(obj);
        }

        // POST: SaleInvoiceNote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = invoiceDAL.AddSaleInvoice(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Invoice has been saved successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SaleInvoiceNote/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleInvoice saleInvoice = db.SaleInvoices.Find(id);
            if (saleInvoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.SaleOrderList = db.SaleOrders.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.SaleOrderNo).ToList();
            var listSIDetails = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceID == id).ToList();
            ViewBag.ListSaleInvoiceDetails = listSIDetails;
            return View(saleInvoice);
        }

        // POST: SaleInvoiceNote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = invoiceDAL.UpdateSaleInvoice(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Invoice has been updated successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        // GET: SaleInvoiceNote/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleInvoice saleInvoice = db.SaleInvoices.Find(id);
            if (saleInvoice == null)
            {
                return HttpNotFound();
            }

            var getmessage = invoiceDAL.DeleteSaleInvoice(saleInvoice, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Invoice has been updated successfully";
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
        public JsonResult LoadSaleOrderDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from s in db.SaleOrders.Where(a => a.SaleOrdeID == id)
                          select new SaleOrderViewModel
                          {
                              SaleOrdeID = s.SaleOrdeID,
                              SaleOrderNo = s.SaleOrderNo,
                              SaleOrderDate = s.SaleOrderDate,
                              CustomerStatementName = s.CustomerStatement.Name,
                              DistributorName = s.DistributorDefinition.DistributorName,
                              TransactionType = s.TransactionType,
                              
                            
                              FreightCharges = s.FreightCharges,
                              NetTotal = s.NetTotal,
                              AmountInWord = s.AmountInWord,
                              TermAndCondition = s.TermAndCondition,
                              AccountNo = s.Account.AccountNo,
                              AccountID = s.AccountID,
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfSaleOrderDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.SaleOrderDetails.Where(a => a.SaleOrderID == id && a.Active == true).ToList();

            return PartialView("_PartialViewSaleOrderDetails", Result);
        }
        //public ActionResult LoadDetailsSchedulerDetails(string ID)
        //{
        //    long id = 0;
        //    if (ID != "")
        //    {
        //        id = Convert.ToInt64(ID);
        //    }
        //    var Result = db.InstallmentsPaymentsSchedulers.Where(a => a.SaleOrderId == id && a.IsActive == true).OrderBy(a=>a.SrNo).ToList();
        //    return PartialView("_Scheduler", Result);
        //}
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            SaleInvoice _searchObject = JsonConvert.DeserializeObject<SaleInvoice>(searchModel);
            List<SaleInvoice> _listObject = new List<SaleInvoice>();
            _listObject = db.SaleInvoices.Where(a => a.IsActive == true && a.IsDeleted == false && a.CID == OrganizationID && a.SaleInvoiceType == SaleInvoiceTypeEnum.SaleInvoice).ToList();
            if (!String.IsNullOrEmpty(_searchObject.SaleInvoiceNo) && !String.IsNullOrEmpty(_searchObject.SaleInvoiceNo))
            {
                _listObject = _listObject.Where(r => r.SaleInvoiceNo != null && r.SaleInvoiceNo.ToString().ToLower().Contains(_searchObject.SaleInvoiceNo)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.SaleInvoiceNo))
            {
                _listObject = _listObject.Where(r => r.SaleInvoiceNo != null && r.SaleInvoiceNo.ToString().ToLower().Contains(_searchObject.SaleInvoiceNo)).ToList();
            }

            return PartialView("_Search", _listObject);
        }
    }
}
