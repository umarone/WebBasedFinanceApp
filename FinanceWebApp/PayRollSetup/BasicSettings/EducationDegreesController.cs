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
    public class EducationDegreesController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: EducationDegrees
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<EducationDegree> listobj = new List<EducationDegree>();
            listobj = db.EducationDegrees.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<EducationDegree> model = new PagedList<EducationDegree>(listobj, page, pageSize);
            return View(model);
        }

        // GET: EducationDegrees/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDegree educationDegree = db.EducationDegrees.Find(id);
            if (educationDegree == null)
            {
                return HttpNotFound();
            }
            return View(educationDegree);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDegree educationDegree = db.EducationDegrees.Find(id);
            if (educationDegree == null)
            {
                return HttpNotFound();
            }
            return View(educationDegree);
        }
        // GET: EducationDegrees/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            EducationDegree obj = new EducationDegree();
            obj.Code = dALCode.AutoEducationDegreesCode(OrganizationID);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: EducationDegrees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EducationDegree educationDegree)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                educationDegree.IsDeleted = false;
                educationDegree.CreatedBy = UserID;
                educationDegree.CreatedDate = DateTime.Now;
                db.EducationDegrees.Add(educationDegree);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Education Degree has been saved successfully";
                return RedirectToAction("Index");
            }
            return View(educationDegree);
        }

        // GET: EducationDegrees/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDegree educationDegree = db.EducationDegrees.Find(id);
            if (educationDegree == null)
            {
                return HttpNotFound();
            }
            return View(educationDegree);
        }

        // POST: EducationDegrees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EducationDegree educationDegree)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
              
                educationDegree.IsDeleted = false;
                educationDegree.UpdatedBy = UserID;
                educationDegree.UpdatedDate = DateTime.Now;
                db.Entry(educationDegree).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Education Degree has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(educationDegree);
        }

        // GET: EducationDegrees/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationDegree educationDegree = db.EducationDegrees.Find(id);
          
            if (educationDegree == null)
            {
                return HttpNotFound();
            }
            educationDegree.IsDeleted = true;
            educationDegree.DeletedBy = UserID;
            educationDegree.DeletedDate = DateTime.Now;
            db.Entry(educationDegree).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Education Degree has been deleted successfully";
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
            EducationDegree _searchObject = JsonConvert.DeserializeObject<EducationDegree>(searchModel);
            List<EducationDegree> _listObject = new List<EducationDegree>();
            _listObject = db.EducationDegrees.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
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
