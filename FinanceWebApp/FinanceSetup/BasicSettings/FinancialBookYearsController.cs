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
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.FinanceSetup.BasicSettings
{
    public class FinancialBookYearsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: FinancialBookYears

        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<FinancialBookYear> listobj = new List<FinancialBookYear>();
            listobj = db.FinancialBookYears.Where(a=>a.IsDeleted==false && a.OrganizationID== OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<FinancialBookYear> model = new PagedList<FinancialBookYear>(listobj, page, pageSize);
            return View(model);
        }
        // GET: FinancialBookYears/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialBookYear financialBookYear = db.FinancialBookYears.Find(id);
            if (financialBookYear == null)
            {
                return HttpNotFound();
            }
            return View(financialBookYear);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialBookYear financialBookYear = db.FinancialBookYears.Find(id);
            if (financialBookYear == null)
            {
                return HttpNotFound();
            }
            return View(financialBookYear);
        }
        // GET: FinancialBookYears/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            FinancialBookYear obj = new FinancialBookYear();
            obj.YearCode = dALCode.AutoGenerateFinancialBookYearsCode(OrganizationID, BranchId);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: FinancialBookYears/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FinancialBookYear financialBookYear)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                financialBookYear.IsDeleted = false;
                financialBookYear.CreatedBy = UserID;
                financialBookYear.CreatedDate = DateTime.Now;              
                db.FinancialBookYears.Add(financialBookYear);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Financial Book Year has been saved successfully";
                return RedirectToAction("Index");
            }
            return View(financialBookYear);
        }

        // GET: FinancialBookYears/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialBookYear financialBookYear = db.FinancialBookYears.Find(id);
            if (financialBookYear == null)
            {
                return HttpNotFound();
            }
            
            return View(financialBookYear);
        }

        // POST: FinancialBookYears/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FinancialBookYear financialBookYear)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                financialBookYear.IsDeleted = false;
                financialBookYear.UpdatedBy = UserID;
                financialBookYear.UpdatedDate = DateTime.Now;
                db.Entry(financialBookYear).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Financial Book Year has been updated successfully";
                return RedirectToAction("Index");
            }
           
            return View(financialBookYear);
        }

        // GET: FinancialBookYears/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinancialBookYear financialBookYear = db.FinancialBookYears.Find(id);
            if (financialBookYear == null)
            {
                return HttpNotFound();
            }
           
            financialBookYear.IsDeleted = true;
            financialBookYear.DeletedBy = UserID;
            financialBookYear.DeletedDate = DateTime.Now;
            db.Entry(financialBookYear).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Financial Book Year has been deleted successfully";
            return RedirectToAction("Index");
        }
        // GET: FinancialBookYears/MakeDefault/5
        public ActionResult MakeDefault(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultedListFinancialBookYear();
            FinancialBookYear financialBookYear = db.FinancialBookYears.Find(id);
            financialBookYear.IsDefault = true;
            financialBookYear.DefaultBy = UserID;
            financialBookYear.DefaultDate = DateTime.Now;

            if (financialBookYear == null)
            {
                return HttpNotFound();
            }
           
            db.Entry(financialBookYear).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Financial Book Year has been defaulted set successfully";
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
        public void DefaultedListFinancialBookYear()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var objList = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId== BranchId).ToList();
            foreach (var item in objList)
            {
                var obj = db.FinancialBookYears.Find(item.Id);
                obj.IsDefault = false;
                db.Entry(obj).State=EntityState.Modified;
                db.SaveChanges();
            }
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            FinancialBookYear _searchObject = JsonConvert.DeserializeObject<FinancialBookYear>(searchModel);
            List<FinancialBookYear> _listObject = new List<FinancialBookYear>();
            _listObject = db.FinancialBookYears.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId==BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.YearCode) && !String.IsNullOrEmpty(_searchObject.YearName))
            {
                _listObject = _listObject.Where(r => r.YearCode != null && r.YearCode.ToString().ToLower().Contains(_searchObject.YearCode) || r.YearName.ToString().ToLower().Contains(_searchObject.YearName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.YearCode))
            {
                _listObject = _listObject.Where(r => r.YearCode != null && r.YearCode.ToString().ToLower().Contains(_searchObject.YearCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.YearName))
            {
                _listObject = _listObject.Where(r => r.YearName != null && r.YearName.ToString().ToLower().Contains(_searchObject.YearName)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
