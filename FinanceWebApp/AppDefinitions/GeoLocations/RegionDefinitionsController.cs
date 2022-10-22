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
    public class RegionDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: RegionDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
           
            List<RegionDefinition> listobj = new List<RegionDefinition>();
            listobj = db.RegionDefinitions.Where(a => a.IsDeleted == false).ToList();
            PagedList<RegionDefinition> model = new PagedList<RegionDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: RegionDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionDefinition regionDefinition = db.RegionDefinitions.Find(id);
            if (regionDefinition == null)
            {
                return HttpNotFound();
            }
            return View(regionDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionDefinition regionDefinition = db.RegionDefinitions.Find(id);
            if (regionDefinition == null)
            {
                return HttpNotFound();
            }
            return View(regionDefinition);
        }
      

        // GET: RegionDefinitions/Create
        public ActionResult Create()
        {
            RegionDefinition obj = new RegionDefinition();
            obj.RegionCode = dALCode.AutoGenerateRegionCode();
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();

            return View(obj);
        }

        // POST: RegionDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegionDefinition regionDefinition)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {
                regionDefinition.RegionCode = dALCode.AutoGenerateRegionCode();
                regionDefinition.IsDeleted = false;
                regionDefinition.CreatedBy = UserID;
                regionDefinition.CreatedDate = DateTime.Now;
                db.RegionDefinitions.Add(regionDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Region has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(regionDefinition);
        }

        // GET: RegionDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionDefinition regionDefinition = db.RegionDefinitions.Find(id);
            if (regionDefinition == null)
            {
                return HttpNotFound();
            }
            return View(regionDefinition);
        }

        // POST: RegionDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegionDefinition regionDefinition)
        {

            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                regionDefinition.IsDeleted = false;
                regionDefinition.UpdatedBy = UserID;
                regionDefinition.UpdatedDate = DateTime.Now;
                db.Entry(regionDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Region has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(regionDefinition);
        }

        // GET: RegionDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegionDefinition regionDefinition = db.RegionDefinitions.Find(id);
            if (regionDefinition == null)
            {
                return HttpNotFound();
            }
            regionDefinition.IsDeleted = true;
            regionDefinition.DeletedBy = UserID;
            regionDefinition.DeletedDate = DateTime.Now;
            db.Entry(regionDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Region has been deleted successfully";
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

        //------- Josn Functions
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
       
        public ActionResult Search(string searchModel)
        {
            RegionDefinition _searchObject = JsonConvert.DeserializeObject<RegionDefinition>(searchModel);
            List<RegionDefinition> _listObject = new List<RegionDefinition>();
            _listObject = db.RegionDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();
           
            if (!String.IsNullOrEmpty(_searchObject.RegionCode) && !String.IsNullOrEmpty(_searchObject.RegionName) && _searchObject.ProvinceId !=0 && _searchObject.ProvinceId != null)
            {
                _listObject = _listObject.Where(r => r.RegionCode != null && r.RegionCode.ToString().ToLower().Contains(_searchObject.RegionCode) || r.RegionName.ToString().ToLower().Contains(_searchObject.RegionName) || r.ProvinceId.ToString().ToLower().Contains(_searchObject.CountryId.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.RegionCode))
            {
                _listObject = _listObject.Where(r => r.RegionCode != null && r.RegionCode.ToString().ToLower().Contains(_searchObject.RegionCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.RegionName))
            {
                _listObject = _listObject.Where(r => r.RegionName != null && r.RegionName.ToString().ToLower().Contains(_searchObject.RegionName)).ToList();
            }
            else if (_searchObject.ProvinceId !=0 && _searchObject.ProvinceId != null)
            {
                _listObject = _listObject.Where(r => r.ProvinceId != null && r.ProvinceId.ToString().ToLower().Contains(_searchObject.ProvinceId.ToString())).ToList();
            }
            return PartialView("_Search", _listObject);
        }


    }
}
