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
    public class SaleInvoicesController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        SaleInvoiceDAL invoiceDAL = new SaleInvoiceDAL();
        
        // GET: SaleInvoices
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<SaleInvoice> listobj = new List<SaleInvoice>();
            listobj = db.SaleInvoices.Where(a => a.SaleInvoiceType == SaleInvoiceTypeEnum.NetSaleInvoice && a.IsDeleted==false && a.OrganizationID== OrganizationID).ToList();
            PagedList<SaleInvoice> model = new PagedList<SaleInvoice>(listobj, page, pageSize);
            return View(model);

        }

        // GET: SaleInvoices/Details/5
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

        // GET: SaleInvoices/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            SaleInvoice obj = new SaleInvoice();
            obj.SaleInvoiceNo = dALCode.AutoGenerateSaleInvoiceNetCode(OrganizationID);            
            var OrganizationObj = dALCode.GetOrganizationDefinition(OrganizationID);
            obj.OrganizationID = OrganizationObj.Id;
            ViewBag.OrganizationUnitName = OrganizationObj.OrganizationUnitName;
            ViewBag.CustomerCode = dALCode.AutoGenerateCustomerCode(OrganizationID);
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

        // POST: SaleInvoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds,string ObjCustomer, HttpPostedFileBase[] uploadCustomerPicture)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase Cusfile = Request.Files["uploadCustomerPicture"];

            
            var getmessage = invoiceDAL.AddSaleInvoiceNet(ObjMasterItem, ObjChilds, ObjCustomer, Cusfile, UserID);
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
        // GET: SaleInvoices/Edit/5
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
            var listSIDetails = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceID == id).ToList();
            ViewBag.ListSaleInvoiceDetails = listSIDetails;

            var findCustomer = db.Customers.Where(a => a.CustomerId == saleInvoice.CustomerID).FirstOrDefault();
            ViewBag.CustomerCode = findCustomer.CustomerCode;
            ViewBag.Title = findCustomer.Title;
            ViewBag.FirstName = findCustomer.FirstName;
            ViewBag.LastName = findCustomer.LastName;
            ViewBag.GuardianName = findCustomer.GuardianName;
            ViewBag.MobileNo = findCustomer.MobileNo;
            ViewBag.PhoneNo = findCustomer.PhoneNo;
            ViewBag.Email = findCustomer.Email;
            ViewBag.CNIC = findCustomer.CNIC;
            ViewBag.Address = findCustomer.Address;
            ViewBag.Picture = findCustomer.Picture;


            return View(saleInvoice);
        }

        // POST: SaleInvoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
      
        public ActionResult Edit(string ObjMasterItem, string ObjChilds, string ObjCustomer, HttpPostedFileBase[] uploadCustomerPicture)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase Cusfile = Request.Files["uploadCustomerPicture"];

           
            var getmessage = invoiceDAL.UpdateSaleInvoiceNet(ObjMasterItem, ObjChilds, ObjCustomer, Cusfile, UserID);
           
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

        // GET: SaleInvoices/Delete/5
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
            var getmessage = invoiceDAL.DeleteSaleInvoiceNet(saleInvoice, UserID);

            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Invoice has been deleted successfully";
                return RedirectToAction("Index");
            }
        }
        public ActionResult DeleteSaleInvoicesDetailSingle(string ID)
        {

            var getmessage = invoiceDAL.DeleteSaleInvoiceDetails(ID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Invoice Detail has been deleted successfully";
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
            SaleInvoice _searchObject = JsonConvert.DeserializeObject<SaleInvoice>(searchModel);
            List<SaleInvoice> _listObject = new List<SaleInvoice>();
            _listObject = db.SaleInvoices.Where(a => a.IsActive == true && a.IsDeleted == false && a.CID == OrganizationID && a.SaleInvoiceType == SaleInvoiceTypeEnum.NetSaleInvoice).ToList();
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
