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
    public class CountryDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        // GET: CountryDefinitions


        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            List<CountryDefinition> listobj = new List<CountryDefinition>();
            listobj = db.CountryDefinitions.Where(a => a.IsDeleted == false).ToList();
            PagedList<CountryDefinition> model = new PagedList<CountryDefinition>(listobj, page, pageSize);
            return View(model);
        }
        // GET: CountryDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountryDefinition countryDefinition = db.CountryDefinitions.Find(id);
            if (countryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(countryDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountryDefinition countryDefinition = db.CountryDefinitions.Find(id);
            if (countryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(countryDefinition);
        }
        // GET: CountryDefinitions/Create
        public ActionResult Create()
        {
            CountryDefinition obj = new CountryDefinition();
            obj.CountryCode = dALCode.AutoGenerateCountryCode();
            return View(obj);
        }

        // POST: CountryDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CountryDefinition countryDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                countryDefinition.CountryCode = dALCode.AutoGenerateCountryCode();
                countryDefinition.IsDeleted = false;
                countryDefinition.CreatedBy = UserID;
                countryDefinition.CreatedDate = DateTime.Now;
                db.CountryDefinitions.Add(countryDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Country has been saved successfully";
                return RedirectToAction("Index");
            }


            return View(countryDefinition);
        }

        // GET: CountryDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountryDefinition countryDefinition = db.CountryDefinitions.Find(id);
            if (countryDefinition == null)
            {
                return HttpNotFound();
            }
            return View(countryDefinition);
        }

        // POST: CountryDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CountryDefinition countryDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (ModelState.IsValid)
            {
                countryDefinition.UpdatedBy = UserID;
                countryDefinition.UpdatedDate = DateTime.Now;
                countryDefinition.IsDeleted = false;
                db.Entry(countryDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Country has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(countryDefinition);
        }
        
        // GET: CountryDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CountryDefinition countryDefinition = db.CountryDefinitions.Find(id);
            countryDefinition.DeletedBy = UserID;
            countryDefinition.DeletedDate = DateTime.Now;
            countryDefinition.IsDeleted = true;
            db.Entry(countryDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Country has been deleted successfully";
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
        public ActionResult Search(string searchModel)
        {
            CountryDefinition _searchObject = JsonConvert.DeserializeObject<CountryDefinition>(searchModel);
            List<CountryDefinition> _listObject = new List<CountryDefinition>();
            _listObject = db.CountryDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false).ToList();
            if (!String.IsNullOrEmpty(_searchObject.CountryCode) && !String.IsNullOrEmpty(_searchObject.CountryName))
            {
                _listObject = _listObject.Where(r => r.CountryCode != null && r.CountryCode.ToString().ToLower().Contains(_searchObject.CountryCode) || r.CountryName.ToString().ToLower().Contains(_searchObject.CountryName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CountryCode))
            {
                _listObject = _listObject.Where(r => r.CountryCode != null && r.CountryCode.ToString().ToLower().Contains(_searchObject.CountryCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.CountryName))
            {

                _listObject = _listObject.Where(r => r.CountryName != null && r.CountryName.ToString().ToLower().Contains(_searchObject.CountryName)).ToList();

            }
            return PartialView("_Search", _listObject);
        }
    }
}
