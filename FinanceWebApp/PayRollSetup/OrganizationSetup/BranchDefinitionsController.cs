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

namespace MudasirRehmanAlp.PayRollSetup.OrganizationSetup
{
    public class BranchDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: BranchDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            List<BranchDefinition> listobj = new List<BranchDefinition>();
            listobj = db.BranchDefinitions.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID).ToList();
            PagedList<BranchDefinition> model = new PagedList<BranchDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: BranchDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDefinition branchDefinition = db.BranchDefinitions.Find(id);
            if (branchDefinition == null)
            {
                return HttpNotFound();
            }
            return View(branchDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDefinition branchDefinition = db.BranchDefinitions.Find(id);
            if (branchDefinition == null)
            {
                return HttpNotFound();
            }
            return View(branchDefinition);
        }
        // GET: BranchDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            BranchDefinition obj = new BranchDefinition();
            obj.Code = dALCode.AutoGenerateBranchCode(OrganizationID);
            var OrganizationObj = dALCode.GetOrganizationDefinition(OrganizationID);
            obj.OrganizationId = OrganizationObj.Id;
            ViewBag.OrganizationUnitName = OrganizationObj.OrganizationUnitName;
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            return View(obj);

        }

        // POST: BranchDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchDefinition branchDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            if (ModelState.IsValid)
            {
                branchDefinition.Code = dALCode.AutoGenerateBranchCode(OrganizationID);
                branchDefinition.IsDeleted = false;
                branchDefinition.CreatedBy = UserID;
                branchDefinition.CreatedDate = DateTime.Now;
                db.BranchDefinitions.Add(branchDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Branch has been saved successfully";

                return RedirectToAction("Index");
            }
            return View(branchDefinition);
        }

        // GET: BranchDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDefinition branchDefinition = db.BranchDefinitions.Find(id);
            if (branchDefinition == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            return View(branchDefinition);
        }

        // POST: BranchDefinitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BranchDefinition branchDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                branchDefinition.IsDeleted = false;
                branchDefinition.UpdatedBy = UserID;
                branchDefinition.UpdatedDate = DateTime.Now;
                db.Entry(branchDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Branch has been updated successfully";
                return RedirectToAction("Index");
            }
            
            return View(branchDefinition);
        }

        // GET: BranchDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchDefinition branchDefinition = db.BranchDefinitions.Find(id);
            if (branchDefinition == null)
            {
                return HttpNotFound();
            }
           
            branchDefinition.IsDeleted = true;
            branchDefinition.DeletedBy = UserID;
            branchDefinition.DeletedDate = DateTime.Now;
            db.Entry(branchDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Branch has been deleted successfully";
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
        public JsonResult LoadProvince(string ID)
        {
            long countryid = 0;
            if (ID != "")
            {
                countryid = Convert.ToInt64(ID);
            }
            var result = (from p in db.ProvinceDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.CountryId == countryid
                          select new
                          {
                              value = p.Id,
                              label = p.ProvinceName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadRegion(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.ProvinceId == id
                          select new
                          {
                              value = p.Id,
                              label = p.RegionName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCity(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.RegionId == id
                          select new
                          {
                              value = p.CityId,
                              label = p.CityName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadArea(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }
            var result = (from p in db.AreaDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false)

                          where p.CityId == id
                          select new
                          {
                              value = p.Id,
                              label = p.AreaName
                          }
                          ).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Search(string searchModel)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            BranchDefinition _searchObject = JsonConvert.DeserializeObject<BranchDefinition>(searchModel);
            List<BranchDefinition> _listObject = new List<BranchDefinition>();
            _listObject = db.BranchDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == OrganizationID).ToList();
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
