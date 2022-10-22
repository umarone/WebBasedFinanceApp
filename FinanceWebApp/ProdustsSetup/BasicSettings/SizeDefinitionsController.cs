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

namespace MudasirRehmanAlp.ProdustsSetup.BasicSettings
{
    public class SizeDefinitionsController : Controller
    {
        private AppEntities db = new AppEntities();
        AutoGenerateCodeDAL dALCode = new AutoGenerateCodeDAL();

        // GET: SizeDefinitions
        public ActionResult Index(int page = 1, int pageSize = 15)
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            List<SizeDefinition> listobj = new List<SizeDefinition>();
            listobj = db.SizeDefinitions.Where(a => a.IsDeleted == false && a.OrganizationId == OrganizationID).OrderByDescending(a=>a.Id).ToList();
            PagedList<SizeDefinition> model = new PagedList<SizeDefinition>(listobj, page, pageSize);
            return View(model);

        }

        // GET: SizeDefinitions/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeDefinition SizeDefinition = db.SizeDefinitions.Find(id);
            if (SizeDefinition == null)
            {
                return HttpNotFound();
            }
            return View(SizeDefinition);
        }
        public ActionResult Print(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeDefinition SizeDefinition = db.SizeDefinitions.Find(id);
            if (SizeDefinition == null)
            {
                return HttpNotFound();
            }
            return View(SizeDefinition);
        }

        // GET: SizeDefinitions/Create
        public ActionResult Create()
        {
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SizeDefinition obj = new SizeDefinition();
            obj.Code = dALCode.AutoSizeDefinitionsCode(OrganizationID/*, BranchId*/);
            var BrancheObj = dALCode.GetBranchDefinition(BranchId);
            obj.OrganizationId = BrancheObj.OrganizationId;
            obj.BranchId = BrancheObj.Id;
            ViewBag.OrganizationUnitName = BrancheObj.OrganizationDefinition.OrganizationUnitName;
            ViewBag.BranchName = BrancheObj.Name;
            return View(obj);
        }
      
        // POST: SizeDefinitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SizeDefinition sizeDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);

            if (ModelState.IsValid)
            {
                sizeDefinition.Code = dALCode.AutoSizeDefinitionsCode(OrganizationID/*, BranchId*/);
                sizeDefinition.IsDeleted = false;
                sizeDefinition.CreatedBy = UserID;
                sizeDefinition.CreatedDate = DateTime.Now;
                db.SizeDefinitions.Add(sizeDefinition);
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Size Definition has been saved successfully";

                return RedirectToAction("Index");
            }

            return View(sizeDefinition);
        }
        public ActionResult JsonCreate(string model)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            long OrganizationID = Convert.ToInt64(Session["OrganizationID"]);
            long BranchId = Convert.ToInt64(Session["BranchId"]);
            SizeDefinition sizeDefinition = JsonConvert.DeserializeObject<SizeDefinition>(model);
            
            if (ModelState.IsValid)
            {
                sizeDefinition.OrganizationId = OrganizationID;
                sizeDefinition.BranchId = BranchId;
                sizeDefinition.Code = dALCode.AutoSizeDefinitionsCode(OrganizationID/*, BranchId*/);
               
                sizeDefinition.IsActive = true;
                sizeDefinition.IsDeleted = false;
                sizeDefinition.CreatedBy = UserID;
                sizeDefinition.CreatedDate = DateTime.Now;
                db.SizeDefinitions.Add(sizeDefinition);
                db.SaveChanges();
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            return View(sizeDefinition);
        }
        // GET: SizeDefinitions/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeDefinition sizeDefinition = db.SizeDefinitions.Find(id);
            if (sizeDefinition == null)
            {
                return HttpNotFound();
            }
            return View(sizeDefinition);
        }

        // POST: SizeDefinitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SizeDefinition sizeDefinition)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);

            if (ModelState.IsValid)
            {

                sizeDefinition.IsDeleted = false;
                sizeDefinition.UpdatedBy = UserID;
                sizeDefinition.UpdatedDate = DateTime.Now;
                db.Entry(sizeDefinition).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Validation"] = true;
                TempData["ErrorMessage"] = "Size Definition has been updated successfully";
                return RedirectToAction("Index");
            }
            return View(sizeDefinition);
        }

        // GET: SizeDefinitions/Delete/5
        public ActionResult Delete(long? id)
        {
            long UserID = Convert.ToInt64(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SizeDefinition SizeDefinition = db.SizeDefinitions.Find(id);
            if (SizeDefinition == null)
            {
                return HttpNotFound();
            }

            SizeDefinition.IsDeleted = true;
            SizeDefinition.DeletedBy = UserID;
            SizeDefinition.DeletedDate = DateTime.Now;
            db.Entry(SizeDefinition).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Validation"] = true;
            TempData["ErrorMessage"] = "Size Definition has been deleted successfully";
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
            SizeDefinition _searchObject = JsonConvert.DeserializeObject<SizeDefinition>(searchModel);
            List<SizeDefinition> _listObject = new List<SizeDefinition>();
            _listObject = db.SizeDefinitions.Where(a => a.IsActive == true && a.IsDeleted == false && a.OrganizationId == OrganizationID && a.BranchId == BrancheId).ToList();
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
