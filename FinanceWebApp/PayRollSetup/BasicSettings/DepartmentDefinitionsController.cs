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
    public class DepartmentDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: DepartmentDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<DepartmentDefinition> listobj = new List<DepartmentDefinition>();
            listobj = db.DepartmentDefinitions.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<DepartmentDefinition> model = new PagedList<DepartmentDefinition>(listobj, page, pageSize);
            return View(model);
        }

        // GET: DepartmentDefinitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentDefinition departmentDefinition = db.DepartmentDefinitions.Find(id);
            if (departmentDefinition == null)
            {
                return HttpNotFound();
            }
            return View(departmentDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentDefinition departmentDefinition = db.DepartmentDefinitions.Find(id);
            if (departmentDefinition == null)
            {
                return HttpNotFound();
            }
            return View(departmentDefinition);
        }
        // GET: DepartmentDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            DepartmentDefinition obj = new DepartmentDefinition();
            obj.Code = dALCode.AutoDepartmentDefinitionsCode(OrganizationID);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: DepartmentDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentDefinition departmentDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
               
                departmentDefinition.IsDeleted = false;
                departmentDefinition.CreatedBy = UserID;
                departmentDefinition.CreatedDate = DateTime.Now;
                db.DepartmentDefinitions.Add(departmentDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Department has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(departmentDefinition);
        }

        // GET: DepartmentDefinitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentDefinition departmentDefinition = db.DepartmentDefinitions.Find(id);
            if (departmentDefinition == null)
            {
                return HttpNotFound();
            }
            return View(departmentDefinition);
        }

        // POST: DepartmentDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentDefinition departmentDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
               
                departmentDefinition.IsDeleted = false;
                departmentDefinition.UpdatedBy = UserID;
                departmentDefinition.UpdatedDate = DateTime.Now;
                db.Entry(departmentDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Department has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(departmentDefinition);
        }

        // GET: DepartmentDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartmentDefinition departmentDefinition = db.DepartmentDefinitions.Find(id);
            if (departmentDefinition == null)
            {
                return HttpNotFound();
            }
            departmentDefinition.IsDeleted = true;
            departmentDefinition.DeletedBy = UserID;
            departmentDefinition.DeletedDate = DateTime.Now;
            db.Entry(departmentDefinition).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Department has been deleted successfully";
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
            DepartmentDefinition _searchObject = JsonConvert.DeserializeObject<DepartmentDefinition>(searchModel);
            List<DepartmentDefinition> _listObject = new List<DepartmentDefinition>();
            _listObject = db.DepartmentDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
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
