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

namespace MudasirRehmanAlp.ProdustsSetup.BasicSettings
{
    public class CategoryDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: CategoryDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<CategoryDefinition> listobj = new List<CategoryDefinition>();
            listobj = db.CategoryDefinitions.Where(a=>a.IsDeleted==false && a.OrganizationID== OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<CategoryDefinition> model = new PagedList<CategoryDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: CategoryDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryDefinition categoryDefinition = db.CategoryDefinitions.Find(id);
            if (categoryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(categoryDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryDefinition categoryDefinition = db.CategoryDefinitions.Find(id);
            if (categoryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(categoryDefinition);
        }

        // GET: CategoryDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            CategoryDefinition obj = new CategoryDefinition();
            obj.CategoryCode = dALCode.AutoGenerateCategoryCode(OrganizationID/*, BranchId*/);          
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);

        }

        // POST: CategoryDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryDefinition categoryDefinition)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            long UserID = Convert.ToInt64(Session["UserID"]);
           
            if (ModelState.IsValid)
            {
                categoryDefinition.CategoryCode = dALCode.AutoGenerateCategoryCode(OrganizationID/*, BranchId*/);
                categoryDefinition.IsDeleted = false;
                categoryDefinition.CreatedBy = UserID;
                categoryDefinition.CreatedDate = DateTime.Now;
                db.CategoryDefinitions.Add(categoryDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Category has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(categoryDefinition);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            CategoryDefinition categoryDefinition = JsonConvert.DeserializeObject<CategoryDefinition>(model);

            if (ModelState.IsValid)
            {
                categoryDefinition.OrganizationID = OrganizationID;
                categoryDefinition.BranchId = BranchId;
                categoryDefinition.CategoryCode = dALCode.AutoGenerateCategoryCode(OrganizationID/*, BranchId*/);
                categoryDefinition.IsActive = true;
                categoryDefinition.IsDeleted = false;
                categoryDefinition.CreatedBy = UserID;
                categoryDefinition.CreatedDate = DateTime.Now;
                db.CategoryDefinitions.Add(categoryDefinition);
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return View(categoryDefinition);
        }
        // GET: CategoryDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryDefinition categoryDefinition = db.CategoryDefinitions.Find(id);
            if (categoryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(categoryDefinition);
        }

        // POST: CategoryDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryDefinition categoryDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            
            if (ModelState.IsValid)
            {
                
                categoryDefinition.IsDeleted = false;
                categoryDefinition.UpdatedBy = UserID;
                categoryDefinition.UpdatedDate = DateTime.Now;
                db.Entry(categoryDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Category has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(categoryDefinition);
        }

        // GET: CategoryDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryDefinition categoryDefinition = db.CategoryDefinitions.Find(id);
            if (categoryDefinition == null)
            {
                return HttpNotFound();
            }
            
            categoryDefinition.IsDeleted = true;
            categoryDefinition.DeletedBy = UserID;
            categoryDefinition.DeletedDate = DateTime.Now;
            db.Entry(categoryDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Category has been deleted successfully";
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
            long BrancheId = Convert.ToInt64(Session["BranchId"]);
            CategoryDefinition _searchObject = JsonConvert.DeserializeObject<CategoryDefinition>(searchModel);
            List<CategoryDefinition> _listObject = new List<CategoryDefinition>();
            _listObject = db.CategoryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID== OrganizationID && a.BranchId == BrancheId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.CategoryCode) && !String.IsNullOrEmpty(_searchObject.CategoryName))
            {
                _listObject = _listObject.Where(r => r.CategoryCode != null && r.CategoryCode.ToString().ToLower().Contains(_searchObject.CategoryCode) || r.CategoryName.ToString().ToLower().Contains(_searchObject.CategoryName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CategoryCode))
            {
                _listObject = _listObject.Where(r => r.CategoryCode != null && r.CategoryCode.ToString().ToLower().Contains(_searchObject.CategoryCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CategoryName))
            {

                _listObject = _listObject.Where(r => r.CategoryName != null && r.CategoryName.ToString().ToLower().Contains(_searchObject.CategoryName)).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }
}
