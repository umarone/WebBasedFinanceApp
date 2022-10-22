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
using Newtonsoft.Json;
using PagedList;

namespace MudasirRehmanAlp.PayRollSetup.BasicSettings
{
    public class DesignationDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: DesignationDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<DesignationDefinition> listobj = new List<DesignationDefinition>();
            listobj = db.DesignationDefinitions.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<DesignationDefinition> model = new PagedList<DesignationDefinition>(listobj, page, pageSize);
            return View(model);
        }

        // GET: DesignationDefinitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationDefinition designationDefinition = db.DesignationDefinitions.Find(id);
            if (designationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(designationDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationDefinition designationDefinition = db.DesignationDefinitions.Find(id);
            if (designationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(designationDefinition);
        }
        // GET: DesignationDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            DesignationDefinition obj = new DesignationDefinition();
            obj.Code = dALCode.AutoDesignationDefinitionsCode(OrganizationID);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: DesignationDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DesignationDefinition designationDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
               
                designationDefinition.IsDeleted = false;
                designationDefinition.CreatedBy = UserID;
                designationDefinition.CreatedDate = DateTime.Now;
                db.DesignationDefinitions.Add(designationDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Designation has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(designationDefinition);
        }

        // GET: DesignationDefinitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationDefinition designationDefinition = db.DesignationDefinitions.Find(id);
            if (designationDefinition == null)
            {
                return HttpNotFound();
            }
            return View(designationDefinition);
        }

        // POST: DesignationDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DesignationDefinition designationDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                designationDefinition.IsDeleted = false;
                designationDefinition.UpdatedBy = UserID;
                designationDefinition.UpdatedDate = DateTime.Now;
                db.Entry(designationDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Designation has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(designationDefinition);
        }

        // GET: DesignationDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignationDefinition designationDefinition = db.DesignationDefinitions.Find(id);
            if (designationDefinition == null)
            {
                return HttpNotFound();
            }
            designationDefinition.IsDeleted = true;
            designationDefinition.DeletedBy = UserID;
            designationDefinition.DeletedDate = DateTime.Now;
            db.Entry(designationDefinition).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Designation has been deleted successfully";
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
        //------- Json Functions
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            DesignationDefinition _searchObject = JsonConvert.DeserializeObject<DesignationDefinition>(searchModel);
            List<DesignationDefinition> _listObject = new List<DesignationDefinition>();
            _listObject = db.DesignationDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
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
