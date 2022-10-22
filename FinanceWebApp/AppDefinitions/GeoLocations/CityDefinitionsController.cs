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
    public class CityDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: CityDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            List<CityDefinition> listobj = new List<CityDefinition>();
            listobj = db.CityDefinitions.Where(a=>a.IsDeleted==false).ToList();
            PagedList<CityDefinition> model = new PagedList<CityDefinition>(listobj, page, pageSize);
            return View(model);
        }

        // GET: CityDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDefinition cityDefinition = db.CityDefinitions.Find(id);
            if (cityDefinition == null)
            {
                return HttpNotFound();
            }
            return View(cityDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDefinition cityDefinition = db.CityDefinitions.Find(id);
            if (cityDefinition == null)
            {
                return HttpNotFound();
            }
            return View(cityDefinition);
        }
        // GET: CityDefinitions/Create
        public ActionResult Create()
        {
            CityDefinition obj = new CityDefinition();
            obj.CityCode = dALCode.AutoGenerateCityCode();
            ViewBag.RegionList = db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.RegionName).ToList();

            return View(obj);
        }

        // POST: CityDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CityDefinition cityDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                cityDefinition.CityCode = dALCode.AutoGenerateCityCode();
                cityDefinition.IsDeleted = false;
                cityDefinition.CreatedBy = UserID;
                cityDefinition.CreatedDate = DateTime.Now;
                db.CityDefinitions.Add(cityDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "City has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(cityDefinition);
        }

        // GET: CityDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.RegionList = db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.RegionName).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDefinition cityDefinition = db.CityDefinitions.Find(id);
            if (cityDefinition == null)
            {
                return HttpNotFound();
            }
            return View(cityDefinition);
        }

        // POST: CityDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CityDefinition cityDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                cityDefinition.IsDeleted = false;
                cityDefinition.UpdatedBy = UserID;
                cityDefinition.UpdatedDate = DateTime.Now;
                db.Entry(cityDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "City has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(cityDefinition);
        }

        // GET: CityDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDefinition cityDefinition = db.CityDefinitions.Find(id);
            if (cityDefinition == null)
            {
                return HttpNotFound();
            }
            cityDefinition.IsDeleted = true;
            cityDefinition.DeletedBy = UserID;
            cityDefinition.DeletedDate = DateTime.Now;
            db.Entry(cityDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "City has been deleted successfully";
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
        public JsonResult GetRegionDetails(string ID)
        {
            long id = 0;
            if (ID != "")
            {
                id = Convert.ToInt64(ID);
            }

            var result = (from r in db.RegionDefinitions.Where(a => a.Id == id)
                          select new
                          {
                              CountryName = r.CountryDefinition.CountryName,
                              ProvinceName = r.ProvinceDefinition.ProvinceName
                          }).FirstOrDefault();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //------- Josn Functions
        public ActionResult Search(string searchModel)
        {
            CityDefinition _searchObject = JsonConvert.DeserializeObject<CityDefinition>(searchModel);
            List<CityDefinition> _listObject = new List<CityDefinition>();
            _listObject = db.CityDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();
            if (!String.IsNullOrEmpty(_searchObject.CityCode) && !String.IsNullOrEmpty(_searchObject.CityName) && _searchObject.RegionId !=0 &&  _searchObject.RegionId != null)
            {
                _listObject = _listObject.Where(r => r.CityCode != null && r.CityCode.ToString().ToLower().Contains(_searchObject.CityCode) || r.CityName.ToString().ToLower().Contains(_searchObject.CityName.ToLower().ToString()) || r.RegionId.ToString().ToLower().Contains(_searchObject.RegionId.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CityCode))
            {
                _listObject = _listObject.Where(r => r.CityCode != null && r.CityCode.ToString().ToLower().Contains(_searchObject.CityCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CityName))
            {

                _listObject = _listObject.Where(r => r.CityName != null && r.CityName.ToString().ToLower().Contains(_searchObject.CityName)).ToList();

            }
            else if (_searchObject.RegionId != 0 && _searchObject.RegionId != null)
            {

                _listObject = _listObject.Where(r => r.RegionId != null && r.RegionId.ToString().ToLower().Contains(_searchObject.RegionId.ToString())).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }

}
