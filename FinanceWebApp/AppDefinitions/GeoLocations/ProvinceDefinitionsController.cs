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
    public class ProvinceDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: ProvinceDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            List<ProvinceDefinition> listobj = new List<ProvinceDefinition>();
            listobj = db.ProvinceDefinitions.Where(a=>a.IsDeleted==false).ToList();
            PagedList<ProvinceDefinition> model = new PagedList<ProvinceDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: ProvinceDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProvinceDefinition provinceDefinition = db.ProvinceDefinitions.Find(id);
            if (provinceDefinition == null)
            {
                return HttpNotFound();
            }
            return View(provinceDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProvinceDefinition provinceDefinition = db.ProvinceDefinitions.Find(id);
            if (provinceDefinition == null)
            {
                return HttpNotFound();
            }
            return View(provinceDefinition);
        }
        // GET: ProvinceDefinitions/Create
        public ActionResult Create()
        {
            ProvinceDefinition obj = new ProvinceDefinition();
            obj.ProvinceCode = dALCode.AutoGenerateProvinceCodeCode();
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();

            return View(obj);
        }

        // POST: ProvinceDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProvinceDefinition provinceDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                provinceDefinition.ProvinceCode = dALCode.AutoGenerateProvinceCodeCode();
                provinceDefinition.IsDeleted = false;
                provinceDefinition.CreatedBy = UserID;
                provinceDefinition.CreatedDate = DateTime.Now;
                db.ProvinceDefinitions.Add(provinceDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Province has been saved successfully";
                return RedirectToAction("Index");
            }

            return View(provinceDefinition);
        }

        // GET: ProvinceDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProvinceDefinition provinceDefinition = db.ProvinceDefinitions.Find(id);
            if (provinceDefinition == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryList = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).OrderBy(a => a.CountryName).ToList();
            return View(provinceDefinition);
        }

        // POST: ProvinceDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProvinceDefinition provinceDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {

                provinceDefinition.IsDeleted = false;
                provinceDefinition.UpdatedBy = UserID;
                provinceDefinition.UpdatedDate = DateTime.Now;
                db.Entry(provinceDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Province has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(provinceDefinition);
        }

        // GET: ProvinceDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProvinceDefinition provinceDefinition = db.ProvinceDefinitions.Find(id);
            if (provinceDefinition == null)
            {
                return HttpNotFound();
            }

            provinceDefinition.IsDeleted = true;
            provinceDefinition.DeletedBy = UserID;
            provinceDefinition.DeletedDate = DateTime.Now;
            db.Entry(provinceDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Province has been deleted successfully";
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
       
        public ActionResult Search(string searchModel)
        {
            ProvinceDefinition _searchObject = JsonConvert.DeserializeObject<ProvinceDefinition>(searchModel);
            List<ProvinceDefinition> _listObject = new List<ProvinceDefinition>();
            _listObject = db.ProvinceDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();

            if (!String.IsNullOrEmpty(_searchObject.ProvinceCode) && !String.IsNullOrEmpty(_searchObject.ProvinceName) && _searchObject.CountryId != 0 && _searchObject.CountryId != null)
            {
                _listObject = _listObject.Where(r => r.ProvinceCode != null && r.ProvinceCode.ToString().ToLower().Contains(_searchObject.ProvinceCode) || r.ProvinceName.ToString().ToLower().Contains(_searchObject.ProvinceName) || r.CountryId.ToString().ToLower().Contains(_searchObject.CountryId.ToString())).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ProvinceCode))
            {
                _listObject = _listObject.Where(r => r.ProvinceCode != null && r.ProvinceCode.ToString().ToLower().Contains(_searchObject.ProvinceCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ProvinceName))
            {
                _listObject = _listObject.Where(r => r.ProvinceName != null && r.ProvinceName.ToString().ToLower().Contains(_searchObject.ProvinceName.ToLower().ToString())).ToList();
            }
            else if (_searchObject.CountryId != 0 && _searchObject.CountryId != null)
            {
                _listObject = _listObject.Where(r => r.CountryId != null && r.CountryId.ToString().ToLower().Contains(_searchObject.CountryId.ToString())).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
