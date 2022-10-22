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

namespace MudasirRehmanAlp.AppDefinitions.GeoLocations
{
    public class AreaDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: AreaDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            List<AreaDefinition> listobj = new List<AreaDefinition>();
            listobj = db.AreaDefinitions.Where(a=>a.IsDeleted==false).ToList();
            PagedList<AreaDefinition> model = new PagedList<AreaDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: AreaDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaDefinition areaDefinition = db.AreaDefinitions.Find(id);
            if (areaDefinition == null)
            {
                return HttpNotFound();
            }
            return View(areaDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaDefinition areaDefinition = db.AreaDefinitions.Find(id);
            if (areaDefinition == null)
            {
                return HttpNotFound();
            }
            return View(areaDefinition);
        }
        // GET: AreaDefinitions/Create
        public ActionResult Create()
        {
            AreaDefinition obj = new AreaDefinition();
            obj.AreaCode = dALCode.AutoGenerateAreaCode();
            ViewBag.CityList = db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CityName).ToList();
            return View(obj);

        }

        // POST: AreaDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AreaDefinition areaDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                areaDefinition.AreaCode = dALCode.AutoGenerateAreaCode();
                areaDefinition.IsDeleted = false;
                areaDefinition.CreatedBy = UserID;
                areaDefinition.CreatedDate = DateTime.Now;
                db.AreaDefinitions.Add(areaDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Area has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(areaDefinition);
        }

        // GET: AreaDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.CityList = db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CityName).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaDefinition areaDefinition = db.AreaDefinitions.Find(id);
            if (areaDefinition == null)
            {
                return HttpNotFound();
            }
            return View(areaDefinition);
        }

        // POST: AreaDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AreaDefinition areaDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                areaDefinition.IsDeleted = false;
                areaDefinition.UpdatedBy = UserID;
                areaDefinition.UpdatedDate = DateTime.Now;
                db.Entry(areaDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Area has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(areaDefinition);
        }

        // GET: AreaDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AreaDefinition areaDefinition = db.AreaDefinitions.Find(id);
            if (areaDefinition == null)
            {
                return HttpNotFound();
            }
            areaDefinition.IsDeleted = true;
            areaDefinition.UpdatedBy = UserID;
            areaDefinition.UpdatedDate = DateTime.Now;
            db.Entry(areaDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Area has been deleted successfully";
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
        //--------------- Json Function
        public JsonResult GetCityDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var result = (from r in db.CityDefinitions.Where(a => a.CityId == id)
                          select new
                          {
                              RegionName = r.RegionDefinition.RegionName,
                              CountryName = r.RegionDefinition.CountryDefinition.CountryName,
                              ProvinceName = r.RegionDefinition.ProvinceDefinition.ProvinceName
                          }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------- Josn Functions
        public ActionResult Search(string searchModel)
        {
            AreaDefinition _searchObject = JsonConvert.DeserializeObject<AreaDefinition>(searchModel);
            List<AreaDefinition> _listObject = new List<AreaDefinition>();
            _listObject = db.AreaDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();
            if (!String.IsNullOrEmpty(_searchObject.AreaCode) && !String.IsNullOrEmpty(_searchObject.AreaCode) && _searchObject.CityId != 0 && _searchObject.CityId != null)
            {
                _listObject = _listObject.Where(r => r.AreaCode != null && r.AreaCode.ToString().ToLower().Contains(_searchObject.AreaCode) || r.AreaName.ToString().ToLower().Contains(_searchObject.AreaName.ToLower().ToString()) || r.CityId.ToString().ToLower().Contains(_searchObject.CityId.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.AreaCode))
            {
                _listObject = _listObject.Where(r => r.AreaCode != null && r.AreaCode.ToString().ToLower().Contains(_searchObject.AreaCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.AreaName))
            {

                _listObject = _listObject.Where(r => r.AreaName != null && r.AreaName.ToString().ToLower().Contains(_searchObject.AreaName)).ToList();

            }
            else if (_searchObject.CityId != 0 && _searchObject.CityId != null)
            {

                _listObject = _listObject.Where(r => r.CityId != null && r.CityId.ToString().ToLower().Contains(_searchObject.CityId.ToString())).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }
}
