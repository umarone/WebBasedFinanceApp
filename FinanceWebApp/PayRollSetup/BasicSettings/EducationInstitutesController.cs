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
    public class EducationInstitutesController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: EducationInstitutes
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<EducationInstitute> listobj = new List<EducationInstitute>();
            listobj = db.EducationInstitutes.Where(a => a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
            PagedList<EducationInstitute> model = new PagedList<EducationInstitute>(listobj, page, pageSize);
            return View(model);
        }

        // GET: EducationInstitutes/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationInstitute educationInstitute = db.EducationInstitutes.Find(id);
            if (educationInstitute == null)
            {
                return HttpNotFound();
            }
            return View(educationInstitute);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationInstitute educationInstitute = db.EducationInstitutes.Find(id);
            if (educationInstitute == null)
            {
                return HttpNotFound();
            }
            return View(educationInstitute);
        }
        // GET: EducationInstitutes/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            EducationInstitute obj = new EducationInstitute();
            obj.Code = dALCode.AutoEducationInstitutesCode(OrganizationID);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: EducationInstitutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EducationInstitute educationInstitute)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                educationInstitute.IsDeleted = false;
                educationInstitute.CreatedBy = UserID;
                educationInstitute.CreatedDate = DateTime.Now;
                db.EducationInstitutes.Add(educationInstitute);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Education Institutes has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(educationInstitute);
        }

        // GET: EducationInstitutes/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationInstitute educationInstitute = db.EducationInstitutes.Find(id);
            if (educationInstitute == null)
            {
                return HttpNotFound();
            }
           
            return View(educationInstitute);
        }

        // POST: EducationInstitutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EducationInstitute educationInstitute)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                
                educationInstitute.IsDeleted = false;
                educationInstitute.UpdatedBy = UserID;
                educationInstitute.UpdatedDate = DateTime.Now;
                db.Entry(educationInstitute).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Education Institute has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(educationInstitute);
        }

        // GET: EducationInstitutes/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EducationInstitute educationInstitute = db.EducationInstitutes.Find(id);
            if (educationInstitute == null)
            {
                return HttpNotFound();
            }
            educationInstitute.IsDeleted = true;
            educationInstitute.DeletedBy = UserID;
            educationInstitute.DeletedDate = DateTime.Now;
            db.Entry(educationInstitute).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Education Institute has been deleted successfully";
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
            EducationInstitute _searchObject = JsonConvert.DeserializeObject<EducationInstitute>(searchModel);
            List<EducationInstitute> _listObject = new List<EducationInstitute>();
            _listObject = db.EducationInstitutes.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationID == OrganizationID).ToList();
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
