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
    public class ManufactureDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();
        
        // GET: ManufactureDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<ManufactureDefinition> listobj = new List<ManufactureDefinition>();
            listobj = db.ManufactureDefinitions.Where(a=>a.IsDeleted==false && a.OrganizationID == OrganizationID).OrderByDescending(a=>a.ManufactureId).ToList();
            PagedList<ManufactureDefinition> model = new PagedList<ManufactureDefinition>(listobj, page, pageSize);
            return View(model);
            
        }

        // GET: ManufactureDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManufactureDefinition manufactureDefinition = db.ManufactureDefinitions.Find(id);
            if (manufactureDefinition == null)
            {
                return HttpNotFound();
            }
            return View(manufactureDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManufactureDefinition manufactureDefinition = db.ManufactureDefinitions.Find(id);
            if (manufactureDefinition == null)
            {
                return HttpNotFound();
            }
            return View(manufactureDefinition);
        }

        // GET: ManufactureDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ManufactureDefinition obj = new ManufactureDefinition();
            obj.ManufactureCode = dALCode.AutoGenerateManufactureDefinitions(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationID = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }

        // POST: ManufactureDefinitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ManufactureDefinition manufactureDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            if (ModelState.IsValid)
            {
               
                manufactureDefinition.IsDeleted = false;
                manufactureDefinition.CreatedBy = UserID;
                manufactureDefinition.CreatedDate = DateTime.Now;
                manufactureDefinition.ManufactureCode = dALCode.AutoGenerateManufactureDefinitions(OrganizationID/*, BranchId*/);
                db.ManufactureDefinitions.Add(manufactureDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Manufacture has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(manufactureDefinition);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ManufactureDefinition manufactureDefinition = JsonConvert.DeserializeObject<ManufactureDefinition>(model);

            if (ModelState.IsValid)
            {
                manufactureDefinition.OrganizationID = OrganizationID;
                manufactureDefinition.BranchId = BranchId;
                manufactureDefinition.ManufactureCode = dALCode.AutoGenerateManufactureDefinitions(OrganizationID/*, BranchId*/);
                manufactureDefinition.IsActive = true;
                manufactureDefinition.IsDeleted = false;
                manufactureDefinition.CreatedBy = UserID;
                manufactureDefinition.CreatedDate = DateTime.Now;
                db.ManufactureDefinitions.Add(manufactureDefinition);
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return View(manufactureDefinition);
        }
        // GET: ManufactureDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManufactureDefinition manufactureDefinition = db.ManufactureDefinitions.Find(id);
            if (manufactureDefinition == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationUnitName = manufactureDefinition.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = manufactureDefinition.BranchDefinition.Name;
            return View(manufactureDefinition);
        }

        // POST: ManufactureDefinitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManufactureDefinition manufactureDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
           
            if (ModelState.IsValid)
            {
               
                manufactureDefinition.IsDeleted = false;
                manufactureDefinition.UpdatedBy = UserID;
                manufactureDefinition.UpdatedDate = DateTime.Now;
                db.Entry(manufactureDefinition).State = EntityState.Modified;
                db.SaveChanges();
                
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Manufacture has been updated successfully";

                return RedirectToAction("Index");
            }
            return View(manufactureDefinition);
        }

        // GET: ManufactureDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManufactureDefinition manufactureDefinition = db.ManufactureDefinitions.Find(id);
            if (manufactureDefinition == null)
            {
                return HttpNotFound();
            }
          
            manufactureDefinition.IsDeleted = true;
            manufactureDefinition.UpdatedBy = UserID;
            manufactureDefinition.UpdatedDate = DateTime.Now;
            db.Entry(manufactureDefinition).State = EntityState.Modified;
            db.SaveChanges();

            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Manufacture has been deleted successfully";

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
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            ManufactureDefinition _searchObject = JsonConvert.DeserializeObject<ManufactureDefinition>(searchModel);
            List<ManufactureDefinition> _listObject = new List<ManufactureDefinition>();
            _listObject = db.ManufactureDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.BranchId == BranchId).ToList();
            if (!String.IsNullOrEmpty(_searchObject.ManufactureCode) && !String.IsNullOrEmpty(_searchObject.ManufactureName))
            {
                _listObject = _listObject.Where(r => r.ManufactureCode != null && r.ManufactureCode.ToString().ToLower().Contains(_searchObject.ManufactureCode) || r.ManufactureName.ToString().ToLower().Contains(_searchObject.ManufactureName)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ManufactureCode))
            {
                _listObject = _listObject.Where(r => r.ManufactureCode != null && r.ManufactureCode.ToString().ToLower().Contains(_searchObject.ManufactureCode)).ToList();
            }
            else if (!String.IsNullOrEmpty(_searchObject.ManufactureName))
            {
                _listObject = _listObject.Where(r => r.ManufactureName != null && r.ManufactureName.ToString().ToLower().Contains(_searchObject.ManufactureName)).ToList();
            }
            return PartialView("_Search", _listObject);
        }
    }
}
