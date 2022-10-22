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

namespace MudasirRehmanAlp.PurchaseSetup.BasicSettings
{
    public class CurrencyDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: CurrencyDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);

            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<CurrencyDefinition> listobj = new List<CurrencyDefinition>();
            listobj = db.CurrencyDefinitions.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).OrderByDescending(a=>a.CurrencyID).ToList();
            PagedList<CurrencyDefinition> model = new PagedList<CurrencyDefinition>(listobj, page, pageSize);
            return View(model);
        }

        // GET: CurrencyDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyDefinition currencyDefinition = db.CurrencyDefinitions.Find(id);
            if (currencyDefinition == null)
            {
                return HttpNotFound();
            }
            return View(currencyDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyDefinition currencyDefinition = db.CurrencyDefinitions.Find(id);
            if (currencyDefinition == null)
            {
                return HttpNotFound();
            }
            return View(currencyDefinition);
        }
        // GET: CurrencyDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            CurrencyDefinition obj = new CurrencyDefinition();
            obj.CurrencyCode = dALCode.AutoGenerateCurrencyCode(OrganizationID/*, BranchId*/);

            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;

            return View(obj);
        }

        // POST: CurrencyDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurrencyDefinition currencyDefinition)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                currencyDefinition.CurrencyCode = dALCode.AutoGenerateCurrencyCode(OrganizationID/*, BranchId*/);
                currencyDefinition.IsDeleted = false;
                currencyDefinition.CreatedBy = UserID;
                currencyDefinition.CreatedDate = DateTime.Now;
                db.CurrencyDefinitions.Add(currencyDefinition);
               
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Currency has been saved successfully";
                return RedirectToAction("Index");
            }


            return View(currencyDefinition);
        }

        // GET: CurrencyDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyDefinition currencyDefinition = db.CurrencyDefinitions.Find(id);
            if (currencyDefinition == null)
            {
                return HttpNotFound();
            }

            return View(currencyDefinition);
        }

        // POST: CurrencyDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurrencyDefinition currencyDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
               
                currencyDefinition.IsDeleted = false;
                currencyDefinition.UpdatedBy = UserID;
                currencyDefinition.UpdatedDate = DateTime.Now;
                db.Entry(currencyDefinition).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Currency has been updated successfully";
                return RedirectToAction("Index");
            }

            return View(currencyDefinition);
        }

        // GET: CurrencyDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrencyDefinition currencyDefinition = db.CurrencyDefinitions.Find(id);
            if (currencyDefinition == null)
            {
                return HttpNotFound();
            }
            currencyDefinition.IsDeleted = true;
            currencyDefinition.DeletedBy = UserID;
            currencyDefinition.DeletedDate = DateTime.Now;
            db.Entry(currencyDefinition).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Currency has been deleted successfully";
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
        // GET: CurrencyDefinitions/MakeDefault/5
        public ActionResult MakeDefault(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DefaultedList();
            CurrencyDefinition currencyDefinition = db.CurrencyDefinitions.Find(id);
            currencyDefinition.IsDefault = true;
            currencyDefinition.DefaultBy = UserID;
            currencyDefinition.DefaultDate = DateTime.Now;

            if (currencyDefinition == null)
            {
                return HttpNotFound();
            }

            db.Entry(currencyDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Currency has been defaulted set successfully";
            return RedirectToAction("Index");
        }
        public void DefaultedList()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            var objList = db.CurrencyDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId == BranchId).ToList();

            foreach (var item in objList)
            {
                var obj = db.CurrencyDefinitions.Find(item.CurrencyID);
                obj.IsDefault = false;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

            }
        }
        //------- Json Functions
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            CurrencyDefinition _searchObject = JsonConvert.DeserializeObject<CurrencyDefinition>(searchModel);
            List<CurrencyDefinition> _listObject = new List<CurrencyDefinition>();
            _listObject = db.CurrencyDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID && a.BranchId==BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.CurrencyCode) && !String.IsNullOrEmpty(_searchObject.CurrencyName))
            {
                _listObject = _listObject.Where(r => r.CurrencyCode != null && r.CurrencyCode.ToString().ToLower().Contains(_searchObject.CurrencyCode) || r.CurrencyName.ToString().ToLower().Contains(_searchObject.CurrencyName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CurrencyCode))
            {
                _listObject = _listObject.Where(r => r.CurrencyCode != null && r.CurrencyCode.ToString().ToLower().Contains(_searchObject.CurrencyCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CurrencyName))
            {
                _listObject = _listObject.Where(r => r.CurrencyName != null && r.CurrencyName.ToString().ToLower().Contains(_searchObject.CurrencyName)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
