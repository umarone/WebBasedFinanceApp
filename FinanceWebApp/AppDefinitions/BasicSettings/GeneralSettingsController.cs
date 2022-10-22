using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MudasirRehmanAlp.AppDAL;
using MudasirRehmanAlp.AppModels;
using MudasirRehmanAlp.Models;
using PagedList;

namespace MudasirRehmanAlp.AppDefinitions.BasicSettings
{
    public class GeneralSettingsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: GeneralSettings
        public ActionResult Index()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            GeneralSetting obj = new GeneralSetting();
            obj = db.GeneralSettings.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).FirstOrDefault();
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            ViewBag.GeneralSettingsObj = obj;
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(GeneralSetting generalSetting)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            var findObj = db.GeneralSettings.Find(generalSetting.Id);
            if (findObj != null)
            {
                findObj.OrganizationID = generalSetting.OrganizationID;
                findObj.GuarantorMaxNeed = generalSetting.GuarantorMaxNeed;
                findObj.StockMinimumQuantity = generalSetting.StockMinimumQuantity;
                findObj.BranchId = generalSetting.BranchId;
                findObj.IsActive = true;
                findObj.IsDeleted = false;
                findObj.UpdatedBy = UserID;
                findObj.UpdatedDate = DateTime.Now;
                db.Entry(findObj).State = EntityState.Modified;
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "General Setting has been updated successfully";
                db.SaveChanges();
            }
            else
            {
                
                generalSetting.IsActive = true;
                generalSetting.IsDeleted = false;
                generalSetting.CreatedBy = UserID;
                generalSetting.CreatedDate = DateTime.Now;
                db.GeneralSettings.Add(generalSetting);
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "General Setting has been saved successfully";
                db.SaveChanges();
            }

            return RedirectToAction("Index");


        }
        // GET: GeneralSettings/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSetting generalSetting = db.GeneralSettings.Find(id);
            if (generalSetting == null)
            {
                return HttpNotFound();
            }
            return View(generalSetting);
        }

        // GET: GeneralSettings/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: GeneralSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GeneralSetting generalSetting)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
               
                generalSetting.IsDeleted = false;
                generalSetting.CreatedBy = UserID;
                generalSetting.CreatedDate = DateTime.Now;
                db.GeneralSettings.Add(generalSetting);
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "General Setting has been saved successfully";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(generalSetting);
        }

        // GET: GeneralSettings/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSetting generalSetting = db.GeneralSettings.Find(id);
            if (generalSetting == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationID = new SelectList(db.OrganizationDefinitions, "Id", "OrganizationUnitName", generalSetting.OrganizationID);
            return View(generalSetting);
        }

        // POST: GeneralSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrganizationID,GuarantorMaxNeed,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,IsActive,DeletedBy,DeletedDate")] GeneralSetting generalSetting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(generalSetting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationID = new SelectList(db.OrganizationDefinitions, "Id", "OrganizationUnitName", generalSetting.OrganizationID);
            return View(generalSetting);
        }

        // GET: GeneralSettings/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GeneralSetting generalSetting = db.GeneralSettings.Find(id);
            if (generalSetting == null)
            {
                return HttpNotFound();
            }
            return View(generalSetting);
        }

        // POST: GeneralSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GeneralSetting generalSetting = db.GeneralSettings.Find(id);
            db.GeneralSettings.Remove(generalSetting);
            db.SaveChanges();
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
    }
}
