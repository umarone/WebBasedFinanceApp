using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.AppDAL;
using PagedList;
using static MudasirRehmanAlp.ModelsView.CommonEnums;
using MudasirRehmanAlp.ModelsView;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MudasirRehmanAlp.SaleSetup
{
    public class SaleReturnsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        SaleReturnNoteDAL saleReturnNoteDAL = new SaleReturnNoteDAL();
        
        // GET: SaleReturns
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<SaleReturnNote> listobj = new List<SaleReturnNote>();
            listobj = db.SaleReturnNotes.Where(a => a.SaleReturnType == SaleInvoiceTypeEnum.NetSaleInvoice && a.IsDeleted == false && a.CID == OrganizationID).ToList();
            PagedList<SaleReturnNote> model = new PagedList<SaleReturnNote>(listobj, page, pageSize);
            return View(model);
        }
        // GET: SaleReturns/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleReturnNote saleReturnNote = db.SaleReturnNotes.Find(id);
            if (saleReturnNote == null)
            {
                return HttpNotFound();
            }
            return View(saleReturnNote);
        }

        // GET: SaleReturns/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            SaleReturnNote obj = new SaleReturnNote();
            obj.SaleReturnCode = dALCode.AutoGenerateSaleReturnCode(OrganizationID);           
            var OrganizationObj = dALCode.GetOrganizationDefinition(OrganizationID);
            obj.OrganizationID = OrganizationObj.Id;
            ViewBag.OrganizationUnitName = OrganizationObj.OrganizationUnitName;

            var SaleInvoice = (from s in db.SaleInvoices.Where(a => a.IsActive == true && a.IsDeleted == false)

                               where s.OrganizationID == OrganizationID && s.SaleInvoiceType == SaleInvoiceTypeEnum.NetSaleInvoice
                               select new SaleInvoicesViewModel
                               {
                                   SaleInvoiceID = s.SaleInvoiceID,
                                   SaleInvoiceNo = s.SaleInvoiceNo
                               }).OrderByDescending(a => a.SaleInvoiceID).ToList();
            ViewBag.SaleInvoiceList = SaleInvoice;

            return View(obj);
        }

        // POST: SaleReturns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            var getmessage = saleReturnNoteDAL.AddSaleReturnNet(ObjMasterItem, ObjChilds, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Return has been created successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SaleReturns/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleReturnNote saleReturnNote = db.SaleReturnNotes.Find(id);
            if (saleReturnNote == null)
            {
                return HttpNotFound();
            }
            var listSIRDetails = db.SaleReturnNoteDetails.Where(a => a.SaleReturnID == id).ToList();
            ViewBag.ListSaleInvoiceRetrunDetails = listSIRDetails;
            ViewBag.OrganizationUnitName = saleReturnNote.OrganizationDefinition.OrganizationUnitName;
            ViewBag.TransactionType = saleReturnNote.SaleInvoice.TransactionType;
            var findCustomer = db.Customers.Where(a => a.CustomerId == saleReturnNote.SaleInvoice.CustomerID).FirstOrDefault();
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

            ViewBag.TermsAndCondition = saleReturnNote.SaleInvoice.TermAndCondition;

            ViewBag.SubTotalWithSaleTax = saleReturnNote.SaleInvoice.SubTotalWithSaleTax;
            ViewBag.SubTotalWithOutSaleTax = saleReturnNote.SaleInvoice.SubTotalWithOutSaleTax;
            ViewBag.FreightCharges = saleReturnNote.SaleInvoice.FreightCharges;
            ViewBag.NetTotal = saleReturnNote.SaleInvoice.NetTotal;
            ViewBag.AmountInWord = saleReturnNote.SaleInvoice.AmountInWord;

            return View(saleReturnNote);
        }

        // POST: SaleReturns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(string ObjMasterItem, string ObjChilds)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            var getmessage = saleReturnNoteDAL.UpdateSaleReturnNet(ObjMasterItem, ObjChilds, UserID);

            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Return has been updated successfully";
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: SaleReturns/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SaleReturnNote saleReturnNote = db.SaleReturnNotes.Find(id);
            if (saleReturnNote == null)
            {
                return HttpNotFound();
            }
            var getmessage = saleReturnNoteDAL.DeleteSaleReturnNet(saleReturnNote, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Sale Return has been deleted successfully";
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

        public JsonResult LoadSaleInvoiceDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = (from s in db.SaleInvoices.Where(a => a.SaleInvoiceID == id)
                          select new SaleInvoicesViewModel
                          {
                              SaleInvoiceID = s.SaleInvoiceID,
                              SaleInvoiceNo = s.SaleInvoiceNo,
                              OrganizationID = s.OrganizationID,
                              OrganizationName = s.OrganizationDefinition.OrganizationUnitName,
                              SaleInvoiceDate = s.SaleInvoiceDate,
                              TransactionType = s.TransactionType,
                              CustomerCode = s.Customer.CustomerCode,
                              Title = s.Customer.Title,
                              FirstName = s.Customer.FirstName,
                              LastName = s.Customer.LastName,
                              GuardianName = s.Customer.GuardianName,
                              PhoneNo = s.Customer.PhoneNo,
                              MobileNo = s.Customer.MobileNo,
                              Email = s.Customer.Email,
                              CNIC = s.Customer.CNIC,
                              Address = s.Customer.Address,
                              SubTotalWithOutSaleTax = s.SubTotalWithOutSaleTax,
                              SubTotalWithSaleTax = s.SubTotalWithSaleTax,
                              FreightCharges = s.FreightCharges,
                              NetTotal = s.NetTotal,
                              AmountInWord = s.AmountInWord,
                              TermAndCondition = s.TermAndCondition,
                          }).FirstOrDefault();
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoadDetailsOfSaleInvoiceDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.SaleInvoiceDetails.Where(a => a.SaleInvoiceID == id).ToList();

            return PartialView("_PartialViewSaleInvoiceDetails", Result);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            SaleReturnNote _searchObject = JsonConvert.DeserializeObject<SaleReturnNote>(searchModel);
            List<SaleReturnNote> _listObject = new List<SaleReturnNote>();
            _listObject = db.SaleReturnNotes.Where(a => a.IsActive == true && a.IsDeleted == false && a.CID == OrganizationID && a.SaleReturnType == SaleInvoiceTypeEnum.NetSaleInvoice).ToList();
            if (!String.IsNullOrEmpty(_searchObject.SaleReturnCode) && !String.IsNullOrEmpty(_searchObject.SaleReturnCode))
            {
                _listObject = _listObject.Where(r => r.SaleReturnCode != null && r.SaleReturnCode.ToString().ToLower().Contains(_searchObject.SaleReturnCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.SaleReturnCode))
            {
                _listObject = _listObject.Where(r => r.SaleReturnCode != null && r.SaleReturnCode.ToString().ToLower().Contains(_searchObject.SaleReturnCode)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
