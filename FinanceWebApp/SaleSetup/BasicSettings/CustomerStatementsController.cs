using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using MudasirRehmanAlp.ModelsView;
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.SaleSetup.BasicSettings
{
    public class CustomerStatementsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        CustomerStatementsDAL customerStatementsDAL = new CustomerStatementsDAL();
        // GET: CustomerStatements
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<CustomerStatement> listobj = new List<CustomerStatement>();
            listobj = db.CustomerStatements.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID &&  a.BranchId == BranchId).OrderByDescending(a=>a.Id).ToList();
            PagedList<CustomerStatement> model = new PagedList<CustomerStatement>(listobj, page, pageSize);
            return View(model);

        }
        // GET: CustomerStatement/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatement customerStatement = db.CustomerStatements.Find(id);
            if (customerStatement == null)
            {
                return HttpNotFound();
            }
            var ListObj = db.GuarantorDefinitions.Where(a => a.IsDeleted == false && a.CustomerStatementId == id).ToList();
            ViewBag.GuarantorDefinitionsList = ListObj;
            return View(customerStatement);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatement customerStatement = db.CustomerStatements.Find(id);
            if (customerStatement == null)
            {
                return HttpNotFound();
            }
            var ListObj = db.GuarantorDefinitions.Where(a => a.IsDeleted == false && a.CustomerStatementId == id).ToList();
            ViewBag.GuarantorDefinitionsList = ListObj;
            return View(customerStatement);
        }
        // GET: CustomerStatement/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            CustomerStatement obj = new CustomerStatement();
            obj.Code = dALCode.AutoGenerateCustomerStatementCode(OrganizationID, BranchId);
            obj.CAFNo = dALCode.AutoGenerateCustomerStatementCAFNo(OrganizationID, BranchId);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
           

            var findGeneralSettings = db.GeneralSettings.Where(a => a.IsActive == true && a.IsDeleted == false  && a.OrganizationID == OrganizationID).FirstOrDefault();
           
            if (findGeneralSettings != null)
            {
                ViewBag.GuarantorMaxNeed = findGeneralSettings.GuarantorMaxNeed;
            }
            else
            {
                TempData["Validation"] = "warning";
                TempData["ErrorMessage"] = "Please add Guarantor Max Need befor you add Customer Statements.";
            }
            return View(obj);
        }

        // POST: CustomerStatement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public ActionResult Create(string ObjMasterItem, string ObjChilds, HttpPostedFileBase[] uploadCustomerImage)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase Customerfile = Request.Files["uploadCustomerImage"];


            var getmessage = customerStatementsDAL.Add(ObjMasterItem, ObjChilds, Customerfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Customer Statement has been saved successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }

        // GET: CustomerStatement/Edit/5
        public ActionResult Edit(long? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatement customerStatement = db.CustomerStatements.Find(id);
            if (customerStatement == null)
            {
                return HttpNotFound();
            }
           
            var ListObj = db.GuarantorDefinitions.Where(a => a.IsDeleted == false && a.CustomerStatementId == id).ToList();
            ViewBag.GuarantorDefinitionsList = ListObj;
            return View(customerStatement);
        }

        // POST: CustomerStatement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(string ObjMasterItem, string ObjChilds, HttpPostedFileBase[] uploadCustomerImage)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            HttpPostedFileBase Customerfile = Request.Files["uploadCustomerImage"];


            var getmessage = customerStatementsDAL.Update(ObjMasterItem, ObjChilds, Customerfile, UserID);
            if (getmessage != "")
            {
                TempData["Validation"] = false;
                TempData["ErrorMessage"] = getmessage;
                return Json(getmessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Customer Statement has been updated successfully";
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
        }
        
        // GET: CustomerStatements/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerStatement CustomerStatement = db.CustomerStatements.Find(id);
            if (CustomerStatement == null)
            {
                return HttpNotFound();
            }
           
            CustomerStatement.DeletedBy = UserID;
            CustomerStatement.DeletedDate = DateTime.Now;
            CustomerStatement.IsDeleted = true;
            db.Entry(CustomerStatement).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Customer Statement has been deleted successfully";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult AccountCheckIn(string ID)
        {
            long id = 0;
            string Message = "";
            if (!String.IsNullOrEmpty(ID))
            {
                id = Convert.ToInt64(ID);
            }

            var Result = db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false && a.AccountID == id).FirstOrDefault();
            if (Result != null)
            {
                Message = "Yes";
            }
            else
            {
                Message = "No";
            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            CustomerStatement _searchObject = JsonConvert.DeserializeObject<CustomerStatement>(searchModel);
            List<CustomerStatement> _listObject = new List<CustomerStatement>();
            _listObject = db.CustomerStatements.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            if (!String.IsNullOrEmpty(_searchObject.Code) && !String.IsNullOrEmpty(_searchObject.Name))
            {
                _listObject = _listObject.Where(r => r.Code != null && r.Code.ToString().ToLower().Contains(_searchObject.Code) || r.Name.ToString().ToLower().Contains(_searchObject.Name)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.Code))
            {
                _listObject = _listObject.Where(r => r.Code != null && r.Code.ToString().ToLower().Contains(_searchObject.Code)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.Name))
            {
                _listObject = _listObject.Where(r => r.Name != null && r.Name.ToString().ToLower().Contains(_searchObject.Name)).ToList();
            }
            return PartialView("_Search", _listObject);
        }


    }
}
